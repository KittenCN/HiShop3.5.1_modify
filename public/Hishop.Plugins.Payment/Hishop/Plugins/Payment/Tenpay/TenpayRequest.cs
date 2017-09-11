namespace Hishop.Plugins.Payment.Tenpay
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web;

    [Plugin("财付通即时交易")]
    public class TenpayRequest : PaymentRequest
    {
        private const string Attach = "Tenpay";
        private const string Cmdno = "1";
        private readonly string date;
        private readonly string desc;
        private const string FeeType = "1";
        private const string GatewayUrl = "https://www.tenpay.com/cgi-bin/v1.0/pay_gate.cgi";
        private readonly string return_url;
        private readonly string sp_billno;
        private readonly string total_fee;
        private string transaction_id;

        public TenpayRequest()
        {
            this.date = "";
            this.desc = "";
            this.transaction_id = "";
            this.sp_billno = "";
            this.total_fee = "";
            this.return_url = "";
        }

        public TenpayRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.date = "";
            this.desc = "";
            this.transaction_id = "";
            this.sp_billno = "";
            this.total_fee = "";
            this.return_url = "";
            this.date = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            this.desc = UrlEncode(subject);
            this.sp_billno = orderId;
            this.return_url = returnUrl;
            this.total_fee = Convert.ToInt32((decimal) (amount * 100M)).ToString(CultureInfo.InvariantCulture);
        }

        private static string getRealIp()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string str = getRealIp();
            this.transaction_id = this.BargainorId + this.date + UnixStamp();
            string str3 = Globals.GetMD5("cmdno=1&date=" + this.date + "&bargainor_id=" + this.BargainorId + "&transaction_id=" + this.transaction_id + "&sp_billno=" + this.sp_billno + "&total_fee=" + this.total_fee + "&fee_type=1&return_url=" + this.return_url + "&attach=Tenpay&spbill_create_ip=" + str + "&key=" + this.Key);
            string url = "https://www.tenpay.com/cgi-bin/v1.0/pay_gate.cgi?cmdno=1&date=" + this.date + "&bank_type=0&desc=" + this.desc + "&purchaser_id=&bargainor_id=" + this.BargainorId + "&transaction_id=" + this.transaction_id + "&sp_billno=" + this.sp_billno + "&total_fee=" + this.total_fee + "&fee_type=1&return_url=" + this.return_url + "&attach=Tenpay&spbill_create_ip=" + str + "&cs=utf-8&sign=" + str3;
            this.RedirectToGateway(url);
        }

        private static uint UnixStamp()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1)));
            return Convert.ToUInt32(span.TotalSeconds);
        }

        private static string UrlEncode(string instr)
        {
            if ((instr == null) || (instr.Trim() == ""))
            {
                return "";
            }
            return instr.Replace("%", "%25").Replace("=", "%3d").Replace("&", "%26").Replace("\"", "%22").Replace("?", "%3f").Replace("'", "%27").Replace(" ", "%20");
        }

        [ConfigElement("商户号", Nullable=false)]
        public string BargainorId { get; set; }

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
    }
}

