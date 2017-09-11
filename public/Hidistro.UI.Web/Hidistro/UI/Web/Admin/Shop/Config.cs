namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Config : AdminPage
    {
        protected Button btnSave;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;
        protected Script Script4;
        protected HtmlForm thisForm;
        protected TextBox txtShopIntroduction;
        protected TextBox txtShopTel;
        protected TextBox txtSiteName;

        protected Config() : base("m01", "dpp02")
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str = this.txtSiteName.Text.Trim();
            if ((str.Length < 1) || (str.Length > 10))
            {
                this.ShowMsg("请填写您的店铺名称，长度在10个字符以内", false);
            }
            else
            {
                string str2 = this.txtShopTel.Text.Trim();
                string str3 = this.txtShopIntroduction.Text.Trim();
                if (str3.Length > 60)
                {
                    this.ShowMsg("店铺介绍的长度不能超过60个字符", false);
                }
                else
                {
                    masterSettings.SiteName = str;
                    masterSettings.ShopIntroduction = str3;
                    masterSettings.ShopTel = str2;
                    SettingsManager.Save(masterSettings);
                    this.hidpicdel.Value = "";
                    this.ShowMsg("保存成功!", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtSiteName.Text = masterSettings.SiteName;
                this.txtShopIntroduction.Text = masterSettings.ShopIntroduction;
                this.hidpic.Value = masterSettings.DistributorLogoPic;
                this.txtShopTel.Text = masterSettings.ShopTel;
                if (!File.Exists(base.Server.MapPath(masterSettings.DistributorLogoPic)))
                {
                    this.hidpic.Value = "http://fpoimg.com/70x70";
                }
            }
        }
    }
}

