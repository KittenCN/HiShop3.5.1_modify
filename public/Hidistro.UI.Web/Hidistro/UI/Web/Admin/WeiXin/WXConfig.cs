namespace Hidistro.UI.Web.Admin.WeiXin
{
    using  global:: ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class WXConfig : AdminPage
    {
        protected Button btnSave;
        protected HiddenField hdfCopyToken;
        protected HiddenField hdfCopyUrl;
        private SiteSettings siteSettings;
        protected TextBox txtAppId;
        protected TextBox txtAppSecret;
        protected Literal txtToken;
        protected Literal txtUrl;

        protected WXConfig() : base("m06", "wxp01")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (masterSettings.WeixinAppId != this.txtAppId.Text.Trim())
            {
                WeiXinHelper.ClearWeiXinMediaID();
                MemberHelper.ClearAllOpenId();
                ScanHelp.ClearScanBind("WX");
            }
            masterSettings.WeixinAppId = this.txtAppId.Text.Trim();
            masterSettings.WeixinAppSecret = this.txtAppSecret.Text.Trim();
            SettingsManager.Save(masterSettings);
            Globals.RefreshWeiXinToken();
            this.ShowMsgAndReUrl("修改成功", true, "WXConfigBindOK.aspx");
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
                if (!string.IsNullOrWhiteSpace(masterSettings.WeixinAppId) && string.IsNullOrEmpty(base.Request.QueryString["reset"]))
                {
                    base.Response.Redirect("WXConfigBindOK.aspx");
                }
                this.hdfCopyUrl.Value = this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text);
                this.hdfCopyToken.Value = this.txtToken.Text = masterSettings.WeixinToken;
                this.txtAppId.Text = masterSettings.WeixinAppId;
                this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
            }
        }
    }
}

