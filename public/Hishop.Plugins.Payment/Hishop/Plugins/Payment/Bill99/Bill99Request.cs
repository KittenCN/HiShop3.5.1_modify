namespace Hishop.Plugins.Payment.Bill99
{
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;

    [Plugin("快钱", Sequence=6)]
    public class Bill99Request : PaymentRequest
    {
        private readonly string bgUrl;
        private const string Ext1 = "99Bill";
        private const string Gateway = "https://www.99bill.com/gateway/recvMerchantInfoAction.htm";
        private const string InputCharset = "1";
        private const string Language = "1";
        private readonly string orderAmount;
        private readonly string orderId;
        private readonly string orderTime;
        private const string PayType = "00";
        private readonly string productName;
        private const string ProductNum = "1";
        private const string RedoFlag = "0";
        private const string SignType = "4";
        private const string Version = "v2.0";

        public Bill99Request()
        {
            this.productName = "";
        }

        public Bill99Request(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.productName = "";
            this.productName = orderId;
            this.bgUrl = notifyUrl;
            this.orderAmount = Convert.ToInt32((decimal) (amount * 100M)).ToString(CultureInfo.InvariantCulture);
            this.orderId = orderId;
            this.orderTime = date.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture);
        }

        private string appendParam(string returnStr, string paramId, string paramValue)
        {
            if (returnStr != "")
            {
                if (paramValue != "")
                {
                    string str2 = returnStr;
                    returnStr = str2 + "&" + paramId + "=" + paramValue;
                }
                return returnStr;
            }
            if (paramValue != "")
            {
                returnStr = paramId + "=" + paramValue;
            }
            return returnStr;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string returnStr = "";
            returnStr = this.appendParam(returnStr, "inputCharset", "1");
            returnStr = this.appendParam(returnStr, "bgUrl", this.bgUrl);
            returnStr = this.appendParam(returnStr, "version", "v2.0");
            returnStr = this.appendParam(returnStr, "language", "1");
            returnStr = this.appendParam(returnStr, "signType", "4");
            returnStr = this.appendParam(returnStr, "merchantAcctId", this.MerchantAcctId);
            returnStr = this.appendParam(returnStr, "orderId", this.orderId);
            returnStr = this.appendParam(returnStr, "orderAmount", this.orderAmount);
            returnStr = this.appendParam(returnStr, "orderTime", this.orderTime);
            returnStr = this.appendParam(returnStr, "productNum", "1");
            returnStr = this.appendParam(returnStr, "ext1", "99Bill");
            returnStr = this.appendParam(returnStr, "payType", "00");
            returnStr = this.appendParam(returnStr, "redoFlag", "0");
            if (!string.IsNullOrEmpty(this.Pid))
            {
                returnStr = this.appendParam(returnStr, "pid", this.Pid);
            }
            byte[] bytes = Encoding.UTF8.GetBytes(returnStr);
            X509Certificate2 certificate = new X509Certificate2(HttpContext.Current.Server.MapPath("~/plugins/payment/Cert/99bill-rsa.pfx"), this.Certpwd, X509KeyStorageFlags.MachineKeySet);
            RSACryptoServiceProvider privateKey = (RSACryptoServiceProvider) certificate.PrivateKey;
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(privateKey);
            formatter.SetHashAlgorithm("SHA1");
            byte[] rgbHash = new SHA1CryptoServiceProvider().ComputeHash(bytes);
            string strValue = Convert.ToBase64String(formatter.CreateSignature(rgbHash)).ToString();
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CreateField("inputCharset", "1"));
            builder.Append(this.CreateField("bgUrl", this.bgUrl));
            builder.Append(this.CreateField("version", "v2.0"));
            builder.Append(this.CreateField("language", "1"));
            builder.Append(this.CreateField("signType", "4"));
            builder.Append(this.CreateField("signMsg", strValue));
            builder.Append(this.CreateField("merchantAcctId", this.MerchantAcctId));
            builder.Append(this.CreateField("orderId", this.orderId));
            builder.Append(this.CreateField("orderAmount", this.orderAmount));
            builder.Append(this.CreateField("orderTime", this.orderTime));
            builder.Append(this.CreateField("productNum", "1"));
            builder.Append(this.CreateField("ext1", "99Bill"));
            builder.Append(this.CreateField("payType", "00"));
            builder.Append(this.CreateField("redoFlag", "0"));
            if (!string.IsNullOrEmpty(this.Pid))
            {
                builder.Append(this.CreateField("pid", this.Pid));
            }
            this.SubmitPaymentForm(this.CreateForm(builder.ToString(), "https://www.99bill.com/gateway/recvMerchantInfoAction.htm"));
        }

        [ConfigElement("证书密码")]
        public string Certpwd { get; set; }

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
        public string MerchantAcctId { get; set; }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        [ConfigElement("合作伙伴账号")]
        public string Pid { get; set; }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

