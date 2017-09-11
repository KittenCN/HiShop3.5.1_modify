namespace Hishop.Plugins.Payment.Cncard
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.Security;

    [Plugin("云网支付")]
    public class CncardRequest : PaymentRequest
    {
        private const string c_language = "0";
        private const string c_memo1 = "Cncard";
        private const string c_moneytype = "0";
        private readonly string c_order;
        private readonly string c_orderamount;
        private const string c_retflag = "1";
        private readonly string c_returl;
        private readonly string c_ymd;
        private const string GatewayUrl = "https://www.cncard.net/purchase/getorder.asp";
        private const string notifytype = "0";

        public CncardRequest()
        {
            this.c_order = "";
            this.c_orderamount = "";
            this.c_ymd = "";
            this.c_returl = "";
        }

        public CncardRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.c_order = "";
            this.c_orderamount = "";
            this.c_ymd = "";
            this.c_returl = "";
            this.c_order = orderId;
            this.c_orderamount = amount.ToString("F", CultureInfo.InvariantCulture);
            this.c_ymd = date.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            this.c_returl = returnUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string strValue = FormsAuthentication.HashPasswordForStoringInConfigFile(this.Cmid + this.c_order + this.c_orderamount + this.c_ymd + "01" + this.c_returl + "Cncard00" + this.Cpass, "MD5").ToLower();
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("c_mid", this.Cmid));
            builder.Append(this.CreateField("c_order", this.c_order));
            builder.Append(this.CreateField("c_orderamount", this.c_orderamount));
            builder.Append(this.CreateField("c_ymd", this.c_ymd));
            builder.Append(this.CreateField("c_moneytype", "0"));
            builder.Append(this.CreateField("c_retflag", "1"));
            builder.Append(this.CreateField("c_returl", this.c_returl));
            builder.Append(this.CreateField("c_paygate", ""));
            builder.Append(this.CreateField("c_memo1", "Cncard"));
            builder.Append(this.CreateField("c_memo2", ""));
            builder.Append(this.CreateField("c_language", "0"));
            builder.Append(this.CreateField("notifytype", "0"));
            builder.Append(this.CreateField("c_signstr", strValue));
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "https://www.cncard.net/purchase/getorder.asp"));
        }

        [ConfigElement("商户号", Nullable=false)]
        public string Cmid { get; set; }

        [ConfigElement("支付密钥", Nullable=false)]
        public string Cpass { get; set; }

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

