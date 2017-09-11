namespace Hishop.Plugins.Payment.Chinabank
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.Security;

    [Plugin("网银在线")]
    public class ChinabankRequest : PaymentRequest
    {
        private const string Gateway = "https://pay3.chinabank.com.cn/PayGate";
        private const string Remark1 = "Chinabank";
        private readonly string remark2;
        private readonly string v_amount;
        private const string v_moneytype = "CNY";
        private readonly string v_oid;
        private readonly string v_url;

        public ChinabankRequest()
        {
            this.v_oid = "";
            this.v_amount = "";
            this.v_url = "";
            this.remark2 = "";
        }

        public ChinabankRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.v_oid = "";
            this.v_amount = "";
            this.v_url = "";
            this.remark2 = "";
            this.v_oid = orderId;
            this.v_amount = amount.ToString("F", CultureInfo.InvariantCulture);
            this.v_url = returnUrl;
            this.remark2 = "[url:=" + notifyUrl + "]";
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string strValue = FormsAuthentication.HashPasswordForStoringInConfigFile(this.v_amount + "CNY" + this.v_oid + this.Vmid + this.v_url + this.Key, "MD5").ToUpper(CultureInfo.InvariantCulture);
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("v_mid", this.Vmid));
            builder.Append(this.CreateField("v_oid", this.v_oid));
            builder.Append(this.CreateField("v_amount", this.v_amount));
            builder.Append(this.CreateField("v_moneytype", "CNY"));
            builder.Append(this.CreateField("v_url", this.v_url));
            builder.Append(this.CreateField("remark1", "Chinabank"));
            builder.Append(this.CreateField("remark2", this.remark2));
            builder.Append(this.CreateField("v_md5info", strValue));
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "https://pay3.chinabank.com.cn/PayGate"));
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
        public string Vmid { get; set; }
    }
}

