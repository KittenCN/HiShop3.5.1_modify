namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VSaleService : VshopTemplatedWebControl
    {
        private Literal litSaleService;

        protected override void AttachChildControls()
        {
            this.litSaleService = (Literal) this.FindControl("litSaleService");
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.litSaleService.Text = masterSettings.SaleService;
            }
            PageTitle.AddSiteNameTitle("售后服务");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VSaleService.html";
            }
            base.OnInit(e);
        }
    }
}

