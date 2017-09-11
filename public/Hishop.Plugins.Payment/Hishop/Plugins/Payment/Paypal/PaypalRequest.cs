namespace Hishop.Plugins.Payment.Paypal
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Plugin("贝宝中国")]
    public class PaypalRequest : PaymentRequest
    {
        private readonly string amount;
        private const string Charset = "utf-8";
        private const string Cmd = "_xclick";
        private const string CurrencyCode = "CNY";
        private const string Custom = "PayPalStandard";
        private const string GatewayUrl = "https://www.paypal.com/cgi-bin/webscr";
        private readonly string invoice;
        private readonly string itemNumber;
        private const string NoNote = "1";
        private const string NoShipping = "1";
        private const string Quantity = "1";
        private readonly string returnUrl;
        private const string Rm = "2";
        private const string UndefinedQuantity = "0";

        public PaypalRequest()
        {
        }

        public PaypalRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.amount = amount.ToString("F", CultureInfo.InvariantCulture);
            this.invoice = orderId;
            this.itemNumber = orderId;
            this.returnUrl = returnUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("cmd", "_xclick"));
            builder.Append(this.CreateField("amount", this.amount));
            builder.Append(this.CreateField("invoice", this.invoice));
            builder.Append(this.CreateField("quantity", "1"));
            builder.Append(this.CreateField("undefined_quantity", "0"));
            builder.Append(this.CreateField("no_shipping", "1"));
            builder.Append(this.CreateField("return", this.returnUrl));
            builder.Append(this.CreateField("rm", "2"));
            builder.Append(this.CreateField("currency_code", "CNY"));
            builder.Append(this.CreateField("custom", "PayPalStandard"));
            builder.Append(this.CreateField("business", this.Business));
            builder.Append(this.CreateField("charset", "utf-8"));
            builder.Append(this.CreateField("no_note", "1"));
            builder.Append(this.CreateField("item_number", this.itemNumber));
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "https://www.paypal.com/cgi-bin/webscr"));
        }

        [ConfigElement("商户号", Nullable=false)]
        public string Business { get; set; }

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

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        protected override bool NeedProtect
        {
            get
            {
                return false;
            }
        }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

