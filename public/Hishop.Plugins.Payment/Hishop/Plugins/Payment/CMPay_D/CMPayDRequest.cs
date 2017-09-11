namespace Hishop.Plugins.Payment.CMPay_D
{
    using Com.HisunCmpay;
    using Hishop.Plugins;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web;

    [Plugin("手机支付即时到帐(双向确认)")]
    public class CMPayDRequest : PaymentRequest
    {
        private string amount;
        private string bankAbbr;
        private string callbackUrl;
        private string characterSet;
        private string couponsFlag;
        private string currency;
        private const string Gateway = "https://ipos.10086.cn/ips/cmpayService";
        private string hmac;
        private string ipAddress;
        private string merAcDate;
        private string merchantAbbr;
        private string merchantCert;
        private string notifyUrl;
        private string orderDate;
        private string orderId;
        private string period;
        private string periodUnit;
        private string productDesc;
        private string productId;
        private string productName;
        private string productNum;
        private string requestId;
        private string reserved1;
        private string reserved2;
        private string showUrl;
        private string signType;
        private string type;
        private string userToken;
        private string version;

        public CMPayDRequest()
        {
            this.ipAddress = IPosMUtil.getIpAddress();
            this.characterSet = "02";
            this.callbackUrl = string.Empty;
            this.notifyUrl = string.Empty;
            this.requestId = IPosMUtil.getTicks();
            this.signType = "MD5";
            this.type = "DirectPayConfirm";
            this.version = "2.0.0";
            this.merchantCert = string.Empty;
            this.hmac = string.Empty;
            this.amount = "0";
            this.bankAbbr = string.Empty;
            this.currency = "00";
            this.orderDate = string.Empty;
            this.merAcDate = string.Empty;
            this.orderId = string.Empty;
            this.period = "3";
            this.periodUnit = "03";
            this.merchantAbbr = string.Empty;
            this.productDesc = string.Empty;
            this.productId = string.Empty;
            this.productName = string.Empty;
            this.productNum = string.Empty;
            this.reserved1 = string.Empty;
            this.reserved2 = string.Empty;
            this.userToken = string.Empty;
            this.showUrl = string.Empty;
            this.couponsFlag = "00";
        }

        public CMPayDRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.ipAddress = IPosMUtil.getIpAddress();
            this.characterSet = "02";
            this.callbackUrl = string.Empty;
            this.notifyUrl = string.Empty;
            this.requestId = IPosMUtil.getTicks();
            this.signType = "MD5";
            this.type = "DirectPayConfirm";
            this.version = "2.0.0";
            this.merchantCert = string.Empty;
            this.hmac = string.Empty;
            this.amount = "0";
            this.bankAbbr = string.Empty;
            this.currency = "00";
            this.orderDate = string.Empty;
            this.merAcDate = string.Empty;
            this.orderId = string.Empty;
            this.period = "3";
            this.periodUnit = "03";
            this.merchantAbbr = string.Empty;
            this.productDesc = string.Empty;
            this.productId = string.Empty;
            this.productName = string.Empty;
            this.productNum = string.Empty;
            this.reserved1 = string.Empty;
            this.reserved2 = string.Empty;
            this.userToken = string.Empty;
            this.showUrl = string.Empty;
            this.couponsFlag = "00";
            this.productName = orderId;
            this.callbackUrl = returnUrl;
            this.notifyUrl = notifyUrl;
            this.amount = Convert.ToInt32((decimal) (amount * 100M)).ToString(CultureInfo.InvariantCulture);
            this.orderId = orderId;
            this.orderDate = this.merAcDate = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        }

        private static string AppendParam(string returnStr, string paramId, string paramValue)
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
            string str3 = SignUtil.HmacSign(SignUtil.HmacSign(this.characterSet + this.callbackUrl + this.notifyUrl + this.ipAddress + this.MerchantAcctId + this.requestId + this.signType + this.type + this.version + this.amount + this.bankAbbr + this.currency + this.orderDate + this.orderId + this.merAcDate + this.period + this.periodUnit + this.merchantAbbr + this.productDesc + this.productId + this.productName + this.productNum + this.reserved1 + this.reserved2 + this.userToken + this.showUrl + this.couponsFlag), this.Key);
            string data = "characterSet=" + this.characterSet + "&callbackUrl=" + this.callbackUrl + "&notifyUrl=" + this.notifyUrl + "&ipAddress=" + this.ipAddress + "&merchantId=" + this.MerchantAcctId + "&requestId=" + this.requestId + "&signType=" + this.signType + "&type=" + this.type + "&version=" + this.version + "&amount=" + this.amount + "&bankAbbr=" + this.bankAbbr + "&currency=" + this.currency + "&orderDate=" + this.orderDate + "&orderId=" + this.orderId + "&merAcDate=" + this.merAcDate + "&period=" + this.period + "&periodUnit=" + this.periodUnit + "&merchantAbbr=" + this.merchantAbbr + "&productDesc=" + this.productDesc + "&productId=" + this.productId + "&productName=" + this.productName + "&productNum=" + this.productNum + "&reserved1=" + this.reserved1 + "&reserved2=" + this.reserved2 + "&userToken=" + this.userToken + "&showUrl=" + this.showUrl + "&couponsFlag=" + this.couponsFlag + "&hmac=" + str3;
            Hashtable hashtable = IPosMUtil.parseStringToMap(IPosMUtil.httpRequest("https://ipos.10086.cn/ips/cmpayService", data));
            string hmac = (string) hashtable["hmac"];
            string str7 = (string) hashtable["returnCode"];
            string str8 = (string) hashtable["message"];
            if ("000000".Equals(str7) && SignUtil.verifySign(((string) hashtable["merchantId"]) + ((string) hashtable["requestId"]) + ((string) hashtable["signType"]) + ((string) hashtable["type"]) + ((string) hashtable["version"]) + str7 + str8 + ((string) hashtable["payUrl"]), this.Key, hmac))
            {
                string payUrl = (string) hashtable["payUrl"];
                HttpContext.Current.Response.Redirect(IPosMUtil.getRedirectUrl(payUrl));
            }
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

        [ConfigElement("商户号", Nullable=false)]
        public string MerchantAcctId { get; set; }

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

