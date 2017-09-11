namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    public class hiRegionSelect : Page
    {
        private string selid = "";
        protected RegionSelector SelReggion;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.selid = base.Request.QueryString["selid"];
            if (!string.IsNullOrEmpty(this.selid))
            {
                this.SelReggion.SetSelectedRegionId(new int?(int.Parse(this.selid)));
            }
        }
    }
}

