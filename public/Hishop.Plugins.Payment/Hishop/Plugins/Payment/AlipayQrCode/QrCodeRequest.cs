namespace Hishop.Plugins.Payment.AlipayQrCode
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Xml;

    [Plugin("支付宝扫码支付", Sequence=4)]
    public class QrCodeRequest : PaymentRequest
    {
        private const string Agent = "C4335302345904805116";
        private const string biztype = "10";
        private readonly string body;
        private readonly string ext_info;
        private const string extend_param = "isv^yf31";
        private const string Gateway = "https://mapi.alipay.com/gateway.do?";
        private const string InputCharset = "UTF-8";
        private const string method = "add";
        private readonly string notifyUrl;
        private readonly string outTradeNo;
        private static IDictionary<string, string> param = new Dictionary<string, string>();
        private const string PaymentType = "1";
        private readonly string query_url;
        private readonly string returnUrl;
        private const string Service = "alipay.mobile.qrcode.manage";
        private readonly string showUrl;
        private const string SignType = "MD5";
        private readonly string subject;
        private readonly string totalFee;

        public QrCodeRequest()
        {
            this.query_url = "";
            this.ext_info = "";
        }

        public QrCodeRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.query_url = "";
            this.ext_info = "";
            this.body = body;
            this.outTradeNo = orderId;
            this.subject = subject;
            this.timestamp = date.ToString("yyyy-MM-dd hh:mm:ss");
            this.totalFee = amount.ToString("F", CultureInfo.InvariantCulture);
            this.returnUrl = returnUrl;
            this.notifyUrl = notifyUrl;
            this.showUrl = showUrl;
            this.trade_type = "1";
            this.need_address = "F";
            this.memo = attach;
            this.id = orderId;
            this.name = subject;
            this.price = amount.ToString("f2");
            this.inventory = "";
            this.sku_title = "";
            this.sku = "";
            this.expiry_date = "";
            this.desc = body;
            param.Clear();
            param.Add("body", body);
            param.Add("outTradeNo", orderId);
            param.Add("subject", subject);
            param.Add("timestamp", this.timestamp);
            param.Add("totalFee", this.totalFee);
            param.Add("returnUrl", returnUrl);
            param.Add("notifyUrl", notifyUrl);
            param.Add("showUrl", showUrl);
            param.Add("trade_type", this.trade_type);
            param.Add("need_address", this.need_address);
            param.Add("memo", this.memo);
            param.Add("id", this.id);
            param.Add("name", this.name);
            param.Add("price", this.price);
            param.Add("inventory", this.inventory);
            param.Add("sku_title", this.sku_title);
            param.Add("sku", this.sku);
            param.Add("expiry_date", this.expiry_date);
            param.Add("desc", this.desc);
        }

        public QrCodeRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach, string tradeType, string QRCode, bool NeedAddress, string productId)
        {
            this.query_url = "";
            this.ext_info = "";
            this.body = body;
            this.outTradeNo = orderId;
            this.subject = subject;
            this.timestamp = date.ToString("yyyy-MM-dd hh:mm:ss");
            this.totalFee = amount.ToString("F", CultureInfo.InvariantCulture);
            this.returnUrl = returnUrl;
            this.notifyUrl = notifyUrl;
            this.showUrl = showUrl;
            this.trade_type = tradeType;
            this.qrCode = (QRCode == "") ? "" : ("https://qr.alipay.com/" + QRCode);
            this.need_address = NeedAddress ? "T" : "F";
            this.memo = attach;
            this.id = (productId == "") ? orderId : productId;
            this.name = subject;
            this.price = amount.ToString("f2");
            this.inventory = "";
            this.sku_title = "";
            this.sku = "";
            this.expiry_date = "";
            this.desc = body;
            param.Clear();
            param.Add("body", body);
            param.Add("outTradeNo", orderId);
            param.Add("subject", subject);
            param.Add("timestamp", this.timestamp);
            param.Add("totalFee", this.totalFee);
            param.Add("returnUrl", returnUrl);
            param.Add("notifyUrl", notifyUrl);
            param.Add("qrCode", this.qrCode);
            param.Add("need_address", this.need_address);
            param.Add("memo", this.memo);
            param.Add("id", this.id);
            param.Add("name", this.name);
            param.Add("price", this.price);
            param.Add("inventory", this.inventory);
            param.Add("sku_title", this.sku_title);
            param.Add("sku", this.sku);
            param.Add("expiry_date", this.expiry_date);
            param.Add("desc", this.desc);
        }

        public string buildBizdata()
        {
            StringBuilder builder = new StringBuilder("{");
            builder.AppendFormat("\"trade_type\":\"{0}\",", this.trade_type);
            builder.AppendFormat("\"need_address\":\"{0}\",", this.need_address);
            builder.AppendFormat("\"goods_info\":{0},", this.buildGoodsInfo());
            if (!string.IsNullOrEmpty(this.returnUrl))
            {
                builder.AppendFormat("\"return_url\":\"{0}\",", this.returnUrl);
            }
            if (!string.IsNullOrEmpty(this.notifyUrl))
            {
                builder.AppendFormat("\"notify_url\":\"{0}\",", this.notifyUrl);
            }
            if (!string.IsNullOrEmpty(this.query_url))
            {
                builder.AppendFormat("\"query_url\":\"{0}\",", this.query_url);
            }
            if (!string.IsNullOrEmpty(this.ext_info))
            {
                builder.AppendFormat("\"ext_info\":{0},", "{}");
            }
            if (!string.IsNullOrEmpty(this.memo))
            {
                builder.AppendFormat("\"memo\":\"{0}\",", this.memo);
            }
            if (!string.IsNullOrEmpty(this.showUrl))
            {
                builder.AppendFormat("\"url\":\"{0}\",", HttpUtility.UrlDecode(this.showUrl));
            }
            param.Add("bizdata", builder.ToString().Substring(0, builder.ToString().Length - 1) + "}");
            return (builder.ToString().Substring(0, builder.ToString().Length - 1) + "}");
        }

        public string buildGoodsInfo()
        {
            StringBuilder builder = new StringBuilder("{");
            builder.AppendFormat("\"id\":\"{0}\",", this.id);
            builder.AppendFormat("\"name\":\"{0}\",", this.name);
            builder.AppendFormat("\"price\":\"{0}\",", this.price);
            if (!string.IsNullOrEmpty(this.inventory))
            {
                builder.AppendFormat("\"inventory\":\"{0}\",", this.inventory);
            }
            if (!string.IsNullOrEmpty(this.sku_title))
            {
                builder.AppendFormat("\"sku_title\":\"{0}\",", this.sku_title);
            }
            if (!string.IsNullOrEmpty(this.sku))
            {
                builder.AppendFormat("\"sku\":{0},", this.sku);
            }
            if (!string.IsNullOrEmpty(this.expiry_date))
            {
                builder.AppendFormat("\"expiry_date\":\"{0}\",", this.expiry_date);
            }
            if (!string.IsNullOrEmpty(this.desc))
            {
                builder.AppendFormat("\"desc\":\"{0}\",", this.desc);
            }
            param.Add("goodsinfo", builder.ToString().Substring(0, builder.ToString().Length - 1) + "}");
            return (builder.ToString().Substring(0, builder.ToString().Length - 1) + "}");
        }

        public static string PostData(string url, string postData, out string QRCode, out bool Success)
        {
            Exception exception;
            QRCode = "";
            Success = false;
            string xml = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = postData.Length;
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(postData);
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        xml = new StreamReader(stream, encoding).ReadToEnd();
                        XmlDocument document = new XmlDocument();
                        try
                        {
                            document.LoadXml(xml);
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            xml = string.Format("获取信息错误doc.load：{0}", exception.Message) + xml;
                        }
                        try
                        {
                            if (document == null)
                            {
                                return xml;
                            }
                            XmlNode node = document.SelectSingleNode("alipay/is_success");
                            if (node == null)
                            {
                                return xml;
                            }
                            if (node.InnerText == "T")
                            {
                                Success = true;
                                XmlNode node2 = document.SelectSingleNode("alipay/response/alipay/qrcode");
                                XmlNode node3 = document.SelectSingleNode("alipay/response/alipay/qrcode_img_url");
                                if ((node2 == null) && (node3 == null))
                                {
                                    Success = false;
                                    XmlNode node4 = document.SelectSingleNode("alipay/response/alipay/error_message");
                                    XmlNode node5 = document.SelectSingleNode("alipay/response/alipay/result_code");
                                    return (node4.InnerText + "---" + node5.InnerText);
                                }
                                if (node2 != null)
                                {
                                    QRCode = node2.InnerText;
                                }
                                if (node3 != null)
                                {
                                    return node3.InnerText;
                                }
                            }
                            else
                            {
                                return document.OuterXml;
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            xml = string.Format("获取信息错误node.load：{0}", exception.Message) + xml;
                        }
                        return xml;
                    }
                }
            }
            catch (Exception exception3)
            {
                exception = exception3;
                xml = string.Format("获取信息错误post error：{0}", exception.Message) + xml;
            }
            return xml;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            string url = Globals.CreatDirectUrl("https://mapi.alipay.com/gateway.do?", "alipay.mobile.qrcode.manage", this.Partner, "UTF-8", "MD5", "add", this.timestamp, this.qrCode, "10", this.buildBizdata(), this.Key);
            string qRCode = "";
            bool success = false;
            string str = PostData(url, "", out qRCode, out success);
            try
            {
                if (success)
                {
                    HttpContext.Current.Response.Redirect("/pay/QrCode.aspx?status=1&QRCodeImg=" + HttpUtility.UrlDecode(str) + "&QrCodeUrl=" + HttpUtility.UrlDecode(qRCode) + "&OrderId=" + this.outTradeNo);
                }
                else
                {
                    Globals.writeLog(param, "", HttpContext.Current.Request.Url.ToString(), str);
                    HttpContext.Current.Response.Redirect("/pay/QrCode.aspx?QRCodeImg=F&status=0&OrderId=" + this.outTradeNo);
                }
            }
            catch (Exception exception)
            {
                HttpContext.Current.Response.Write("/pay/QrCode.aspx?status=1&QRCodeImg=" + HttpUtility.UrlDecode(str) + "&QrCodeUrl=" + HttpUtility.UrlDecode(qRCode) + "&OrderId=" + this.outTradeNo + "<br>" + exception.Message);
            }
        }

        private string desc { get; set; }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        private string expiry_date { get; set; }

        private string id { get; set; }

        private string inventory { get; set; }

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

        private string memo { get; set; }

        private string name { get; set; }

        private string need_address { get; set; }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        [ConfigElement("合作者身份(PID)", Nullable=false)]
        public string Partner { get; set; }

        private string price { get; set; }

        private string qrCode { get; set; }

        [ConfigElement("收款支付宝账号", Nullable=false)]
        public string SellerEmail { get; set; }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }

        private string sku { get; set; }

        private string sku_title { get; set; }

        private string timestamp { get; set; }

        private string trade_type { get; set; }
    }
}

