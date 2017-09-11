namespace Hidistro.UI.Web.Admin.AliFuwu
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.AlipayFuwu.Api.Model;
    using Hishop.AlipayFuwu.Api.Util;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class AliFuwuConfig : AdminPage
    {
        protected Button btnSave;
        protected HiddenField hdfCopyToken;
        protected HiddenField hdfCopyUrl;
        protected Button RSACreat;
        protected HiddenField RSAPublic;
        protected TextBox txtAliAppId;
        protected TextBox txtAliFollowTitle;
        protected Literal txtRSA;
        protected Literal txtToken;
        protected Literal txtUrl;

        protected AliFuwuConfig() : base("m11", "fwp01")
        {
        }

        private void BindData()
        {
            this.hdfCopyUrl.Value = this.txtUrl.Text = string.Format("http://{0}/api/AliPayFuwuApi.ashx", base.Request.Url.Host);
            this.hdfCopyToken.Value = this.txtToken.Text = string.Format("http://{0}/api/AliPayFuwuApi.ashx", base.Request.Url.Host);
            this.RSAPublic.Value = this.txtRSA.Text = RsaKeyHelper.GetRSAKeyContent(base.Server.MapPath("~/config/rsa_public_key.pem"), true);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            this.txtAliAppId.Text = masterSettings.AlipayAppid;
            this.txtAliFollowTitle.Text = masterSettings.AliOHFollowRelayTitle;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string str = this.txtAliAppId.Text.Trim();
            string str2 = this.txtAliFollowTitle.Text.Trim();
            if (str.Length < 14)
            {
                this.ShowMsg("请输入正确的AppId信息！", false);
            }
            else if (str2.Length < 2)
            {
                this.ShowMsg("关注消息，请不要留空，2个字符以上！", false);
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (masterSettings.AlipayAppid != str)
                {
                    MemberHelper.ClearAllAlipayopenId();
                    ScanHelp.ClearScanBind("ALIPAY");
                }
                masterSettings.AlipayAppid = str;
                masterSettings.AliOHFollowRelayTitle = str2;
                SettingsManager.Save(masterSettings);
                AlipayFuwuConfig.CommSetConfig(str, base.Server.MapPath("~/"), "GBK");
                this.ShowMsg("服务窗配置保存成功！", true);
                this.BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void RsaCreat_Click(object sender, EventArgs e)
        {
            string str = RsaKeyHelper.CreateRSAKeyFile(base.Server.MapPath("~/config/RSAGenerator/Rsa.exe"), base.Server.MapPath("~/config"), false);
            if (str == "success")
            {
                this.ShowMsg("新密钥对生成成功！", true);
                this.BindData();
            }
            else
            {
                this.ShowMsg("新密钥对生成失败" + str, false);
            }
        }
    }
}

