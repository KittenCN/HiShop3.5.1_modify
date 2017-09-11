namespace Hishop.Plugins.Payment.ShengPay
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using LitJson;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;
    using System.Web.Security;

    [Plugin("盛付通即时交易")]
    public class ShengPayRequest : PaymentRequest
    {
        private readonly string _amount;
        private readonly string _backUrl;
        private readonly string _bankCode;
        private readonly string _merchantUserId;
        private readonly string _notifyUrl;
        private readonly string _orderNo;
        private readonly string _orderTime;
        private readonly string _payChannel;
        private readonly string _payType;
        private readonly string _postBackUrl;
        private readonly string _productDesc;
        private readonly string _productNo;
        private readonly string _productUrl;
        private readonly string _remark1;
        private readonly string _remark2;
        private string BuyerContact;
        private string BuyerIP;
        private const string Charset = "UTF-8";
        private const string CurrencyType = "CNY";
        private const string DefaultChannel = "";
        private const string GatewayUrl = "https://mas.shengpay.com/web-acquire-channel/cashier.htm";
        private const string InstCode = "";
        private readonly string Name;
        private const string NotifyUrlType = "http";
        private string SendTime;
        private const string SignType = "MD5";
        private const string Version = "V4.1.1.1.1";

        public ShengPayRequest()
        {
            this.SendTime = "";
            this.Name = "B2CPayment";
            this._payType = "PT001";
            this._payChannel = "";
            this._amount = "";
            this._orderNo = "";
            this._postBackUrl = "";
            this._notifyUrl = "";
            this._backUrl = "";
            this._merchantUserId = "";
            this._productNo = "";
            this._productDesc = "";
            this._orderTime = "";
            this._remark1 = "";
            this._remark2 = "";
            this._bankCode = "";
            this._productUrl = "";
            this.BuyerContact = "";
            this.BuyerIP = DataHelper.IPAddress;
        }

        public ShengPayRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.SendTime = "";
            this.Name = "B2CPayment";
            this._payType = "PT001";
            this._payChannel = "";
            this._amount = "";
            this._orderNo = "";
            this._postBackUrl = "";
            this._notifyUrl = "";
            this._backUrl = "";
            this._merchantUserId = "";
            this._productNo = "";
            this._productDesc = "";
            this._orderTime = "";
            this._remark1 = "";
            this._remark2 = "";
            this._bankCode = "";
            this._productUrl = "";
            this.BuyerContact = "";
            this.BuyerIP = DataHelper.IPAddress;
            this._orderNo = orderId;
            this._amount = amount.ToString("F2");
            this._postBackUrl = HttpUtility.UrlDecode(returnUrl);
            this._notifyUrl = HttpUtility.UrlDecode(notifyUrl);
            if (string.IsNullOrEmpty(showUrl))
            {
                this._backUrl = HttpUtility.UrlDecode(returnUrl);
            }
            else
            {
                this._backUrl = HttpUtility.UrlDecode(showUrl);
            }
            this._postBackUrl = "";
            this._productNo = subject;
            this._productUrl = showUrl;
            this._orderTime = date.ToString("yyyyMMddHHmmss");
            this.BuyerContact = buyerEmail;
            this._remark1 = attach;
        }

        public string GetSendTimeSpan(string merchantNo)
        {
            string url = string.Format("https://api.shengpay.com/mas/v1/timestamp?merchantNo={0}", merchantNo);
            try
            {
                string data = DataHelper.GetData(url, "GET");
                if (string.IsNullOrEmpty(data))
                {
                    return "";
                }
                return (string) JsonMapper.ToObject(data)["timestamp"];
            }
            catch (Exception)
            {
                return "";
            }
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            this.SendTime = this.GetSendTimeSpan(this.MerchantNo);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(string.IsNullOrEmpty(this.Name) ? "" : (this.Name + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty("V4.1.1.1.1") ? "" : "V4.1.1.1.1|", new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty("UTF-8") ? "" : "UTF-8|", new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.MerchantNo) ? "" : (this.MerchantNo + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.SendTime) ? "" : (this.SendTime + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._orderNo) ? "" : (this._orderNo + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._amount) ? "" : (this._amount + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._orderTime) ? "" : (this._orderTime + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty("CNY") ? "" : "CNY|", new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._payType) ? "" : (this._payType + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._payChannel) ? "" : (this._payChannel + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty("") ? "" : "|", new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._backUrl) ? "" : (this._backUrl + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._notifyUrl) ? "" : (this._notifyUrl + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._productNo) ? "" : (this._productNo + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.BuyerContact) ? "" : (this.BuyerContact + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this.BuyerIP) ? "" : (this.BuyerIP + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty(this._remark1) ? "" : (this._remark1 + "|"), new object[0]);
            builder.AppendFormat(string.IsNullOrEmpty("MD5") ? "" : "MD5|", new object[0]);
            string str = builder.ToString();
            string strValue = FormsAuthentication.HashPasswordForStoringInConfigFile(str + this.Key, "MD5");
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(this.CreateField("Name", this.Name));
            builder2.Append(this.CreateField("Version", "V4.1.1.1.1"));
            builder2.Append(this.CreateField("Charset", "UTF-8"));
            builder2.Append(this.CreateField("MsgSender", this.MerchantNo));
            builder2.Append(this.CreateField("SendTime", this.SendTime));
            builder2.Append(this.CreateField("OrderNo", this._orderNo));
            builder2.Append(this.CreateField("OrderAmount", this._amount));
            builder2.Append(this.CreateField("OrderTime", this._orderTime));
            builder2.Append(this.CreateField("Currency", "CNY"));
            builder2.Append(this.CreateField("PayType", this._payType));
            builder2.Append(this.CreateField("PayChannel", this._payChannel));
            builder2.Append(this.CreateField("InstCode", ""));
            builder2.Append(this.CreateField("PageUrl", this._backUrl));
            builder2.Append(this.CreateField("NotifyUrl", this._notifyUrl));
            builder2.Append(this.CreateField("ProductName", this._productNo));
            builder2.Append(this.CreateField("BuyerContact", this.BuyerContact));
            builder2.Append(this.CreateField("BuyerIp", this.BuyerIP));
            builder2.Append(this.CreateField("Ext1", this._remark1));
            builder2.Append(this.CreateField("SignType", "MD5"));
            builder2.Append(this.CreateField("SignMsg", strValue));
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("originStr", str);
            param.Add("Name", this.Name);
            param.Add("Version", "V4.1.1.1.1");
            param.Add("Charset", "UTF-8");
            param.Add("MsgSender", this.MerchantNo);
            param.Add("SendTime", this.SendTime);
            param.Add("OrderNo", this._orderNo);
            param.Add("OrderAmount", this._amount);
            param.Add("OrderTime", this._orderTime);
            param.Add("Currency", "CNY");
            param.Add("PayType", this._payType);
            param.Add("PayChannel", this._payChannel);
            param.Add("InstCode", "");
            param.Add("PageUrl", this._backUrl);
            param.Add("NotifyUrl", this._notifyUrl);
            param.Add("ProductName", this._productNo);
            param.Add("BuyerContact", this.BuyerContact);
            param.Add("BuyerIp", this.BuyerIP);
            param.Add("Ext1", this._remark1);
            param.Add("SignType", "MD5");
            param.Add("SignMsg", strValue);
            PayLog.AppendLog(param, strValue, this._notifyUrl, "支付日志", LogType.ShengpayMobile);
            this.SubmitPaymentForm(this.CreateForm(builder2.ToString(), "https://mas.shengpay.com/web-acquire-channel/cashier.htm"));
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
        public string MerchantNo { get; set; }

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

