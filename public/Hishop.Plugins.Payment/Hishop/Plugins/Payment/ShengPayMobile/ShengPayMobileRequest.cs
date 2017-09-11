namespace Hishop.Plugins.Payment.ShengPayMobile
{
    using Hishop.Plugins;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;
    using System.Web.Security;

    [Plugin("盛付通移动支付")]
    public class ShengPayMobileRequest : PaymentRequest
    {
        private string BuyerContact;
        private string BuyerId;
        private string BuyerIP;
        private string BuyerName;
        private string Charset;
        private const string Currency = "CNY";
        private string Ext1;
        private string Ext2;
        private const string GatewayUrl = "http://api.shengpay.com/html5-gateway/pay.htm?page=mobile";
        private string InstCode;
        private string NotifyUrl;
        private string OrderAmount;
        private string OrderNo;
        private string OrderTime;
        private string PageUrl;
        private string PayerAuthTicket;
        private string PayerMobileNo;
        private string PayType;
        private string ProductDesc;
        private string ProductId;
        private string ProductName;
        private string ProductNum;
        private string ProductUrl;
        private string SellerId;
        private string SendTime;
        private const string ServiceCode = "B2CPayment";
        private string SignType;
        private string TraceNo;
        private string UnitPrice;
        private const string Version = "V4.1.1.1.1";

        public ShengPayMobileRequest()
        {
            this.Charset = "UTF-8";
            this.SignType = "MD5";
        }

        public ShengPayMobileRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime orderTime, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.Charset = "UTF-8";
            this.SignType = "MD5";
            this.OrderNo = orderId;
            this.OrderAmount = amount.ToString("F2");
            this.PageUrl = returnUrl;
            this.NotifyUrl = notifyUrl;
            this.BuyerIP = GetUserIP();
            this.TraceNo = Guid.NewGuid().ToString("N");
            this.ProductUrl = showUrl;
            this.ProductName = body;
            this.ProductDesc = body;
            this.OrderTime = orderTime.ToString("yyyyMMddHHmmss");
        }

        public static string GetUserIP()
        {
            string userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            switch (userHostAddress)
            {
                case null:
                case "":
                    userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    break;
            }
            if ((userHostAddress == null) || (userHostAddress == string.Empty))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            if (userHostAddress.Length >= 20)
            {
                userHostAddress = "";
            }
            return userHostAddress;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
            throw new NotImplementedException();
        }

        public override void SendRequest()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(string.IsNullOrEmpty("B2CPayment") ? "" : "B2CPayment|", new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty("V4.1.1.1.1") ? "" : "V4.1.1.1.1|", new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.Charset) ? "" : (this.Charset + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.TraceNo) ? "" : (this.TraceNo + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.SenderId) ? "" : (this.SenderId + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.SendTime) ? "" : (this.SendTime + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.OrderNo) ? "" : (this.OrderNo + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.OrderAmount) ? "" : (this.OrderAmount + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.OrderTime) ? "" : (this.OrderTime + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty("CNY") ? "" : "CNY|", new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.PageUrl) ? "" : (this.PageUrl + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.NotifyUrl) ? "" : (this.NotifyUrl + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.ProductId) ? "" : (this.ProductId + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.ProductName) ? "" : (this.ProductName + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.ProductNum) ? "" : (this.ProductNum + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.UnitPrice) ? "" : (this.UnitPrice + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.ProductDesc) ? "" : (this.ProductDesc + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.ProductUrl) ? "" : (this.ProductUrl + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.SellerId) ? "" : (this.SellerId + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.BuyerName) ? "" : (this.BuyerName + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.BuyerId) ? "" : (this.BuyerId + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.BuyerContact) ? "" : (this.BuyerContact + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.BuyerIP) ? "" : (this.BuyerIP + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.PayerMobileNo) ? "" : (this.PayerMobileNo + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.PayerAuthTicket) ? "" : (this.PayerAuthTicket + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.Ext1) ? "" : (this.Ext1 + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.Ext2) ? "" : (this.Ext2 + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.SignType) ? "" : (this.SignType + "|"), new object[0]);
            string strValue = FormsAuthentication.HashPasswordForStoringInConfigFile(builder.ToString() + this.SellerKey, this.SignType);
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(this.CreateField("serviceCode", "B2CPayment"));
            builder2.Append(this.CreateField("version", "V4.1.1.1.1"));
            builder2.Append(this.CreateField("charset", this.Charset));
            builder2.Append(this.CreateField("TraceNo", this.TraceNo));
            builder2.Append(this.CreateField("senderId", this.SenderId));
            builder2.Append(this.CreateField("sendTime", this.SendTime));
            builder2.Append(this.CreateField("orderNo", this.OrderNo));
            builder2.Append(this.CreateField("orderAmount", this.OrderAmount));
            builder2.Append(this.CreateField("orderTime", this.OrderTime));
            builder2.Append(this.CreateField("currency", "CNY"));
            builder2.Append(this.CreateField("payType", this.PayType));
            builder2.Append(this.CreateField("instCode", this.InstCode));
            builder2.Append(this.CreateField("pageUrl", this.PageUrl));
            builder2.Append(this.CreateField("notifyUrl", this.NotifyUrl));
            builder2.Append(this.CreateField("productId", this.ProductId));
            builder2.Append(this.CreateField("productName", this.ProductName));
            builder2.Append(this.CreateField("productNum", this.ProductNum));
            builder2.Append(this.CreateField("productDesc", this.ProductDesc));
            builder2.Append(this.CreateField("productUrl", this.ProductUrl));
            builder2.Append(this.CreateField("sellerId", this.SellerId));
            builder2.Append(this.CreateField("buyerName", this.BuyerName));
            builder2.Append(this.CreateField("buyerId", this.BuyerId));
            builder2.Append(this.CreateField("buyerContact", this.BuyerContact));
            builder2.Append(this.CreateField("buyerIp", this.BuyerIP));
            builder2.Append(this.CreateField("payerMobileNo", this.PayerMobileNo));
            builder2.Append(this.CreateField("payerAuthTicket", this.PayerAuthTicket));
            builder2.Append(this.CreateField("ext1", this.Ext1));
            builder2.Append(this.CreateField("ext2", this.Ext2));
            builder2.Append(this.CreateField("signType", this.SignType));
            builder2.Append(this.CreateField("SignMsg", strValue));
            this.SubmitPaymentForm(this.CreateForm(builder2.ToString(), "http://api.shengpay.com/html5-gateway/pay.htm?page=mobile"));
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
                throw new NotImplementedException();
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

        [ConfigElement("商户密钥", Nullable=false)]
        public string SellerKey { get; set; }

        [ConfigElement("发送方标识", Nullable=false)]
        public string SenderId { get; set; }

        public override string ShortDescription
        {
            get
            {
                return "mobile";
            }
        }
    }
}

