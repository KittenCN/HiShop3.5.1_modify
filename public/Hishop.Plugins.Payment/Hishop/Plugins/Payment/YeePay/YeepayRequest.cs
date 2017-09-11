namespace Hishop.Plugins.Payment.YeePay
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    [Plugin("易宝支付")]
    public class YeepayRequest : PaymentRequest
    {
        private const string AddressFlag = "0";
        private readonly string amount;
        private const string Cur = "CNY";
        private readonly string merchantCallbackURL;
        private readonly string orderId;
        private const string SMctProperties = "YeePay";

        public YeepayRequest()
        {
            this.orderId = "";
            this.amount = "";
            this.merchantCallbackURL = "";
        }

        public YeepayRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.orderId = "";
            this.amount = "";
            this.merchantCallbackURL = "";
            this.amount = amount.ToString("F", CultureInfo.InvariantCulture);
            this.orderId = orderId;
            this.merchantCallbackURL = returnUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            this.RedirectToGateway(Buy.CreateUrl(this.MerchantId, this.KeyValue, this.orderId, this.amount, "CNY", "", this.merchantCallbackURL, "0", "YeePay", ""));
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool IsMedTrade
        {
            get
            {
                return false;
            }
        }

        [ConfigElement("商家密钥", Nullable=false)]
        public string KeyValue { get; set; }

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("商户号", Nullable=false)]
        public string MerchantId { get; set; }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        [ConfigElement("合作伙伴账号")]
        public string Pid { get; set; }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

