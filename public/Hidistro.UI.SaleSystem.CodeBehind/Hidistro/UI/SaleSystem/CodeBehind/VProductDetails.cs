namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
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
    public class VProductDetails : VshopTemplatedWebControl
    {
        private Common_ExpandAttributes expandAttr;
        private HyperLink linkDescription;
        private HtmlInputHidden litCategoryId;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private HtmlInputHidden litHasCollected;
        private Literal litItemParams;
        private Literal litMarketPrice;
        private Literal litProdcutName;
        private Literal litProdcutTag;
        private HtmlInputHidden litproductid;
        private Literal litReviewsCount;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litStock;
        private HtmlInputHidden litTemplate;
        private int productId;
        private VshopTemplatedRepeater rptProductImages;
        private Common_SKUSelector skuSelector;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound("");
            }
            this.rptProductImages = (VshopTemplatedRepeater) this.FindControl("rptProductImages");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litProdcutTag = (Literal) this.FindControl("litProdcutTag");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.litMarketPrice = (Literal) this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litStock = (Literal) this.FindControl("litStock");
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.linkDescription = (HyperLink) this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes) this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal) this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal) this.FindControl("litReviewsCount");
            this.litHasCollected = (HtmlInputHidden) this.FindControl("litHasCollected");
            this.litCategoryId = (HtmlInputHidden) this.FindControl("litCategoryId");
            this.litproductid = (HtmlInputHidden) this.FindControl("litproductid");
            this.litTemplate = (HtmlInputHidden) this.FindControl("litTemplate");
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            if (product != null)
            {
                this.litproductid.Value = this.productId.ToString();
                this.litTemplate.Value = product.FreightTemplateId.ToString();
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
                    this.litCategoryId.Value = mainCategoryPath.Split(new char[] { '|' })[0];
                }
                else
                {
                    this.litCategoryId.Value = "0";
                }
                string productName = product.ProductName;
                string productTagName = ProductBrowser.GetProductTagName(this.productId);
                if (!string.IsNullOrEmpty(productTagName))
                {
                    this.litProdcutTag.Text = "<div class='y-shopicon'>" + productTagName.Trim() + "</div>";
                    productTagName = "<span class='producttag'>【" + HttpContext.Current.Server.HtmlEncode(productTagName) + "】</span>";
                }
                this.litProdcutName.Text = productTagName + productName;
                if (product.MinSalePrice != product.MaxSalePrice)
                {
                    this.litSalePrice.Text = product.MinSalePrice.ToString("F2") + "~" + product.MaxSalePrice.ToString("F2");
                }
                else
                {
                    this.litSalePrice.Text = product.MinSalePrice.ToString("F2");
                }
                if (product.MarketPrice.HasValue && (product.MarketPrice > 0M))
                {
                    this.litMarketPrice.Text = "<del class=\"text-muted font-s\">\x00a5" + product.MarketPrice.Value.ToString("F2") + "</del>";
                }
                this.litShortDescription.Text = product.ShortDescription;
                string description = product.Description;
                if (!string.IsNullOrEmpty(description))
                {
                    description = Regex.Replace(description, "<img[^>]*\\bsrc=('|\")([^'\">]*)\\1[^>]*>", "<img alt='" + HttpContext.Current.Server.HtmlEncode(productName) + "' src='$2' />", RegexOptions.IgnoreCase);
                }
                if (this.litDescription != null)
                {
                    this.litDescription.Text = description;
                }
                this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
                this.litStock.Text = product.Stock.ToString();
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
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                bool flag = false;
                if (currentMember != null)
                {
                    flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
                }
                this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
                ProductBrowser.UpdateVisitCounts(this.productId);
                PageTitle.AddSiteNameTitle(productName);
                PageTitle.AddSiteDescription(product.ShortDescription);
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
                this.SkinName = "Skin-VProductDetails.html";
            }
            base.OnInit(e);
        }
    }
}

