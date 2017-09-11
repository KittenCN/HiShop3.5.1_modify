namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class WXConfigBindOK : AdminPage
    {
        public string BindSetDesc;
        protected Button btnClearToken;
        protected Button btnSave;
        public string ChangeBindUrl;
        protected HiddenField hdfCopyToken;
        protected HiddenField hdfCopyUrl;
        protected Label lbAppId;
        private SiteSettings siteSettings;
        protected TextBox txtAppSecret;
        protected Literal txtToken;
        protected Literal txtUrl;

        protected WXConfigBindOK() : base("m06", "wxp01")
        {
            this.BindSetDesc = "立即绑定";
            this.ChangeBindUrl = "";
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void btnClearToken_Click(object sender, EventArgs e)
        {
            Globals.RefreshWeiXinToken();
            this.ShowMsgAndReUrl("刷新成功", true, "WXConfigBindOK.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.WeixinAppSecret = this.txtAppSecret.Text.Trim();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }

        private string CreateKey(int len)
        {
            byte[] data = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", data[i]));
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (string.IsNullOrEmpty(masterSettings.WeixinToken))
                {
                    masterSettings.WeixinToken = this.CreateKey(8);
                    SettingsManager.Save(masterSettings);
                }
                if (string.IsNullOrWhiteSpace(masterSettings.CheckCode))
                {
                    masterSettings.CheckCode = this.CreateKey(20);
                    SettingsManager.Save(masterSettings);
                }
                this.BindSetDesc = string.IsNullOrEmpty(masterSettings.WeixinAppId) ? "立即绑定" : "查询详情";
                this.hdfCopyUrl.Value = this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text);
                this.hdfCopyToken.Value = this.txtToken.Text = masterSettings.WeixinToken;
                this.lbAppId.Text = masterSettings.WeixinAppId;
                this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
                int bindOpenIDAndNoUserNameCount = MemberHelper.GetBindOpenIDAndNoUserNameCount();
                this.ChangeBindUrl = (bindOpenIDAndNoUserNameCount > 0) ? "WXConfigChangeBind.aspx" : "WXConfig.aspx";
            }
        }
    }
}

