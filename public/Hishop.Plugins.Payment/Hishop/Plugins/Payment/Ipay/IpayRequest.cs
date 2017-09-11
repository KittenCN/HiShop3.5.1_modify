namespace Hishop.Plugins.Payment.Ipay
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.Security;

    [Plugin("中国在线支付网")]
    public class IpayRequest : PaymentRequest
    {
        private const string Gateway = "http://www.ipay.cn/4.0/bank.shtml";
        private readonly string v_amount;
        private readonly string v_email;
        private readonly string v_oid;
        private readonly string v_url;

        public IpayRequest()
        {
            this.v_oid = "";
            this.v_amount = "";
            this.v_email = "";
            this.v_url = "";
        }

        public IpayRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.v_oid = "";
            this.v_amount = "";
            this.v_email = "";
            this.v_url = "";
            this.v_oid = orderId;
            this.v_amount = amount.ToString("F", CultureInfo.InvariantCulture);
            this.v_email = buyerEmail;
            this.v_url = returnUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string strValue = FormsAuthentication.HashPasswordForStoringInConfigFile(this.Vmid + this.v_oid + this.v_amount + this.v_email + this.Vkey, "MD5").ToLower(CultureInfo.InvariantCulture);
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("v_mid", this.Vmid));
            builder.Append(this.CreateField("v_oid", this.v_oid));
            builder.Append(this.CreateField("v_amount", this.v_amount));
            builder.Append(this.CreateField("v_email", this.v_email));
            builder.Append(this.CreateField("v_mobile", ""));
            builder.Append(this.CreateField("v_md5", strValue));
            builder.Append(this.CreateField("v_url", this.v_url));
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "http://www.ipay.cn/4.0/bank.shtml"));
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

        [ConfigElement("私钥", Nullable=false)]
        public string Vkey { get; set; }

        [ConfigElement("商户号", Nullable=false)]
        public string Vmid { get; set; }
    }
}

