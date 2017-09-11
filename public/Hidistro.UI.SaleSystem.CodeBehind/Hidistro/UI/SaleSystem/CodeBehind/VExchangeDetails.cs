namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ControlPanel.Promotions;
    using global::ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    [ParseChildren(true)]
    public class VExchangeDetails : VshopTemplatedWebControl
    {
        private int exchangeId;
        private Common_ExpandAttributes expandAttr;
        private HtmlInputHidden hdCategoryId;
        private HtmlInputHidden hdEachCount;
        private HtmlInputHidden hdHasCollected;
        private HtmlInputHidden hdIsActive;
        private HtmlInputHidden hdIsInRange;
        private HtmlInputHidden hdPoint;
        private HtmlInputHidden hdProductId;
        private HtmlInputHidden hdStock;
        private HtmlInputHidden hdTemplateid;
        private HtmlInputHidden hdUserExchanged;
        private HyperLink linkDescription;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private Literal litEachCount;
        private Literal litItemParams;
        private Literal litMarketPrice;
        private Literal litProdcutName;
        private Literal litReviewsCount;
        private Literal litSalePoint;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litStock;
        private Literal litSurplusTime;
        private int productId;
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId) || !int.TryParse(this.Page.Request.QueryString["exchangeId"], out this.exchangeId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater) this.FindControl("rptProductImages");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litSalePoint = (Literal) this.FindControl("litSalePoint");
            this.litMarketPrice = (Literal) this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litSurplusTime = (Literal) this.FindControl("litSurplusTime");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litStock = (Literal) this.FindControl("litStock");
            this.litEachCount = (Literal) this.FindControl("litEachCount");
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.linkDescription = (HyperLink) this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes) this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal) this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal) this.FindControl("litReviewsCount");
            this.hdHasCollected = (HtmlInputHidden) this.FindControl("hdHasCollected");
            this.hdCategoryId = (HtmlInputHidden) this.FindControl("hdCategoryId");
            this.hdEachCount = (HtmlInputHidden) this.FindControl("hdEachCount");
            this.hdProductId = (HtmlInputHidden) this.FindControl("hdProductId");
            this.hdStock = (HtmlInputHidden) this.FindControl("hdStock");
            this.hdIsActive = (HtmlInputHidden) this.FindControl("hdIsActive");
            this.hdIsInRange = (HtmlInputHidden) this.FindControl("hdIsInRange");
            this.hdPoint = (HtmlInputHidden) this.FindControl("hdPoint");
            this.hdTemplateid = (HtmlInputHidden) this.FindControl("hdTemplateid");
            this.hdUserExchanged = (HtmlInputHidden) this.FindControl("hdUserExchanged");
            PointExChangeInfo info = PointExChangeHelper.Get(this.exchangeId);
            PointExchangeProductInfo productInfo = PointExChangeHelper.GetProductInfo(this.exchangeId, this.productId);
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            if (((info != null) && (product != null)) && (productInfo != null))
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (currentMember != null)
                {
                    this.hdPoint.Value = currentMember.Points.ToString();
                    if (MemberProcessor.CheckCurrentMemberIsInRange(info.MemberGrades, info.DefualtGroup, info.CustomGroup))
                    {
                        this.hdIsInRange.Value = "true";
                    }
                    else
                    {
                        this.hdIsInRange.Value = "false";
                    }
                }
                if (info.EndDate < DateTime.Now)
                {
                    this.litSurplusTime.Text = "已结束";
                    this.hdIsActive.Value = "0";
                }
                else if (info.BeginDate > DateTime.Now)
                {
                    this.litSurplusTime.Text = "未开始";
                    this.hdIsActive.Value = "0";
                }
                else
                {
                    this.hdIsActive.Value = "1";
                    TimeSpan span = (TimeSpan) (info.EndDate - DateTime.Now);
                    if (span.Days > 1)
                    {
                        this.litSurplusTime.Text = string.Concat(new object[] { "还剩", span.Days, "天", span.Hours, "小时" });
                    }
                    else
                    {
                        this.litSurplusTime.Text = "还剩" + span.Hours + "小时";
                    }
                }
                this.hdProductId.Value = this.productId.ToString();
                if (product == null)
                {
                    base.GotoResourceNotFound("此商品已不存在");
                }
                if (product.SaleStatus != ProductSaleStatus.OnSale)
                {
                    base.GotoResourceNotFound(ErrorType.前台商品下架, "此商品已下架");
                }
                if (this.rptProductImages != null)
                {
                    string locationUrl = "javascript:;";
                    SlideImage[] imageArray = new SlideImage[] { new SlideImage(product.ImageUrl1, locationUrl), new SlideImage(product.ImageUrl2, locationUrl), new SlideImage(product.ImageUrl3, locationUrl), new SlideImage(product.ImageUrl4, locationUrl), new SlideImage(product.ImageUrl5, locationUrl) };
                    this.rptProductImages.DataSource = from item in imageArray
                        where !string.IsNullOrWhiteSpace(item.ImageUrl)
                        select item;
                    this.rptProductImages.DataBind();
                }
                string mainCategoryPath = product.MainCategoryPath;
                if (!string.IsNullOrEmpty(mainCategoryPath))
                {
                    this.hdCategoryId.Value = mainCategoryPath.Split(new char[] { '|' })[0];
                }
                else
                {
                    this.hdCategoryId.Value = "0";
                }
                this.litProdcutName.Text = product.ProductName;
                this.hdTemplateid.Value = product.FreightTemplateId.ToString();
                this.litSalePoint.Text = productInfo.PointNumber.ToString();
                if (product.MarketPrice.HasValue && (product.MarketPrice > 0M))
                {
                    this.litMarketPrice.Text = "<del class=\"text-muted font-s\">\x00a5" + product.MarketPrice.Value.ToString("F2") + "</del>";
                }
                this.litShortDescription.Text = product.ShortDescription;
                string description = product.Description;
                if (!string.IsNullOrEmpty(description))
                {
                    description = Regex.Replace(description, "<img[^>]*\\bsrc=('|\")([^'\">]*)\\1[^>]*>", "<img alt='" + HttpContext.Current.Server.HtmlEncode(product.ProductName) + "' src='$2' />", RegexOptions.IgnoreCase);
                }
                if (this.litDescription != null)
                {
                    this.litDescription.Text = description;
                }
                this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
                int productExchangedCount = PointExChangeHelper.GetProductExchangedCount(this.exchangeId, this.productId);
                int num2 = ((productInfo.ProductNumber - productExchangedCount) >= 0) ? (productInfo.ProductNumber - productExchangedCount) : 0;
                this.litStock.Text = num2.ToString();
                this.hdStock.Value = num2.ToString();
                this.litEachCount.Text = productInfo.EachMaxNumber.ToString();
                this.hdEachCount.Value = productInfo.EachMaxNumber.ToString();
                this.skuSelector.ProductId = this.productId;
                if (this.expandAttr != null)
                {
                    this.expandAttr.ProductId = this.productId;
                }
                if (this.linkDescription != null)
                {
                    this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
                }
                int productConsultationsCount = ProductBrowser.GetProductConsultationsCount(this.productId, false);
                this.litConsultationsCount.SetWhenIsNotNull(productConsultationsCount.ToString());
                this.litReviewsCount.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(this.productId).ToString());
                bool flag = false;
                if (currentMember != null)
                {
                    this.hdUserExchanged.Value = PointExChangeHelper.GetUserProductExchangedCount(this.exchangeId, this.productId, currentMember.UserId).ToString();
                    flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
                    this.hdHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
                }
                ProductBrowser.UpdateVisitCounts(this.productId);
                PageTitle.AddSiteNameTitle("积分商品");
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                string objStr = "";
                if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
                {
                    objStr = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
                }
                this.litItemParams.Text = Globals.GetReplaceStr(objStr, "|", "｜") + "|" + Globals.GetReplaceStr(masterSettings.GoodsName, "|", "｜") + "|" + Globals.GetReplaceStr(masterSettings.GoodsDescription, "|", "｜") + "$" + Globals.HostPath(HttpContext.Current.Request.Url).Replace("|", "｜") + Globals.GetReplaceStr(product.ImageUrl1, "|", "｜") + "|" + Globals.GetReplaceStr(product.ProductName, "|", "｜") + "|" + Globals.GetReplaceStr(product.ShortDescription, "|", "｜") + "|" + HttpContext.Current.Request.Url.ToString().Replace("|", "｜");
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
                this.SkinName = "Skin-VExchangeDetails.html";
            }
            base.OnInit(e);
        }
    }
}

