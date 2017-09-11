namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ProductDetailsLink : HyperLink
    {
        public const string TagID = "ProductDetailsLink";

        public ProductDetailsLink()
        {
            base.ID = "ProductDetailsLink";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((this.ProductId != null) && (this.ProductId != DBNull.Value))
            {
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { this.ProductId });
                string str = Globals.ApplicationPath + "ProductDetails.aspx?ProductId=" + this.ProductId;
                if (Globals.GetCurrentDistributorId() > 0)
                {
                    base.NavigateUrl = str + "&&ReferralId=" + Globals.GetCurrentDistributorId();
                }
            }
            base.Target = "_blank";
            base.Render(writer);
        }

        public bool ImageLink { get; set; }

        public object ProductId { get; set; }
    }
}

