namespace Hishop.Plugins.Payment.Xpay
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Plugin("易付通")]
    public class XpayRequest : PaymentRequest
    {
        private const string ActionCode = "sell";
        private readonly string bid;
        private const string Card = "bank";
        private const string GatewayUrl = "http://pay.xpay.cn/pay.aspx";
        private readonly string pdt;
        private readonly string prc;
        private const string Remark1 = "xpay";
        private const string Scard = "bank,Unicom,xpay,ebilling,ibank";
        private readonly string url;
        private const string Ver = "2.0";

        public XpayRequest()
        {
            this.bid = "";
            this.prc = "";
            this.url = "";
            this.pdt = "";
        }

        public XpayRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.bid = "";
            this.prc = "";
            this.url = "";
            this.pdt = "";
            this.bid = orderId;
            this.prc = amount.ToString("F", CultureInfo.InvariantCulture);
            this.pdt = subject;
            this.url = returnUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string strValue = Globals.GetXpayMD5(this.Key + ":" + this.prc + "," + this.bid + "," + this.Tid + ",bank,bank,Unicom,xpay,ebilling,ibank,sell,,2.0");
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("tid", this.Tid));
            builder.Append(this.CreateField("bid", this.bid));
            builder.Append(this.CreateField("prc", this.prc));
            builder.Append(this.CreateField("card", "bank"));
            builder.Append(this.CreateField("scard", "bank,Unicom,xpay,ebilling,ibank"));
            builder.Append(this.CreateField("actioncode", "sell"));
            builder.Append(this.CreateField("actionparameter", ""));
            builder.Append(this.CreateField("ver", "2.0"));
            builder.Append(this.CreateField("url", this.url));
            builder.Append(this.CreateField("pdt", this.pdt));
            builder.Append(this.CreateField("remark1", "xpay"));
            builder.Append(this.CreateField("md", strValue));
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "http://pay.xpay.cn/pay.aspx"));
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

        [ConfigElement("商户号", Nullable=false)]
        public string Tid { get; set; }
    }
}

