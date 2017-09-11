namespace Hishop.Plugins.Payment.WS_WapPay
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    [Plugin("支付宝手机网站支付")]
    public class WsWapPayRequest : PaymentRequest
    {
        private string Call_back_url;
        private const string Format = "xml";
        private const string Input_charset_UTF8 = "utf-8";
        private string Merchant_url;
        private string Notify_url;
        private string Out_trade_no;
        private string Out_user;
        private string Req_id;
        private const string Req_url = "http://wappaygw.alipay.com/service/rest.htm";
        private const string Sec_id = "MD5";
        private const string Service_Auth = "alipay.wap.auth.authAndExecute";
        private const string Service_Create = "alipay.wap.trade.create.direct";
        private string Subject;
        private string Total_fee;
        private const string V = "2.0";

        public WsWapPayRequest()
        {
            this.Req_id = DateTime.Now.ToString();
            this.Out_trade_no = "";
            this.Subject = "";
            this.Total_fee = "";
            this.Out_user = "";
            this.Call_back_url = "";
            this.Notify_url = "";
            this.Merchant_url = "";
        }

        public WsWapPayRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.Req_id = DateTime.Now.ToString();
            this.Out_trade_no = "";
            this.Subject = "";
            this.Total_fee = "";
            this.Out_user = "";
            this.Call_back_url = "";
            this.Notify_url = "";
            this.Merchant_url = "";
            this.Out_trade_no = orderId;
            this.Total_fee = amount.ToString("F", CultureInfo.InvariantCulture);
            this.Subject = subject;
            this.Merchant_url = showUrl;
            this.Call_back_url = returnUrl;
            this.Notify_url = notifyUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            Service service = new Service();
            string token = service.alipay_wap_trade_create_direct("http://wappaygw.alipay.com/service/rest.htm", this.Subject, this.Out_trade_no, this.Total_fee, this.Seller_account_name, this.Notify_url, this.Out_user, this.Merchant_url, this.Call_back_url, "alipay.wap.trade.create.direct", "MD5", this.Partner, this.Req_id, "xml", "2.0", "utf-8", "http://wappaygw.alipay.com/service/rest.htm", this.Key, "MD5");
            string url = service.alipay_Wap_Auth_AuthAndExecute("http://wappaygw.alipay.com/service/rest.htm", "MD5", this.Partner, this.Call_back_url, "xml", "2.0", "alipay.wap.auth.authAndExecute", token, "utf-8", "http://wappaygw.alipay.com/service/rest.htm", this.Key, "MD5");
            this.RedirectToGateway(url);
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

        [ConfigElement("商户私钥", Nullable=false)]
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

        [ConfigElement("商户号", Nullable=false)]
        public string Partner { get; set; }

        [ConfigElement("收款账号", Nullable=false)]
        public string Seller_account_name { get; set; }

        public override string ShortDescription
        {
            get
            {
                return "mobile";
            }
        }
    }
}

