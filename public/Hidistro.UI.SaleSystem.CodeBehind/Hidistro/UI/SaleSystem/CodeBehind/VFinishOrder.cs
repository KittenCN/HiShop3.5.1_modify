namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VFinishOrder : VMemberTemplatedWebControl
    {
        private Literal literalBalancePayInfo;
        private Literal literalOrderTotal;
        private Literal litHelperText;
        private Literal litMessage;
        private Literal litOPertorList;
        private Literal litOrderId;
        private Literal litOrderTotal;
        private HtmlInputHidden litPaymentType;
        private string orderId;

        protected override void AttachChildControls()
        {
            this.orderId = this.Page.Request.QueryString["orderId"];
            List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(this.orderId, false);
            decimal num = 0M;
            decimal num2 = 0M;
            if (orderMarkingOrderInfo.Count == 0)
            {
                this.Page.Response.Redirect("/Vshop/MemberOrders.aspx?status=0");
            }
            bool flag = true;

            if (orderMarkingOrderInfo[0].UserId != 0)
            {
                foreach (OrderInfo info in orderMarkingOrderInfo)
                {
                    if (info.OrderStatus != OrderStatus.BuyerAlreadyPaid)
                    {
                        num += info.GetTotal();
                        num2 += info.GetBalancePayMoneyTotal();
                        foreach (LineItemInfo info2 in info.LineItems.Values)
                        {
                            if (info2.Type == 0)
                            {
                                flag = false;
                            }
                            foreach (LineItemInfo info3 in info.LineItems.Values)
                            {
                                if (!ProductHelper.GetProductHasSku(info3.SkuId, info3.Quantity))
                                {
                                    info.OrderStatus = OrderStatus.Closed;
                                    info.CloseReason = "库存不足";
                                    OrderHelper.UpdateOrder(info);
                                    HttpContext.Current.Response.Write("<script>alert('库存不足，订单自动关闭！');location.href='/Vshop/MemberOrders.aspx'</script>");
                                    HttpContext.Current.Response.End();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                num = orderMarkingOrderInfo[0].Amount;
            }
            this.Page.Request.Url.ToString().ToLower();
            int num3 = Globals.RequestQueryNum("IsAlipay");
            string userAgent = this.Page.Request.UserAgent;
            if (((num3 == 1) || !userAgent.ToLower().Contains("micromessenger")) || (string.IsNullOrEmpty(orderMarkingOrderInfo[0].Gateway) || !(orderMarkingOrderInfo[0].Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")))
            {
                if (!string.IsNullOrEmpty(orderMarkingOrderInfo[0].Gateway) && (orderMarkingOrderInfo[0].Gateway == "hishop.plugins.payment.offlinerequest"))
                {
                    this.litMessage = (Literal) this.FindControl("litMessage");
                    this.litMessage.SetWhenIsNotNull(SettingsManager.GetMasterSettings(false).OffLinePayContent);
                }
                this.litOPertorList = (Literal) this.FindControl("litOPertorList");
                this.litOPertorList.Text = "<div class=\"btns mt20\"><a id=\"linkToDetail\" class=\"btn btn-default mr10\" role=\"button\">查看订单</a><a href=\"/Default.aspx\" class=\"btn btn-default\" role=\"button\">继续逛逛</a></div>";
                if (!string.IsNullOrEmpty(orderMarkingOrderInfo[0].Gateway) && (orderMarkingOrderInfo[0].Gateway == "hishop.plugins.payment.weixinrequest"))
                {
                    string str2 = "立即支付";
                    if ((num2 > 0M) && ((num - num2) > 0M))
                    {
                        str2 = "还需支付 " + ((num - num2)).ToString("F2");
                    }
                    this.litOPertorList.Text = "<div class=\"mt20\"><a href=\"/pay/wx_Submit.aspx?orderId=" + this.orderId + "\" class=\"btn btn-danger\" role=\"button\" id=\"btnToPay\">" + str2 + "</a></div>";
                }
                if (((!string.IsNullOrEmpty(orderMarkingOrderInfo[0].Gateway) && (orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.podrequest")) && ((orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.offlinerequest") && (orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.weixinrequest"))) && (((orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.balancepayrequest") && (orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.pointtocach")) && (orderMarkingOrderInfo[0].Gateway != "hishop.plugins.payment.coupontocach")))
                {
                    PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderMarkingOrderInfo[0].PaymentTypeId);
                    string attach = "";
                    string showUrl = string.Format("http://{0}/vshop/", HttpContext.Current.Request.Url.Host);
                    PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), this.orderId, num - num2, "订单支付", "订单号-" + this.orderId, orderMarkingOrderInfo[0].EmailAddress, orderMarkingOrderInfo[0].OrderDate, showUrl, Globals.FullPath("/pay/PaymentReturn_url.aspx"), Globals.FullPath("/pay/PaymentNotify_url.aspx"), attach).SendRequest();
                }
                else
                {
                    this.litOrderId = (Literal) this.FindControl("litOrderId");
                    this.litOrderTotal = (Literal) this.FindControl("litOrderTotal");
                    this.literalOrderTotal = (Literal) this.FindControl("literalOrderTotal");
                    this.literalBalancePayInfo = (Literal) this.FindControl("literalBalancePayInfo");
                    this.litPaymentType = (HtmlInputHidden) this.FindControl("litPaymentType");
                    int result = 0;
                    this.litPaymentType.SetWhenIsNotNull("0");
                    if (int.TryParse(this.Page.Request.QueryString["PaymentType"], out result))
                    {
                        this.litPaymentType.SetWhenIsNotNull(result.ToString());
                    }
                    this.litOrderId.SetWhenIsNotNull(this.orderId);
                    if (flag)
                    {
                        this.litOrderTotal.SetWhenIsNotNull("您需要支付：\x00a5" + num.ToString("F2"));
                    }
                    this.literalOrderTotal.SetWhenIsNotNull("订单金额：<span style='color:red'>\x00a5" + num.ToString("F2") + "</span>");
                    if (num2 > 0M)
                    {
                        this.literalBalancePayInfo.Text = "<div class='font-xl'>余额已支付：<span style='color:red'>\x00a5" + num2.ToString("F2") + "</span></div>";
                    }
                    this.litHelperText = (Literal) this.FindControl("litHelperText");
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    this.litHelperText.SetWhenIsNotNull(masterSettings.OffLinePayContent);
                    PageTitle.AddSiteNameTitle("下单成功");
                }
            }
            else
            {
                this.Page.Response.Redirect("/Pay/IframeAlipay.aspx?OrderId=" + this.orderId);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VFinishOrder.html";
            }
            base.OnInit(e);
        }
    }
}

