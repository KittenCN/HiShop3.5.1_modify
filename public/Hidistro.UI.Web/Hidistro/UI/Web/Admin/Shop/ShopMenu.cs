namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class ShopMenu : AdminPage
    {
        protected bool _enable;
        protected CheckBox ActivityMenu;
        protected CheckBox BrandMenu;
        protected Button BtnSave;
        protected CheckBox DistributorsMenu;
        protected CheckBox GoodsCheck;
        protected CheckBox GoodsListMenu;
        protected CheckBox GoodsType;
        protected CheckBox MemberDefault;
        protected RadioButtonList RadioType;
        protected CheckBox ShopDefault;

        protected ShopMenu() : base("m01", "dpp04")
        {
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if ((!this.ShopDefault.Checked && !this.MemberDefault.Checked) && (!this.GoodsType.Checked && !this.GoodsCheck.Checked))
            {
                this.ShowMsg("请至少选择一个显示店铺导航的页面", false);
            }
            else
            {
                masterSettings.ShopDefault = this.ShopDefault.Checked;
                masterSettings.MemberDefault = this.MemberDefault.Checked;
                masterSettings.GoodsType = this.GoodsType.Checked;
                masterSettings.GoodsCheck = this.GoodsCheck.Checked;
                masterSettings.ShopMenuStyle = this.RadioType.SelectedValue;
                masterSettings.ActivityMenu = this.ActivityMenu.Checked;
                masterSettings.DistributorsMenu = this.DistributorsMenu.Checked;
                masterSettings.GoodsListMenu = this.GoodsListMenu.Checked;
                masterSettings.BrandMenu = this.BrandMenu.Checked;
                this.ShowMsg("保存成功", true);
                SettingsManager.Save(masterSettings);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.BtnSave.Click += new EventHandler(this.BtnSave_Click);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (!base.IsPostBack)
            {
                this.ShopDefault.Checked = masterSettings.ShopDefault;
                this.MemberDefault.Checked = masterSettings.MemberDefault;
                this.GoodsType.Checked = masterSettings.GoodsType;
                this.GoodsCheck.Checked = masterSettings.GoodsCheck;
                this.ActivityMenu.Checked = masterSettings.ActivityMenu;
                this.DistributorsMenu.Checked = masterSettings.DistributorsMenu;
                this.GoodsListMenu.Checked = masterSettings.GoodsListMenu;
                this.BrandMenu.Checked = masterSettings.BrandMenu;
                this.RadioType.SelectedValue = masterSettings.ShopMenuStyle;
            }
            this._enable = masterSettings.EnableShopMenu;
        }
    }
}

