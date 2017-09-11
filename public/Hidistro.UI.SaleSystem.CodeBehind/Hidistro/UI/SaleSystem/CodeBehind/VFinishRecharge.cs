namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using System;
    using System.Web;

    public class VFinishRecharge : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            string payId = this.Page.Request.QueryString["PayId"];
            MemberAmountDetailedInfo amountDetailByPayId = MemberAmountProcessor.GetAmountDetailByPayId(payId);
            if (amountDetailByPayId == null)
            {
                this.Page.Response.Redirect("/Vshop/MemberRecharge.aspx");
            }
            this.Page.Request.Url.ToString().ToLower();
            int num = Globals.RequestQueryNum("IsAlipay");
            string userAgent = this.Page.Request.UserAgent;
            if (((num != 1) && userAgent.ToLower().Contains("micromessenger")) && (amountDetailByPayId.TradeWays == TradeWays.Alipay))
            {
                this.Page.Response.Redirect("/Pay/IframeAlipayCharge.aspx?PayId=" + payId);
            }
            else if (amountDetailByPayId.TradeWays == TradeWays.WeChatWallet)
            {
                this.Page.Response.Redirect("~/pay/wx_SubmitCharge.aspx?PayId=" + payId);
            }
            else if ((amountDetailByPayId.TradeWays != TradeWays.WeChatWallet) && (amountDetailByPayId.TradeWays != TradeWays.LineTransfer))
            {
                PaymentModeInfo paymentMode = MemberAmountProcessor.GetPaymentMode(amountDetailByPayId.TradeWays);
                string attach = "";
                string showUrl = string.Format("http://{0}/vshop/", HttpContext.Current.Request.Url.Host);
                PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), payId, amountDetailByPayId.TradeAmount, "会员充值", "充值号-" + payId, "", amountDetailByPayId.TradeTime, showUrl, Globals.FullPath("/pay/RePaymentReturn_url.aspx"), Globals.FullPath("/pay/RePaymentNotify_url.aspx"), attach).SendRequest();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VFinishRecharge.html";
            }
            base.OnInit(e);
        }
    }
}

