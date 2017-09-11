namespace Hishop.Plugins.Payment.Allbuy
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Plugin("AllBuy")]
    public class AllbuyRequest : PaymentRequest
    {
        private readonly string amount;
        private readonly string backUrl;
        private readonly string billNo;
        private readonly string date;
        private const string GatewayUrl = "http://www.allbuy.cn/newpayment/payment.asp";
        private const string Remark = "Allbuy";

        public AllbuyRequest()
        {
            this.billNo = "";
            this.amount = "";
            this.date = "";
            this.backUrl = "";
        }

        public AllbuyRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.billNo = "";
            this.amount = "";
            this.date = "";
            this.backUrl = "";
            this.billNo = orderId;
            this.amount = amount.ToString("F", CultureInfo.InvariantCulture);
            this.date = date.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            this.backUrl = returnUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("merchant", this.Merchant));
            builder.Append(this.CreateField("BillNo", this.billNo));
            builder.Append(this.CreateField("Amount", this.amount));
            builder.Append(this.CreateField("Date", this.date));
            builder.Append(this.CreateField("Remark", "Allbuy"));
            builder.Append(this.CreateField("BackUrl", this.backUrl));
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "http://www.allbuy.cn/newpayment/payment.asp"));
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

        [ConfigElement("商户密钥", Nullable=false)]
        public string Key { get; set; }

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("商户号", Nullable=false)]
        public string Merchant { get; set; }

        protected override bool NeedProtect
        {
            get
            {
                return true;
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

