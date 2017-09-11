namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class PaymentTemplatedWebControl : SimpleTemplatedWebControl
    {
        protected decimal Amount;
        protected string Gateway;
        private readonly bool isBackRequest;
        protected PaymentNotify Notify;
        protected OrderInfo Order;
        protected string OrderId;
        protected List<OrderInfo> orderlist;

        public PaymentTemplatedWebControl(bool _isBackRequest)
        {
            this.isBackRequest = _isBackRequest;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!this.isBackRequest)
            {
                if (!base.LoadHtmlThemedControl())
                {
                    throw new SkinNotFoundException(this.SkinPath);
                }
                this.AttachChildControls();
            }
            this.DoValidate();
        }

        protected abstract void DisplayMessage(string status);
        private void DoValidate()
        {
            NameValueCollection values2 = new NameValueCollection();
            values2.Add(this.Page.Request.Form);
            values2.Add(this.Page.Request.QueryString);
            NameValueCollection parameters = values2;
            if (!this.isBackRequest)
            {
                parameters.Add("IsReturn", "true");
            }
            this.Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest";
            this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
            if (this.isBackRequest)
            {
                this.Notify.ReturnUrl = Globals.FullPath("/pay/PaymentReturn_url.aspx") + "?" + this.Page.Request.Url.Query;
            }
            this.OrderId = this.Notify.GetOrderId();
            string gatewayOrderId = this.Notify.GetGatewayOrderId();
            if (string.IsNullOrEmpty(this.OrderId))
            {
                Globals.Debuglog(" OrderId:没获取到,GetewayOrderId:" + gatewayOrderId, "_DebuglogPaymentTest.txt");
                this.ResponseStatus(true, "noorderId");
            }
            else
            {
                this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId, false);
                if (this.orderlist.Count == 0)
                {
                    Globals.Debuglog("更新订单失败，也许是订单已后台付款，OrderId:" + this.OrderId, "_DebugAlipayPayNotify.txt");
                    this.ResponseStatus(true, "nodata");
                }
                else
                {
                    int modeId = 0;
                    foreach (OrderInfo info in this.orderlist)
                    {
                        this.Amount += info.GetTotal();
                        info.GatewayOrderId = gatewayOrderId;
                        modeId = info.PaymentTypeId;
                    }
                    PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(modeId);
                    if (paymentMode == null)
                    {
                        Globals.Debuglog("gatewaynotfound" + this.OrderId, "_DebugAlipayPayNotify.txt");
                        this.ResponseStatus(true, "gatewaynotfound");
                    }
                    else
                    {
                        this.Notify.Finished += new EventHandler<FinishedEventArgs>(this.Notify_Finished);
                        this.Notify.NotifyVerifyFaild += new EventHandler(this.Notify_NotifyVerifyFaild);
                        this.Notify.Payment += new EventHandler(this.Notify_Payment);
                        string configXml = HiCryptographer.Decrypt(paymentMode.Settings);
                        this.Notify.VerifyNotify(0x7530, configXml);
                    }
                }
            }
        }

        private void FinishOrder()
        {
            int num = 0;
            int num2 = 0;
            foreach (OrderInfo info in this.orderlist)
            {
                num++;
                if (info.OrderStatus == OrderStatus.Finished)
                {
                    num2++;
                }
            }
            if ((num2 > 0) && (num == num2))
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                num2 = 0;
                num = 0;
                foreach (OrderInfo info2 in this.orderlist)
                {
                    num++;
                    if (info2.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && MemberProcessor.ConfirmOrderFinish(info2))
                    {
                        num2++;
                    }
                }
                if ((num2 > 0) && (num == num2))
                {
                    this.ResponseStatus(true, "success");
                }
                else
                {
                    this.ResponseStatus(false, "fail");
                }
            }
        }

        private void Notify_Finished(object sender, FinishedEventArgs e)
        {
            if (e.IsMedTrade)
            {
                this.FinishOrder();
            }
            else
            {
                this.UserPayOrder();
            }
        }

        private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
        {
            Globals.Debuglog("验证失败", "_DebuglogAlipayFaild.txt");
            try
            {
                NameValueCollection values2 = new NameValueCollection();
                values2.Add(this.Page.Request.Form);
                values2.Add(this.Page.Request.QueryString);
                NameValueCollection values = values2;
                StringBuilder builder = new StringBuilder();
                foreach (string str in values)
                {
                    builder.Append(str + ":" + values[str] + "；");
                }
                Globals.Debuglog(builder.ToString(), "_DebuglogAlipayFaild.txt");
            }
            catch (Exception)
            {
            }
            this.ResponseStatus(false, "verifyfaild");
        }

        private void Notify_Payment(object sender, EventArgs e)
        {
            this.UserPayOrder();
        }

        private void ResponseStatus(bool success, string status)
        {
            if (this.isBackRequest)
            {
                this.Notify.WriteBack(HttpContext.Current, success);
            }
            else
            {
                this.DisplayMessage(status);
            }
        }

        private void UserPayOrder()
        {
            int num = 0;
            int num2 = 0;
            foreach (OrderInfo info in this.orderlist)
            {
                num++;
                if (info.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                {
                    num2++;
                }
            }
            if ((num2 > 0) && (num == num2))
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                num2 = 0;
                num = 0;
                foreach (OrderInfo info2 in this.orderlist)
                {
                    num++;
                    if (info2.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(info2))
                    {
                        num2++;
                        info2.OnPayment();
                    }
                }
                if ((num2 > 0) && (num == num2))
                {
                    this.ResponseStatus(true, "success");
                }
                else
                {
                    this.ResponseStatus(false, "fail");
                }
            }
        }
    }
}

