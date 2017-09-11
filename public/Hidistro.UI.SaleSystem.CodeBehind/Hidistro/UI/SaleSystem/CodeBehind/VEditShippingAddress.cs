namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VEditShippingAddress : VMemberTemplatedWebControl
    {
        private HtmlTextArea address;
        private HtmlInputText cellphone;
        private RegionSelector dropRegions;
        private HtmlInputHidden Hiddenshipid;
        private HtmlInputHidden region;
        private HtmlInputHidden regionText;
        private int shippingid;
        private HtmlInputText shipTo;

        protected override void AttachChildControls()
        {
            this.shipTo = (HtmlInputText) this.FindControl("shipTo");
            this.address = (HtmlTextArea) this.FindControl("address");
            this.cellphone = (HtmlInputText) this.FindControl("cellphone");
            this.Hiddenshipid = (HtmlInputHidden) this.FindControl("shipId");
            this.regionText = (HtmlInputHidden) this.FindControl("regionText");
            this.region = (HtmlInputHidden) this.FindControl("region");
            ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(this.shippingid);
            if (shippingAddress == null)
            {
                this.Page.Response.Redirect("./ShippingAddresses.aspx", true);
            }
            string fullRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, " ");
            this.shipTo.Value = shippingAddress.ShipTo;
            this.address.Value = shippingAddress.Address;
            this.cellphone.Value = shippingAddress.CellPhone;
            this.Hiddenshipid.Value = this.shippingid.ToString();
            this.regionText.SetWhenIsNotNull(fullRegion);
            this.region.SetWhenIsNotNull(shippingAddress.RegionId.ToString());
            PageTitle.AddSiteNameTitle("编辑收货地址");
        }

        protected override void OnInit(EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["ShippingId"], out this.shippingid))
            {
                this.Page.Response.Redirect("./ShippingAddresses.aspx", true);
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Veditshippingaddress.html";
            }
            base.OnInit(e);
        }
    }
}

