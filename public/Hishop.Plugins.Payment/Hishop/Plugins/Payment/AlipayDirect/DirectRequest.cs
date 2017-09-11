namespace Hishop.Plugins.Payment.AlipayDirect
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    [Plugin("支付宝即时到帐交易", Sequence=2)]
    public class DirectRequest : PaymentRequest
    {
        private const string Agent = "C4335302345904805116";
        private readonly string body;
        private const string extend_param = "isv^yf31";
        private const string Gateway = "https://mapi.alipay.com/gateway.do?";
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

        public DirectRequest()
        {
        }

        public DirectRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.body = body;
            this.outTradeNo = orderId;
            this.subject = subject;
            this.totalFee = amount.ToString("F", CultureInfo.InvariantCulture);
            this.returnUrl = returnUrl;
            this.notifyUrl = notifyUrl;
            this.showUrl = showUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            this.RedirectToGateway(Globals.CreatDirectUrl("https://mapi.alipay.com/gateway.do?", "create_direct_pay_by_user", this.Partner, "MD5", this.outTradeNo, this.subject, this.body, "1", this.totalFee, this.showUrl, this.SellerEmail, this.Key, this.returnUrl, "utf-8", this.notifyUrl, "C4335302345904805116", "isv^yf31"));
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

