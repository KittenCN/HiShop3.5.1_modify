namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    public class ViewOneTao : VMemberTemplatedWebControl
    {
        private HtmlControl buyNum;
        private Common_ExpandAttributes expandAttr;
        private Literal litActivityId;
        private Literal litBuytxt;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private Literal litFinished;
        private Literal litItemParams;
        private Literal litMarketPrice;
        private Literal litMaxtxt;
        private Literal litMinNum;
        private Literal litPrizeNum;
        private Literal litProdcutName;
        private Literal litProdcutTag;
        private Literal litReviewsCount;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litState;
        private Literal litStock;
        private HtmlContainerControl NomachMember;
        private HtmlControl Prizeprogress;
        private HtmlControl PrizeTime;
        private int productId;
        private VshopTemplatedRepeater rptProductImages;
        private HtmlControl SaveBtn;
        private Common_SKUSelector skuSelector;
        private string Vaid = "";
        private HtmlControl ViewtReview;

        protected override void AttachChildControls()
        {
            this.Vaid = Globals.RequestQueryStr("vaid");
            if (string.IsNullOrEmpty(this.Vaid))
            {
                base.GotoResourceNotFound("");
            }
            OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(this.Vaid);
            if (oneyuanTaoInfoById == null)
            {
                base.GotoResourceNotFound("");
            }
            this.productId = oneyuanTaoInfoById.ProductId;
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), this.productId);
            if (product == null)
            {
                base.GotoResourceNotFound("");
            }
            OneTaoState state = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
            this.rptProductImages = (VshopTemplatedRepeater) this.FindControl("rptProductImages");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litProdcutTag = (Literal) this.FindControl("litProdcutTag");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.litMarketPrice = (Literal) this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litStock = (Literal) this.FindControl("litStock");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal) this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal) this.FindControl("litReviewsCount");
            this.litActivityId = (Literal) this.FindControl("litActivityId");
            this.litState = (Literal) this.FindControl("litState");
            this.PrizeTime = (HtmlControl) this.FindControl("PrizeTime");
            this.buyNum = (HtmlControl) this.FindControl("buyNum");
            this.SaveBtn = (HtmlControl) this.FindControl("SaveBtn");
            this.ViewtReview = (HtmlControl) this.FindControl("ViewtReview");
            this.litMaxtxt = (Literal) this.FindControl("litMaxtxt");
            this.expandAttr = (Common_ExpandAttributes) this.FindControl("ExpandAttributes");
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.NomachMember = (HtmlContainerControl) this.FindControl("NomachMember");
            this.litMinNum = (Literal) this.FindControl("litMinNum");
            this.litPrizeNum = (Literal) this.FindControl("litPrizeNum");
            this.litFinished = (Literal) this.FindControl("litFinished");
            this.Prizeprogress = (HtmlControl) this.FindControl("Prizeprogress");
            this.litBuytxt = (Literal) this.FindControl("litBuytxt");
            this.litPrizeNum.Text = oneyuanTaoInfoById.ReachNum.ToString();
            this.litFinished.Text = oneyuanTaoInfoById.FinishedNum.ToString();
            int num = oneyuanTaoInfoById.ReachNum - oneyuanTaoInfoById.FinishedNum;
            if (num < 0)
            {
                num = 0;
            }
            this.litMinNum.Text = num.ToString();
            float num2 = (100 * oneyuanTaoInfoById.FinishedNum) / oneyuanTaoInfoById.ReachNum;
            this.Prizeprogress.Attributes.Add("style", "width:" + num2.ToString("F0") + "%");
            this.ViewtReview.Attributes.Add("href", "ProductReview.aspx?ProductId=" + oneyuanTaoInfoById.ProductId.ToString());
            if (this.expandAttr != null)
            {
                this.expandAttr.ProductId = this.productId;
            }
            this.skuSelector.ProductId = this.productId;
            if (product != null)
            {
                if (this.rptProductImages != null)
                {
                    string locationUrl = "javascript:;";
                    SlideImage[] imageArray = new SlideImage[] { new SlideImage(product.ImageUrl1, locationUrl), new SlideImage(product.ImageUrl2, locationUrl), new SlideImage(product.ImageUrl3, locationUrl), new SlideImage(product.ImageUrl4, locationUrl), new SlideImage(product.ImageUrl5, locationUrl) };
                    this.rptProductImages.DataSource = from item in imageArray
                        where !string.IsNullOrWhiteSpace(item.ImageUrl)
                        select item;
                    this.rptProductImages.DataBind();
                }
                this.litShortDescription.Text = product.ShortDescription;
            }
            int num3 = OneyuanTaoHelp.MermberCanbuyNum(oneyuanTaoInfoById.ActivityId, Globals.GetCurrentMemberUserId(false));
            this.buyNum.Attributes.Add("max", num3.ToString());
            this.litBuytxt.Text = string.Concat(new object[] { "限购", oneyuanTaoInfoById.EachCanBuyNum, "份，每份价格￥", oneyuanTaoInfoById.EachPrice.ToString("F2") });
            this.litMaxtxt.Text = "您已订购<di>" + ((oneyuanTaoInfoById.EachCanBuyNum - num3)).ToString() + "</di>份";
            if (((num3 == 0) || (state != OneTaoState.进行中)) || !MemberHelper.CheckCurrentMemberIsInRange(oneyuanTaoInfoById.FitMember, oneyuanTaoInfoById.DefualtGroup, oneyuanTaoInfoById.CustomGroup, base.CurrentMemberInfo.UserId))
            {
                this.buyNum.Attributes.Add("disabled", "disabled");
                this.SaveBtn.Visible = false;
            }
            string str2 = "全部会员";
            if ((oneyuanTaoInfoById.FitMember == "0") || (oneyuanTaoInfoById.CustomGroup == "0"))
            {
                str2 = "全部会员";
            }
            else
            {
                str2 = "部分会员";
            }
            str2 = "适用会员：" + str2;
            if ((oneyuanTaoInfoById.FitMember != "0") && !MemberHelper.CheckCurrentMemberIsInRange(oneyuanTaoInfoById.FitMember, oneyuanTaoInfoById.DefualtGroup, oneyuanTaoInfoById.CustomGroup, base.CurrentMemberInfo.UserId))
            {
                str2 = "会员等级不符合活动要求";
                this.NomachMember.Attributes.Add("CanBuy", "false");
            }
            this.NomachMember.InnerHtml = str2;
            string productName = product.ProductName;
            string description = product.Description;
            if (!string.IsNullOrEmpty(description))
            {
                description = Regex.Replace(description, "<img[^>]*\\bsrc=('|\")([^'\">]*)\\1[^>]*>", "<img alt='" + HttpContext.Current.Server.HtmlEncode(productName) + "' src='$2' />", RegexOptions.IgnoreCase);
            }
            if (this.litDescription != null)
            {
                this.litDescription.Text = description;
            }
            this.litProdcutName.Text = productName;
            this.litSalePrice.Text = product.MinSalePrice.ToString("F2");
            this.litActivityId.Text = oneyuanTaoInfoById.ActivityId;
            if (oneyuanTaoInfoById.ReachType == 1)
            {
                this.litActivityId.Text = "活动结束前满足总需份数，自动开出" + oneyuanTaoInfoById.PrizeNumber + "个奖品";
            }
            else if (oneyuanTaoInfoById.ReachType == 2)
            {
                this.litActivityId.Text = "活动到期自动开出" + oneyuanTaoInfoById.PrizeNumber + "个奖品";
            }
            else if (oneyuanTaoInfoById.ReachType == 3)
            {
                this.litActivityId.Text = "到开奖时间并满足总需份数，自动开出" + oneyuanTaoInfoById.PrizeNumber + "个奖品";
            }
            this.PrizeTime.Attributes.Add("PrizeTime", oneyuanTaoInfoById.EndTime.ToString("G"));
            this.litState.Text = state.ToString();
            if (state == OneTaoState.已开奖)
            {
                IsoDateTimeConverter converter = new IsoDateTimeConverter {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
                Literal literal = (Literal) this.FindControl("LitDataJson");
                IList<LuckInfo> list = OneyuanTaoHelp.getWinnerLuckInfoList(oneyuanTaoInfoById.ActivityId, "");
                if (list != null)
                {
                    literal.Text = "var LitDataJson=" + JsonConvert.SerializeObject(list, new JsonConverter[] { converter });
                }
                else
                {
                    literal.Text = "var LitDataJson=null";
                }
            }
            Literal literal2 = (Literal) this.FindControl("litJs");
            string title = oneyuanTaoInfoById.Title;
            string activityDec = oneyuanTaoInfoById.ActivityDec;
            Uri url = this.Context.Request.Url;
            string str7 = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
            string productImg = oneyuanTaoInfoById.ProductImg;
            if (productImg == "/utility/pics/none.gif")
            {
                productImg = oneyuanTaoInfoById.HeadImgage;
            }
            literal2.Text = "<script>wxinshare_title=\"" + this.Context.Server.HtmlEncode(title.Replace("\n", " ").Replace("\r", "")) + "\";wxinshare_desc=\"" + this.Context.Server.HtmlEncode(activityDec.Replace("\n", " ").Replace("\r", "")) + "\";wxinshare_link=location.href;wxinshare_imgurl=\"" + str7 + productImg + "\"</script>";
            PageTitle.AddSiteNameTitle("一元夺宝商品详情");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ViewOneTao.html";
            }
            base.OnInit(e);
        }
    }
}

