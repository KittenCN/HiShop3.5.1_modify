namespace Hidistro.UI.Web.Installer
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Configuration;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Succeed : Page
    {
        protected HtmlForm form1;
        protected Literal txtToken;
        protected Literal txtUrl;

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
            if (!string.IsNullOrEmpty(base.Request.QueryString["callback"]))
            {
                try
                {
                    System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
                    configuration.AppSettings.Settings.Remove("Installer");
                    configuration.Save();
                    base.Response.Write("true");
                }
                catch (Exception exception)
                {
                    base.Response.Write(exception.Message);
                }
                base.Response.End();
            }
            else
            {
                if ((base.Request.UrlReferrer == null) || (base.Request.UrlReferrer.OriginalString.IndexOf("Install.aspx") < 0))
                {
                    base.Response.Redirect("default.aspx");
                }
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (string.IsNullOrEmpty(masterSettings.WeixinToken))
                {
                    masterSettings.WeixinToken = this.CreateKey(8);
                    SettingsManager.Save(masterSettings);
                }
                this.txtToken.Text = masterSettings.WeixinToken;
                this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text);
            }
        }
    }
}

