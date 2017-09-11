namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VAddShippingAddress : VMemberTemplatedWebControl
    {
        private RegionSelector dropRegions;

        protected override void AttachChildControls()
        {
            this.dropRegions = (RegionSelector) this.FindControl("dropRegions");
            PageTitle.AddSiteNameTitle("添加收货地址");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Vaddshippingaddress.html";
            }
            base.OnInit(e);
        }
    }
}

