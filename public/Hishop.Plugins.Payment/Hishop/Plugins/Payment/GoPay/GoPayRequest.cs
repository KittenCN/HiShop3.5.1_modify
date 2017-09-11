namespace Hishop.Plugins.Payment.GoPay
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Plugin("国付宝")]
    public class GoPayRequest : PaymentRequest
    {
        private readonly string backgroundMerUrl;
        private readonly string bankCode;
        private string body;
        private string buyerEmail;
        private const string charset = "2";
        private const string currencyTyp = "156";
        private const string currencyType = "156";
        private readonly string feeAmt;
        private readonly string frontMerUrl;
        private const string GatewayUrl = "https://gateway.gopay.com.cn/Trans/WebClientAction.do";
        private string gopayServerTime;
        private const string language = "1";
        private readonly string merOrderNum;
        private readonly string msgExt;
        private string sign;
        private const string signType = "1";
        private string subject;
        private readonly string tranAmt;
        private const string tranCode = "8888";
        private readonly string tranDateTime;
        private readonly string tranIP;
        private readonly string userType;
        private const string version = "2.1";

        public GoPayRequest()
        {
            this.merOrderNum = "";
            this.tranAmt = "";
            this.feeAmt = "0";
            this.frontMerUrl = "";
            this.backgroundMerUrl = "";
            this.tranDateTime = "";
            this.tranIP = Globals.getRealIp();
            this.msgExt = "";
            this.bankCode = "";
            this.userType = "";
            this.gopayServerTime = Globals.GetGopayServerTime();
            this.sign = "";
            this.subject = "";
            this.body = "";
            this.buyerEmail = "";
        }

        public GoPayRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.merOrderNum = "";
            this.tranAmt = "";
            this.feeAmt = "0";
            this.frontMerUrl = "";
            this.backgroundMerUrl = "";
            this.tranDateTime = "";
            this.tranIP = Globals.getRealIp();
            this.msgExt = "";
            this.bankCode = "";
            this.userType = "";
            this.gopayServerTime = Globals.GetGopayServerTime();
            this.sign = "";
            this.subject = "";
            this.body = "";
            this.buyerEmail = "";
            this.buyerEmail = buyerEmail;
            this.body = body;
            this.subject = subject;
            this.frontMerUrl = returnUrl;
            this.backgroundMerUrl = notifyUrl;
            this.merOrderNum = orderId;
            this.tranAmt = amount.ToString("F", CultureInfo.InvariantCulture);
            this.tranDateTime = date.ToString("yyyyMMddHHmmss");
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("version", "2.1"));
            builder.Append(this.CreateField("charset", "2"));
            builder.Append(this.CreateField("language", "1"));
            builder.Append(this.CreateField("signType", "1"));
            builder.Append(this.CreateField("tranCode", "8888"));
            builder.Append(this.CreateField("merchantID", this.merchantID));
            builder.Append(this.CreateField("merOrderNum", this.merOrderNum));
            builder.Append(this.CreateField("tranAmt", this.tranAmt));
            builder.Append(this.CreateField("feeAmt", this.feeAmt));
            builder.Append(this.CreateField("currencyType", "156"));
            builder.Append(this.CreateField("frontMerUrl", this.frontMerUrl));
            builder.Append(this.CreateField("backgroundMerUrl", this.backgroundMerUrl));
            builder.Append(this.CreateField("tranDateTime", this.tranDateTime));
            builder.Append(this.CreateField("virCardNoIn", this.virCardNoIn));
            builder.Append(this.CreateField("tranIP", this.tranIP));
            builder.Append(this.CreateField("isRepeatSubmit", "1"));
            builder.Append(this.CreateField("goodsName", this.subject));
            builder.Append(this.CreateField("goodsDetail", this.body));
            builder.Append(this.CreateField("buyerName", ""));
            builder.Append(this.CreateField("buyerContact", this.buyerEmail));
            builder.Append(this.CreateField("merRemark1", ""));
            builder.Append(this.CreateField("merRemark2", ""));
            builder.Append(this.CreateField("gopayServerTime", this.gopayServerTime));
            builder.Append(this.CreateField("bankCode", this.bankCode));
            builder.Append(this.CreateField("userType", this.userType));
            builder.Append(this.CreateField("VerficationCode", this.VerficationCode));
            string str = "version=[2.1]tranCode=[8888]merchantID=[" + this.merchantID + "]merOrderNum=[" + this.merOrderNum + "]tranAmt=[" + this.tranAmt + "]feeAmt=[" + this.feeAmt + "]tranDateTime=[" + this.tranDateTime + "]frontMerUrl=[" + this.frontMerUrl + "]backgroundMerUrl=[" + this.backgroundMerUrl + "]orderId=[]gopayOutOrderId=[]tranIP=[" + this.tranIP + "]respCode=[]gopayServerTime=[" + this.gopayServerTime + "]VerficationCode=[" + this.VerficationCode + "]";
            this.sign = Globals.GetMD5(str);
            builder.Append(this.CreateField("signValue", this.sign));
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "https://gateway.gopay.com.cn/Trans/WebClientAction.do"));
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
        public string merchantID { get; set; }

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

        [ConfigElement("商户密钥", Nullable=false)]
        public string VerficationCode { get; set; }

        [ConfigElement("国付宝账号", Nullable=false)]
        public string virCardNoIn { get; set; }
    }
}

