namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Bargain;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    [ParseChildren(true)]
    public class VInviteBargainDetial : VshopTemplatedWebControl
    {
        private int bargainId;
        private HtmlInputHidden hiddEndDate;
        private HtmlInputHidden hiddHasCollected;
        private HtmlInputHidden hiddProductId;
        private HtmlInputHidden hiddPurchaseNumber;
        private HtmlInputHidden hideDesc;
        private HtmlInputHidden hideImgUrl;
        private HtmlInputHidden hideTitle;
        private Literal litFloorPrice;
        private Literal litFloorPrice1;
        private Literal litParticipantNumber;
        private Literal litProdcutName;
        private Literal litProductCommentTotal;
        private Literal litProductConsultationTotal;
        private Literal litProductDesc;
        private Literal litPurcharseNum;
        private Literal litPurchaseNumber;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litStock;
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["bargainId"], out this.bargainId))
            {
                base.GotoResourceNotFound("");
            }
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.litFloorPrice = (Literal) this.FindControl("litFloorPrice");
            this.litFloorPrice1 = (Literal) this.FindControl("litFloorPrice1");
            this.litPurchaseNumber = (Literal) this.FindControl("litPurchaseNumber");
            this.litParticipantNumber = (Literal) this.FindControl("litParticipantNumber");
            this.litProductDesc = (Literal) this.FindControl("litProductDesc");
            this.litProductConsultationTotal = (Literal) this.FindControl("litProductConsultationTotal");
            this.litProductCommentTotal = (Literal) this.FindControl("litProductCommentTotal");
            this.litStock = (Literal) this.FindControl("litStock");
            this.litPurcharseNum = (Literal) this.FindControl("litPurcharseNum");
            this.hiddHasCollected = (HtmlInputHidden) this.FindControl("hiddHasCollected");
            this.hiddProductId = (HtmlInputHidden) this.FindControl("hiddProductId");
            this.hideTitle = (HtmlInputHidden) this.FindControl("hideTitle");
            this.hideImgUrl = (HtmlInputHidden) this.FindControl("hideImgUrl");
            this.hideDesc = (HtmlInputHidden) this.FindControl("hideDesc");
            this.hiddEndDate = (HtmlInputHidden) this.FindControl("hiddEndDate");
            this.hiddPurchaseNumber = (HtmlInputHidden) this.FindControl("hiddPurchaseNumber");
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.rptProductImages = (VshopTemplatedRepeater) this.FindControl("rptProductImages");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            bool flag = false;
            BargainInfo bargainInfo = BargainHelper.GetBargainInfo(this.bargainId);
            if (bargainInfo != null)
            {
                this.hideTitle.Value = bargainInfo.Title;
                this.hideDesc.Value = bargainInfo.Remarks;
                Uri url = HttpContext.Current.Request.Url;
                string activityCover = bargainInfo.ActivityCover;
                string str2 = string.Empty;
                if (!activityCover.StartsWith("http"))
                {
                    str2 = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
                }
                int id = Globals.RequestQueryNum("bargainDetialId");
                if (id > 0)
                {
                    BargainDetialInfo bargainDetialInfo = BargainHelper.GetBargainDetialInfo(id);
                    if ((currentMember == null) || (((currentMember != null) && (bargainDetialInfo != null)) && (bargainDetialInfo.UserId != currentMember.UserId)))
                    {
                        HttpContext.Current.Response.Redirect(string.Concat(new object[] { "HelpBargainDetial.aspx?bargainId=", this.bargainId, "&bargainDetialId=", id }));
                        HttpContext.Current.Response.End();
                    }
                }
                PageTitle.AddSiteNameTitle(bargainInfo.Title);
                this.litFloorPrice.Text = bargainInfo.FloorPrice.ToString("F2");
                this.litFloorPrice1.Text = bargainInfo.FloorPrice.ToString("F2");
                this.litPurchaseNumber.Text = (bargainInfo.ActivityStock - bargainInfo.TranNumber).ToString();
                this.litParticipantNumber.Text = BargainHelper.HelpBargainCount(this.bargainId).ToString();
                this.hiddEndDate.Value = bargainInfo.EndDate.ToString("yyyy:MM:dd:HH:mm:ss");
                this.hiddPurchaseNumber.Value = bargainInfo.PurchaseNumber.ToString();
                this.litStock.Text = bargainInfo.PurchaseNumber.ToString();
                this.litPurcharseNum.Text = bargainInfo.PurchaseNumber.ToString();
                if (bargainInfo.ProductId > 0)
                {
                    this.skuSelector.ProductId = bargainInfo.ProductId;
                    if (currentMember != null)
                    {
                        flag = ProductBrowser.CheckHasCollect(currentMember.UserId, bargainInfo.ProductId);
                    }
                    this.hiddHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
                    ProductInfo productDetails = ProductHelper.GetProductDetails(bargainInfo.ProductId);
                    this.hiddProductId.Value = bargainInfo.ProductId.ToString();
                    this.litProdcutName.Text = productDetails.ProductName;
                    this.litShortDescription.Text = bargainInfo.Remarks;
                    this.litSalePrice.Text = productDetails.MinSalePrice.ToString("f2");
                    this.litProductDesc.Text = productDetails.Description;
                    this.hideImgUrl.Value = string.IsNullOrEmpty(productDetails.ThumbnailUrl60) ? (str2 + activityCover) : (str2 + productDetails.ThumbnailUrl60);
                    if (this.rptProductImages != null)
                    {
                        string locationUrl = "javascript:;";
                        SlideImage[] imageArray = new SlideImage[] { new SlideImage(productDetails.ImageUrl1, locationUrl), new SlideImage(productDetails.ImageUrl2, locationUrl), new SlideImage(productDetails.ImageUrl3, locationUrl), new SlideImage(productDetails.ImageUrl4, locationUrl), new SlideImage(productDetails.ImageUrl5, locationUrl) };
                        this.rptProductImages.DataSource = from item in imageArray
                            where !string.IsNullOrWhiteSpace(item.ImageUrl)
                            select item;
                        this.rptProductImages.DataBind();
                    }
                    int productConsultationsCount = ProductBrowser.GetProductConsultationsCount(bargainInfo.ProductId, false);
                    this.litProductConsultationTotal.SetWhenIsNotNull(productConsultationsCount.ToString());
                    this.litProductCommentTotal.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(bargainInfo.ProductId).ToString());
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/default.aspx");
                HttpContext.Current.Response.End();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VInviteBargainDetial.html";
            }
            base.OnInit(e);
        }
    }
}

