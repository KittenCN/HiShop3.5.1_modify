namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VProductReview : VMemberTemplatedWebControl
    {
        private Literal litItemid;
        private Literal litOrderId;
        private Literal litPID;
        private Literal litProdcutName;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litSkuId;
        private Literal litSoldCount;
        private int productId;
        private HtmlImage productImage;
        private HyperLink productLink;
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
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.productImage = (HtmlImage) this.FindControl("productImage");
            this.productLink = (HyperLink) this.FindControl("productLink");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            this.litPID = (Literal) this.FindControl("litPID");
            this.litSkuId = (Literal) this.FindControl("litSkuId");
            this.litItemid = (Literal) this.FindControl("litItemid");
            this.litOrderId = (Literal) this.FindControl("litOrderId");
            string orderID = this.Page.Request["OrderId"];
            string str2 = "";
            this.litSkuId.Text = Globals.RequestQueryStr("skuid");
            this.litItemid.Text = Globals.RequestQueryNum("itemid").ToString();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (!string.IsNullOrEmpty(orderID))
            {
                this.litOrderId.Text = orderID;
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderID);
                if ((orderInfo != null) && (orderInfo.ReferralUserId > 0))
                {
                    str2 = "&&ReferralId=" + orderInfo.ReferralUserId;
                }
            }
            else
            {
                LineItemInfo latestOrderItemByProductIDAndUserID = ProductBrowser.GetLatestOrderItemByProductIDAndUserID(this.productId, currentMember.UserId);
                if (latestOrderItemByProductIDAndUserID != null)
                {
                    orderID = latestOrderItemByProductIDAndUserID.OrderID;
                    this.Page.Response.Redirect(string.Concat(new object[] { "ProductReview.aspx?OrderId=", orderID, "&ProductId=", latestOrderItemByProductIDAndUserID.ProductId, "&skuid=", latestOrderItemByProductIDAndUserID.SkuId, "&itemid=", latestOrderItemByProductIDAndUserID.ID }));
                    this.Page.Response.End();
                }
                if (Globals.GetCurrentDistributorId() > 0)
                {
                    str2 = "&&ReferralId=" + Globals.GetCurrentDistributorId().ToString();
                }
            }
            ProductInfo product = ProductBrowser.GetProduct(currentMember, this.productId);
            this.litProdcutName.SetWhenIsNotNull(product.ProductName);
            this.litSalePrice.SetWhenIsNotNull(product.MinSalePrice.ToString("F2"));
            this.litShortDescription.SetWhenIsNotNull(product.ShortDescription);
            this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
            this.productImage.Src = product.ThumbnailUrl180;
            this.litPID.Text = this.productId.ToString();
            this.productLink.NavigateUrl = "/ProductDetails.aspx?ProductId=" + product.ProductId + str2;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            ProductReviewQuery reviewQuery = new ProductReviewQuery {
                productId = this.productId,
                IsCount = true,
                PageIndex = num,
                PageSize = num2,
                SortBy = "ReviewId",
                SortOrder = SortAction.Desc
            };
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            DbQueryResult productReviews = ProductBrowser.GetProductReviews(reviewQuery);
            this.rptProducts.DataSource = productReviews.Data;
            this.rptProducts.DataBind();
            this.txtTotal.SetWhenIsNotNull(productReviews.TotalRecords.ToString());
            PageTitle.AddSiteNameTitle("商品评价");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductReview.html";
            }
            base.OnInit(e);
        }
    }
}

