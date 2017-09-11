namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Entities.Insurance;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using SqlDal.Insurance;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VFinishInsuranceOrder : VMemberTemplatedWebControl
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
        InsuranceDao dao = new InsuranceDao();
        protected override void AttachChildControls()
        {
            this.orderId = this.Page.Request.QueryString["orderId"];
            InsuranceOrderInfo info = dao.GetModel(Convert.ToInt32(this.orderId));
            decimal num = 0M;
            decimal num2 = 0M;
            num = info.InsuranceOrderAmount.Value;

            if (info == null)
            {
                this.Page.Response.Redirect("/Vshop/MemberInsuranceOrders.aspx");
            }
            bool flag = true;

            if(info.InsuranceOrderStatu!=2)
            {
                HttpContext.Current.Response.Write("<script>alert('未报价订单无法支付！');location.href='/Vshop/MemberInsuranceOrders.aspx'</script>");
                HttpContext.Current.Response.End();
            }

            string gateway = "hishop.plugins.payment.weixinrequest";

            int PaymentTypeId = 88;


            this.Page.Request.Url.ToString().ToLower();
            int num3 = Globals.RequestQueryNum("IsAlipay");
            string userAgent = this.Page.Request.UserAgent;
            if (((num3 == 1) || !userAgent.ToLower().Contains("micromessenger")) || (string.IsNullOrEmpty(gateway) || !(gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")))
            {
                if (!string.IsNullOrEmpty(gateway) && (gateway == "hishop.plugins.payment.offlinerequest"))
                {
                    this.litMessage = (Literal) this.FindControl("litMessage");
                    this.litMessage.SetWhenIsNotNull(SettingsManager.GetMasterSettings(false).OffLinePayContent);
                }
                this.litOPertorList = (Literal) this.FindControl("litOPertorList");
                this.litOPertorList.Text = "<div class=\"btns mt20\"><a id=\"linkToDetail\" class=\"btn btn-default mr10\" role=\"button\">查看订单</a><a href=\"/Default.aspx\" class=\"btn btn-default\" role=\"button\">继续逛逛</a></div>";
                if (!string.IsNullOrEmpty(gateway) && (gateway == "hishop.plugins.payment.weixinrequest"))
                {
                    string str2 = "立即支付";
                    if ((num2 > 0M) && ((num - num2) > 0M))
                    {
                        str2 = "还需支付 " + ((num - num2)).ToString("F2");
                    }
                    this.litOPertorList.Text = "<div class=\"mt20\"><a href=\"/pay/wx_Submit.aspx?orderId=" + this.orderId + "\" class=\"btn btn-danger\" role=\"button\" id=\"btnToPay\">" + str2 + "</a></div>";
                }
                if (((!string.IsNullOrEmpty(gateway) && (gateway != "hishop.plugins.payment.podrequest")) && ((gateway != "hishop.plugins.payment.offlinerequest") && (gateway != "hishop.plugins.payment.weixinrequest"))) && (((gateway != "hishop.plugins.payment.balancepayrequest") && (gateway != "hishop.plugins.payment.pointtocach")) && (gateway != "hishop.plugins.payment.coupontocach")))
                {
                    PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(PaymentTypeId);
                    string attach = "";
                    string showUrl = string.Format("http://{0}/vshop/", HttpContext.Current.Request.Url.Host);
                    PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), this.orderId, num - num2, "订单支付", "订单号-" + this.orderId, "3318681914@qq.com", info.InsuranceOrderCreatDate.Value, showUrl, Globals.FullPath("/pay/PaymentReturn_url.aspx"), Globals.FullPath("/pay/PaymentNotify_url.aspx"), attach).SendRequest();
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
                this.SkinName = "Skin-VFinishInsuranceOrder.html";
            }
            base.OnInit(e);
        }
    }
}

