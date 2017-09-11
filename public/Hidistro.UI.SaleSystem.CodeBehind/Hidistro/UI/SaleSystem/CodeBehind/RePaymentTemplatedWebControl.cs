namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class RePaymentTemplatedWebControl : SimpleTemplatedWebControl
    {
        protected decimal Amount;
        protected string Gateway;
        private readonly bool isBackRequest;
        protected MemberAmountDetailedInfo Model;
        protected PaymentNotify Notify;
        protected string PayId;

        public RePaymentTemplatedWebControl(bool _isBackRequest)
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
            Globals.Debuglog("充值支付：0-" + JsonConvert.SerializeObject(this.Notify), "_DebugAlipayPayNotify.txt");
            try
            {
                StringBuilder builder = new StringBuilder();
                foreach (string str in parameters)
                {
                    builder.Append(str + ":" + parameters[str] + "；");
                }
                Globals.Debuglog(builder.ToString(), "_DebugAlipayPayNotify.txt");
            }
            catch (Exception)
            {
            }
            if (this.isBackRequest)
            {
                this.Notify.ReturnUrl = Globals.FullPath("/pay/RePaymentReturn_url.aspx") + "?" + this.Page.Request.Url.Query;
            }
            Globals.Debuglog("充值支付：1-" + JsonConvert.SerializeObject(this.Notify), "_DebugAlipayPayNotify.txt");
            this.PayId = this.Notify.GetOrderId();
            this.Model = MemberAmountProcessor.GetAmountDetailByPayId(this.PayId);
            if (this.Model != null)
            {
                this.Amount = this.Model.TradeAmount;
                this.Model.GatewayPayId = this.Notify.GetGatewayOrderId();
                PaymentModeInfo paymentMode = MemberAmountProcessor.GetPaymentMode(this.Model.TradeWays);
                if (paymentMode == null)
                {
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

        private void Notify_Finished(object sender, FinishedEventArgs e)
        {
            this.UserPayOrder();
        }

        private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
        {
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
            if (this.Model.State == 1)
            {
                this.ResponseStatus(true, "success");
            }
            else if ((this.Model.TradeType == TradeType.Recharge) && MemberAmountProcessor.UserPayOrder(this.Model))
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

