namespace Hishop.Plugins.Payment.WXQRCode
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Security;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;

    [Plugin("微信扫码支付")]
    public class WXQRCodeRequest : PaymentRequest
    {
        private const string Gateway = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        public WXQRCodeRequest()
        {
        }

        public WXQRCodeRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.ProductInfo = body;
            this.OrderId = orderId;
            this.TotalFee = amount;
            this.NotifyUrl = notifyUrl;
            this.Subject = subject;
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public string CoverDictionaryToString(Dictionary<string, string> data)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (KeyValuePair<string, string> pair in data)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    dictionary.Add(pair.Key, pair.Value);
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair2 in dictionary)
            {
                builder.Append(pair2.Key + "=" + pair2.Value + "&");
            }
            return builder.ToString().Substring(0, builder.Length - 1);
        }

        public string GetCodrUrl(string url, string postData)
        {
            Exception exception;
            string innerText = string.Empty;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            string xml = reader.ReadToEnd();
                            XmlDocument document = new XmlDocument();
                            try
                            {
                                document.LoadXml(xml);
                                if (document == null)
                                {
                                    return innerText;
                                }
                                XmlNode node = document.SelectSingleNode("xml/return_code");
                                XmlNode node2 = document.SelectSingleNode("xml/return_msg");
                                if (node == null)
                                {
                                    return innerText;
                                }
                                if (node.InnerText == "SUCCESS")
                                {
                                    XmlNode node3 = document.SelectSingleNode("xml/result_code");
                                    XmlNode node4 = document.SelectSingleNode("xml/err_code_des");
                                    if (node3.InnerText == "SUCCESS")
                                    {
                                        innerText = document.SelectSingleNode("xml/code_url").InnerText;
                                    }
                                    return innerText;
                                }
                                PayLog.AppendLog(null, postData, url, document.OuterXml, LogType.WXQRCode);
                                return ("error" + xml);
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                PayLog.AppendLog(null, null, this.NotifyUrl, exception.StackTrace + exception.Message, LogType.WXQRCode);
                            }
                        }
                        return innerText;
                    }
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                PayLog.AppendLog(null, null, this.NotifyUrl, exception.StackTrace + exception.Message, LogType.WXQRCode);
            }
            return innerText;
        }

        private string GetMD5String(string encypStr)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(encypStr);
            return BitConverter.ToString(provider.ComputeHash(bytes)).Replace("-", "").ToUpper();
        }

        private string GetStrnonce()
        {
            string str = string.Empty;
            Random random = new Random((int) (((ulong) DateTime.Now.Ticks) & 0xffffffffL));
            for (int i = 0; i < 0x1f; i++)
            {
                char ch;
                int num3 = random.Next();
                if ((num3 % 2) == 0)
                {
                    ch = (char) (0x30 + ((ushort) (num3 % 10)));
                }
                else
                {
                    ch = (char) (0x41 + ((ushort) (num3 % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        private string GetXmlData(Dictionary<string, string> param)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<xml>");
            foreach (string str in param.Keys)
            {
                string input = param[str];
                if (input == null)
                {
                    input = string.Empty;
                }
                if (Regex.IsMatch(input, "^[0-9.]$"))
                {
                    builder.Append("<" + str + ">" + input + "</" + str + ">");
                }
                else
                {
                    builder.Append("<" + str + "><![CDATA[" + input + "]]></" + str + ">");
                }
            }
            builder.Append("</xml>");
            return builder.ToString();
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (!(string.IsNullOrEmpty(this.Sub_AppId) || string.IsNullOrEmpty(this.Sub_mch_Id)))
            {
                data.Add("sub_appid", this.Sub_AppId);
                data.Add("sub_mch_id", this.Sub_mch_Id);
            }
            data.Add("appid", this.AppId);
            data.Add("mch_id", this.MCHID);
            data.Add("device_info", string.Empty);
            data.Add("nonce_str", this.GetStrnonce());
            data.Add("body", this.OrderId);
            data.Add("attach", string.Empty);
            data.Add("out_trade_no", this.OrderId);
            data.Add("total_fee", ((int) (this.TotalFee * 100M)).ToString());
            data.Add("spbill_create_ip", "127.0.0.1");
            data.Add("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.Add("time_expire", string.Empty);
            data.Add("goods_tag", string.Empty);
            data.Add("notify_url", this.NotifyUrl);
            data.Add("trade_type", "NATIVE");
            data.Add("openid", string.Empty);
            data.Add("product_id", this.OrderId);
            string str = this.GetMD5String(this.CoverDictionaryToString(data) + "&key=" + this.AppSecret).ToUpper();
            data.Add("sign", str);
            string codrUrl = this.GetCodrUrl("https://api.mch.weixin.qq.com/pay/unifiedorder", this.GetXmlData(data));
            if (!string.IsNullOrEmpty(codrUrl) && !codrUrl.StartsWith("error"))
            {
                if (this.Subject.Trim().Equals("预付款充值"))
                {
                    HttpContext.Current.Response.Redirect("/pay/WXQRCode.aspx?code_url=" + HttpUtility.UrlDecode(codrUrl) + "&orderId=" + HttpUtility.UrlDecode(this.OrderId) + "&isrecharge=1");
                }
                else if (this.Subject.Trim().Equals("分销商充值"))
                {
                    HttpContext.Current.Response.Redirect("/pay/WXQRCode.aspx?code_url=" + HttpUtility.UrlDecode(codrUrl) + "&orderId=" + HttpUtility.UrlDecode(this.OrderId) + "&isrecharge=2");
                }
                else if (this.Subject.Trim().Equals("采购单支付"))
                {
                    HttpContext.Current.Response.Redirect("/pay/WXQRCode.aspx?code_url=" + HttpUtility.UrlDecode(codrUrl) + "&orderId=" + HttpUtility.UrlDecode(this.OrderId) + "&isrecharge=3");
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/pay/WXQRCode.aspx?code_url=" + HttpUtility.UrlDecode(codrUrl) + "&orderId=" + HttpUtility.UrlDecode(this.OrderId));
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/pay/WXQRCode.aspx?code_url=getcodeurl_error&orderId=" + HttpUtility.UrlDecode(this.OrderId));
            }
        }

        [ConfigElement("AppId", Description = "由服务商代理申请的微信支付时，请填写服务商提供的AppId,否则填写商户自己的AppId", Nullable = false)]
        public string AppId { get; set; }


        [ConfigElement("key", Nullable=false, Description="由服务商代理申请的微信支付时，请填写服务商提供的Key,否则填写商户自己设置的Key")]
        public string AppSecret { get; set; }

        public string CertPassword
        {
            get
            {
                return this.MCHID;
            }
        }

        [ConfigElement("证书路径", Nullable=true, InputType=(InputType) 6, Description="证书用于企业帐号支付以及退款原路返回，请使用扩展名为p12的证书文件")]
        public string CertPath { get; set; }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        private string DeviceInfo { get; set; }

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

        [ConfigElement("mch_id", Nullable=false, Description="由服务商代理申请的微信支付时，请填写服务商提供的mch_id,否则填写商户自己的mch_id")]
        public string MCHID { get; set; }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        private string NotifyUrl { get; set; }

        private string OrderId { get; set; }

        private string ProductInfo { get; set; }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }

        private string StrNonce { get; set; }

        [ConfigElement("子商户AppId", Nullable=true, Description="由服务商代理申请的微信支付需要填写该项，填写商户自己的AppId,否则不需要填写")]
        public string Sub_AppId { get; set; }

        [ConfigElement("子商户号", Nullable=true, Description="由服务商代理申请的微信支付需要填写该项,填写商户自己的Mch_Id,否则不需要填写")]
        public string Sub_mch_Id { get; set; }

        private string Subject { get; set; }

        private decimal TotalFee { get; set; }
    }
}

