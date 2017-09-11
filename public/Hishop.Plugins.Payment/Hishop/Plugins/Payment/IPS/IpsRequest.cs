namespace Hishop.Plugins.Payment.IPS
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Plugin("环迅(IPS v3.0)")]
    public class IpsRequest : PaymentRequest
    {
        private readonly string Amount;
        private const string Attach = "IPS";
        private readonly string Billno;
        private const string Currency_Type = "RMB";
        private readonly string Date;
        private const string Gateway_Type = "01";
        private readonly string Merchanturl;
        private const string OrderEncodeType = "5";
        private const string PostUrl = "https://pay.ips.com.cn/ipayment.aspx";
        private const string RetEncodeType = "17";
        private const string Rettype = "1";
        private readonly string ServerUrl;

        public IpsRequest()
        {
            this.Billno = "";
            this.ServerUrl = "";
            this.Amount = "";
            this.Date = "";
            this.Merchanturl = "";
        }

        public IpsRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.Billno = "";
            this.ServerUrl = "";
            this.Amount = "";
            this.Date = "";
            this.Merchanturl = "";
            this.Merchanturl = returnUrl;
            this.ServerUrl = notifyUrl;
            this.Billno = orderId;
            this.Amount = amount.ToString("F2", CultureInfo.InvariantCulture);
            this.Date = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("billno{0}currencytype{1}amount{2}date{3}", new object[] { this.Billno, "RMB", this.Amount, this.Date });
            builder.AppendFormat("orderencodetype{0}{1}", "5", this.Cert);
            string strValue = Globals.GetMD5(builder.ToString()).ToLower(CultureInfo.InvariantCulture);
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(this.CreateField("mer_code", this.Mer_code));
            builder2.Append(this.CreateField("Billno", this.Billno));
            builder2.Append(this.CreateField("Amount", this.Amount));
            builder2.Append(this.CreateField("Date", this.Date));
            builder2.Append(this.CreateField("Currency_Type", "RMB"));
            builder2.Append(this.CreateField("Gateway_type", "01"));
            builder2.Append(this.CreateField("Merchanturl", this.Merchanturl));
            builder2.Append(this.CreateField("Attach", "IPS"));
            builder2.Append(this.CreateField("OrderEncodeType", "5"));
            builder2.Append(this.CreateField("RetEncodeType", "17"));
            builder2.Append(this.CreateField("RetType", "1"));
            builder2.Append(this.CreateField("ServerUrl", this.ServerUrl));
            builder2.Append(this.CreateField("SignMD5", strValue));
            this.SubmitPaymentForm(this.CreateForm(builder2.ToString(), "https://pay.ips.com.cn/ipayment.aspx"));
        }

        [ConfigElement("商户密钥", Nullable=false)]
        public string Cert { get; set; }

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

        [ConfigElement("商户号", Nullable=false)]
        public string Mer_code { get; set; }

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

