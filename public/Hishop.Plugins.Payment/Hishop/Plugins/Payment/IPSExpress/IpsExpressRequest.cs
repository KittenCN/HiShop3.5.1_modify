namespace Hishop.Plugins.Payment.IPSExpress
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.Security;

    [Plugin("环迅易捷支付")]
    public class IpsExpressRequest : PaymentRequest
    {
        private readonly string Amount;
        private readonly string BackUrl;
        private readonly string BillNo;
        private const string PostUrl = "http://express.ips.com.cn/pay/payment.asp";
        private const string Remark = "IPSExpress";

        public IpsExpressRequest()
        {
        }

        public IpsExpressRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.BillNo = orderId;
            this.Amount = amount.ToString("F", CultureInfo.InvariantCulture);
            this.BackUrl = returnUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string strValue = FormsAuthentication.HashPasswordForStoringInConfigFile(this.Merchant + this.BillNo + this.Amount + "IPSExpress" + this.MerPassword, "MD5").ToLower(CultureInfo.InvariantCulture);
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("Merchant", this.Merchant));
            builder.Append(this.CreateField("BillNo", this.BillNo));
            builder.Append(this.CreateField("Amount", this.Amount));
            builder.Append(this.CreateField("Remark", "IPSExpress"));
            builder.Append(this.CreateField("BackUrl", this.BackUrl));
            builder.Append(this.CreateField("Sign", strValue));
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00018\" checked>中国工商银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00021\">招商银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00003\">中国建设银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00017\">中国农业银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00013\">民生银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00030\">光大银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00016\">兴业银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00111\">中国银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00211\">交通银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00311\">交通银行上海" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00411\">广东发展银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00023\">深圳发展银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00032\">浦东发展银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00511\">中信实业银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00611\">广州商业银行" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00711\">邮政储蓄" + Environment.NewLine);
            builder.Append("<input type=\"radio\" name=\"PayBank\" value=\"00811\">华夏银行" + Environment.NewLine);
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "http://express.ips.com.cn/pay/payment.asp"));
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

        [ConfigElement("商户号", Nullable=false)]
        public string Merchant { get; set; }

        [ConfigElement("商户密钥", Nullable=false)]
        public string MerPassword { get; set; }

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

