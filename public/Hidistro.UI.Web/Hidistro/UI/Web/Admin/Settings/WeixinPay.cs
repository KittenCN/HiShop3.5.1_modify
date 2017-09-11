namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class WeixinPay : AdminPage
    {
        protected bool _enable;
        protected HtmlGenericControl alipaypanel;
        protected Button btnSave;
        protected HtmlInputCheckBox EnableSP;
        protected Label lblAppId;
        protected Label lblAppSecret;
        protected TextBox Main_AppId;
        protected TextBox Main_Mch_ID;
        protected TextBox Main_PayKey;
        protected Script Script1;
        private SiteSettings siteSettings;
        protected Hidistro.UI.Common.Controls.Style Style1;
        protected HtmlForm thisForm;
        protected TextBox txt_key;
        protected TextBox txt_mch_id;

        protected WeixinPay() : base("m06", "wxp08")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.siteSettings.WeixinAppId) && !string.IsNullOrEmpty(this.siteSettings.WeixinAppSecret))
                {
                    this.lblAppId.Text = this.siteSettings.WeixinAppId;
                    this.lblAppSecret.Text = this.siteSettings.WeixinAppSecret;
                }
                else
                {
                    this.lblAppSecret.Text = this.lblAppId.Text = "<a href='../weixin/wxconfig.aspx'>去设置</a>";
                    this.btnSave.Visible = false;
                }
                this.txt_key.Text = this.siteSettings.WeixinPartnerKey;
                this.txt_mch_id.Text = this.siteSettings.WeixinPartnerID;
                this.Main_PayKey.Text = this.siteSettings.Main_PayKey;
                this.Main_Mch_ID.Text = this.siteSettings.Main_Mch_ID;
                this.Main_AppId.Text = this.siteSettings.Main_AppId;
                this.EnableSP.Checked = this.siteSettings.EnableSP;
            }
            this._enable = this.siteSettings.EnableWeiXinRequest;
        }

        private void saveData()
        {
            if (string.IsNullOrEmpty(this.txt_key.Text.Trim()) && !this.EnableSP.Checked)
            {
                this.ShowMsg("请输入Key！", false);
            }
            else if (string.IsNullOrEmpty(this.txt_mch_id.Text.Trim()))
            {
                this.ShowMsg("请输入商户号mch_id！", false);
            }
            else
            {
                if (this.EnableSP.Checked)
                {
                    if (string.IsNullOrEmpty(this.Main_AppId.Text.Trim()))
                    {
                        this.ShowMsg("请输入服务商公众号！", false);
                        return;
                    }
                    if (string.IsNullOrEmpty(this.Main_Mch_ID.Text.Trim()))
                    {
                        this.ShowMsg("请输入服务商商户号mch_id！", false);
                        return;
                    }
                    if (string.IsNullOrEmpty(this.Main_PayKey.Text.Trim()))
                    {
                        this.ShowMsg("请输入服务商KEY！", false);
                        return;
                    }
                }
                else
                {
                    this.Main_PayKey.Text = "";
                    this.Main_Mch_ID.Text = "";
                    this.Main_AppId.Text = "";
                }
                this.siteSettings.WeixinPartnerKey = this.txt_key.Text.Trim();
                this.siteSettings.WeixinPartnerID = this.txt_mch_id.Text.Trim();
                this.siteSettings.Main_PayKey = this.Main_PayKey.Text.Trim();
                this.siteSettings.Main_Mch_ID = this.Main_Mch_ID.Text.Trim();
                this.siteSettings.Main_AppId = this.Main_AppId.Text.Trim();
                this.siteSettings.EnableSP = this.EnableSP.Checked;
                SettingsManager.Save(this.siteSettings);
                this.ShowMsg("保存成功！", true);
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            this.saveData();
        }
    }
}

