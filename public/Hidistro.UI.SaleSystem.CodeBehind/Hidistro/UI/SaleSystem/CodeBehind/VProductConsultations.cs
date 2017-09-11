namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VProductConsultations : VMemberTemplatedWebControl
    {
        private HtmlImage imgProductImage;
        private Literal litDescription;
        private Literal litProductTitle;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private int productId;
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound("");
            }
            ProductConsultationAndReplyQuery consultationQuery = new ProductConsultationAndReplyQuery();
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            consultationQuery.ProductId = this.productId;
            consultationQuery.IsCount = true;
            consultationQuery.PageIndex = num;
            consultationQuery.PageSize = num2;
            consultationQuery.SortBy = "ConsultationId";
            consultationQuery.SortOrder = SortAction.Desc;
            consultationQuery.HasReplied = true;
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(consultationQuery);
            this.rptProducts.DataSource = productConsultations.Data;
            this.rptProducts.DataBind();
            this.txtTotal.SetWhenIsNotNull(productConsultations.TotalRecords.ToString());
            this.litProductTitle = (Literal) this.FindControl("litProductTitle");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.imgProductImage = (HtmlImage) this.FindControl("imgProductImage");
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            this.litProductTitle.SetWhenIsNotNull(product.ProductName);
            this.litShortDescription.SetWhenIsNotNull(product.ShortDescription);
            this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
            this.litSalePrice.SetWhenIsNotNull(product.MinSalePrice.ToString("F2"));
            this.imgProductImage.Src = product.ThumbnailUrl60;
            PageTitle.AddSiteNameTitle("商品咨询");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductConsultations.html";
            }
            base.OnInit(e);
        }
    }
}

