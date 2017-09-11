namespace Hishop.Plugins.Payment.Alipay_Bank
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web;

    [Plugin("支付宝纯网关接口", Sequence=3)]
    public class BankRequest : PaymentRequest
    {
        private const string Agent = "C4335302345904805116";
        private readonly string body;
        private readonly string defaultBank;
        private const string extend_param = "";
        private const string Gateway = "https://mapi.alipay.com/gateway.do";
        private const string InputCharset = "utf-8";
        private readonly string notifyUrl;
        private readonly string outTradeNo;
        private const string PaymentType = "1";
        private readonly string returnUrl;
        private const string Service = "create_direct_pay_by_user";
        private readonly string showUrl;
        private const string SignType = "MD5";
        private readonly string subject;
        private readonly string totalFee;

        public BankRequest()
        {
        }

        public BankRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.body = body;
            this.outTradeNo = orderId;
            this.subject = subject;
            this.totalFee = amount.ToString("F", CultureInfo.InvariantCulture);
            this.returnUrl = returnUrl;
            this.notifyUrl = notifyUrl;
            this.showUrl = showUrl;
            this.defaultBank = attach;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string str = "1";
            string notifyUrl = this.notifyUrl;
            string returnUrl = this.returnUrl;
            string sellerEmail = this.SellerEmail;
            string outTradeNo = this.outTradeNo;
            string subject = this.subject;
            string totalFee = this.totalFee;
            string body = this.body;
            string str9 = "bankPay";
            string defaultBank = this.defaultBank;
            string showUrl = this.showUrl;
            string str12 = "";
            string str13 = "";
            Config.Partner = this.Partner;
            Config.Key = this.Key;
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("payment_type", str);
            sParaTemp.Add("notify_url", notifyUrl);
            sParaTemp.Add("return_url", returnUrl);
            sParaTemp.Add("seller_email", sellerEmail);
            sParaTemp.Add("out_trade_no", outTradeNo);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", totalFee);
            sParaTemp.Add("body", body);
            sParaTemp.Add("paymethod", str9);
            sParaTemp.Add("defaultbank", defaultBank);
            sParaTemp.Add("show_url", showUrl);
            sParaTemp.Add("anti_phishing_key", str12);
            sParaTemp.Add("exter_invoke_ip", str13);
            string s = Submit.BuildRequest(sParaTemp, "get", "确认");
            HttpContext.Current.Response.Write(s);
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

        [ConfigElement("安全校验码(Key)", Nullable=false)]
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

        [ConfigElement("合作者身份(PID)", Nullable=false)]
        public string Partner { get; set; }

        [ConfigElement("收款支付宝账号", Nullable=false)]
        public string SellerEmail { get; set; }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

