namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VOneTaoPayView : VshopTemplatedWebControl
    {
        private Literal litActivityID;
        private Literal litBuyNum;
        private Literal litPayPrice;
        private Literal litPrice;
        private Literal litProdcutAttr;
        private Literal litProdcutName;
        private Literal litReachType;
        private HtmlContainerControl PayBtn;
        private HtmlContainerControl PayWaytxt;
        private HtmlImage ProductImg;

        protected override void AttachChildControls()
        {
            string activityId = "";
            string str2 = Globals.RequestQueryStr("Pid");
            if (string.IsNullOrEmpty(str2))
            {
                base.GotoResourceNotFound("");
            }
            OneyuanTaoParticipantInfo info = OneyuanTaoHelp.GetAddParticipant(0, str2, "");
            if (info == null)
            {
                base.GotoResourceNotFound("");
            }
            activityId = info.ActivityId;
            OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(activityId);
            if (oneyuanTaoInfoById == null)
            {
                base.GotoResourceNotFound("");
            }
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litProdcutAttr = (Literal) this.FindControl("litProdcutAttr");
            this.litActivityID = (Literal) this.FindControl("litActivityID");
            this.litReachType = (Literal) this.FindControl("litReachType");
            this.litPrice = (Literal) this.FindControl("litPrice");
            this.litBuyNum = (Literal) this.FindControl("litBuyNum");
            this.litPayPrice = (Literal) this.FindControl("litPayPrice");
            this.ProductImg = (HtmlImage) this.FindControl("ProductImg");
            this.PayWaytxt = (HtmlContainerControl) this.FindControl("PayWaytxt");
            this.PayBtn = (HtmlContainerControl) this.FindControl("PayBtn");
            this.litProdcutName.Text = oneyuanTaoInfoById.ProductTitle;
            this.litProdcutAttr.Text = info.SkuIdStr;
            this.litActivityID.Text = activityId;
            this.litReachType.Text = "";
            this.litPrice.Text = oneyuanTaoInfoById.EachPrice.ToString("F2");
            this.litBuyNum.Text = info.BuyNum.ToString();
            this.litPayPrice.Text = info.TotalPrice.ToString("F2");
            this.ProductImg.Src = oneyuanTaoInfoById.ProductImg;
            if (oneyuanTaoInfoById.ReachType == 1)
            {
                this.litReachType.Text = "满足参与人数自动开奖";
            }
            else if (oneyuanTaoInfoById.ReachType == 2)
            {
                this.litReachType.Text = "活动到期自动开奖";
            }
            else
            {
                this.litReachType.Text = "活动到期时且满足参与人数自动开奖";
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string str3 = "商家尚未开启网上支付功能！";
            this.PayBtn.Visible = false;
            if (!this.Page.Request.UserAgent.ToLower().Contains("micromessenger") && masterSettings.EnableAlipayRequest)
            {
                str3 = "支付宝手机支付";
                this.PayBtn.Visible = true;
            }
            else if (masterSettings.EnableWeiXinRequest && this.Page.Request.UserAgent.ToLower().Contains("micromessenger"))
            {
                str3 = "微信支付";
                this.PayBtn.Visible = true;
            }
            this.PayWaytxt.InnerText = "当前可用支付方式：" + str3;
            this.PayWaytxt.Attributes.Add("Pid", str2);
            PageTitle.AddSiteNameTitle("结算支付");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VOneTaoPayView.html";
            }
            base.OnInit(e);
        }
    }
}

