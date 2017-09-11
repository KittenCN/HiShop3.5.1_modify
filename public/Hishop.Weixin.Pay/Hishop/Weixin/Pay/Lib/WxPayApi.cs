namespace Hishop.Weixin.Pay.Lib
{
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Runtime.InteropServices;

    public class WxPayApi
    {
        public static WxPayData CloseOrder(WxPayData inputObj, PayConfig config, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("nonce_str", GenerateNonceStr());
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = inputObj.ToXml();
            DateTime now = DateTime.Now;
            string str3 = HttpService.Post(xml, url, false, config, timeOut);
            TimeSpan span = (TimeSpan) (DateTime.Now - now);
            double totalMilliseconds = span.TotalMilliseconds;
            WxPayData data = new WxPayData();
            data.FromXml(str3, config.Key);
            return data;
        }

        public static WxPayData DownloadBill(WxPayData inputObj, PayConfig config, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("nonce_str", GenerateNonceStr());
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = HttpService.Post(inputObj.ToXml(), url, false, config, timeOut);
            WxPayData data = new WxPayData();
            if (xml.Substring(0, 5) == "<xml>")
            {
                data.FromXml(xml, config.Key);
                return data;
            }
            data.SetValue("result", xml);
            return data;
        }

        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string GenerateOutTradeNo(PayConfig config)
        {
            Random random = new Random();
            return string.Format("{0}{1}{2}", config.MchID, DateTime.Now.ToString("yyyyMMddHHmmss"), random.Next(0x3e7));
        }

        public static string GenerateTimeStamp()
        {
            TimeSpan span = (TimeSpan) (DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0));
            return Convert.ToInt64(span.TotalSeconds).ToString();
        }

        public static WxPayData Micropay(WxPayData inputObj, PayConfig config, int timeOut = 10)
        {
            string url = "https://api.mch.weixin.qq.com/pay/micropay";
            inputObj.SetValue("spbill_create_ip", config.IPAddress);
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = inputObj.ToXml();
            DateTime now = DateTime.Now;
            string str3 = HttpService.Post(xml, url, false, config, timeOut);
            TimeSpan span = (TimeSpan) (DateTime.Now - now);
            double totalMilliseconds = span.TotalMilliseconds;
            WxPayData data = new WxPayData();
            data.FromXml(str3, config.Key);
            return data;
        }

        public static WxPayData OrderQuery(WxPayData inputObj, PayConfig config, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("nonce_str", GenerateNonceStr());
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = inputObj.ToXml();
            DateTime now = DateTime.Now;
            string str3 = HttpService.Post(xml, url, false, config, timeOut);
            TimeSpan span = (TimeSpan) (DateTime.Now - now);
            double totalMilliseconds = span.TotalMilliseconds;
            WxPayData data = new WxPayData();
            data.FromXml(str3, config.Key);
            return data;
        }

        public static WxPayData Refund(WxPayData inputObj, PayConfig config, int timeOut = 60)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = inputObj.ToXml();
            DateTime now = DateTime.Now;
            string str3 = HttpService.Post(xml, url, true, config, timeOut);
            WxPayData data = new WxPayData();
            if (!str3.StartsWith("POSTERROR"))
            {
                data.FromXml(str3, config.Key);
                return data;
            }
            data.SetValue("return_msg", str3.Replace("POSTERROR", "").Replace("\r", "").Replace("\n", ""));
            return data;
        }

        public static WxPayData RefundQuery(WxPayData inputObj, PayConfig config, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/refundquery";
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("nonce_str", GenerateNonceStr());
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = inputObj.ToXml();
            DateTime now = DateTime.Now;
            string str3 = HttpService.Post(xml, url, false, config, timeOut);
            TimeSpan span = (TimeSpan) (DateTime.Now - now);
            double totalMilliseconds = span.TotalMilliseconds;
            WxPayData data = new WxPayData();
            data.FromXml(str3, config.Key);
            return data;
        }

        public static WxPayData Reverse(WxPayData inputObj, PayConfig config, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("nonce_str", GenerateNonceStr());
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = inputObj.ToXml();
            DateTime now = DateTime.Now;
            string str3 = HttpService.Post(xml, url, true, config, timeOut);
            TimeSpan span = (TimeSpan) (DateTime.Now - now);
            double totalMilliseconds = span.TotalMilliseconds;
            WxPayData data = new WxPayData();
            data.FromXml(str3, config.Key);
            return data;
        }

        public static WxPayData ShortUrl(WxPayData inputObj, PayConfig config, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/tools/shorturl";
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("nonce_str", GenerateNonceStr());
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = inputObj.ToXml();
            DateTime now = DateTime.Now;
            string str3 = HttpService.Post(xml, url, false, config, timeOut);
            TimeSpan span = (TimeSpan) (DateTime.Now - now);
            double totalMilliseconds = span.TotalMilliseconds;
            WxPayData data = new WxPayData();
            data.FromXml(str3, config.Key);
            return data;
        }

        public static WxPayData UnifiedOrder(WxPayData inputObj, PayConfig config, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            if (!inputObj.IsSet("NOTIFY_URL"))
            {
                inputObj.SetValue("NOTIFY_URL", config.NOTIFY_URL);
            }
            inputObj.SetValue("appid", config.AppId);
            inputObj.SetValue("mch_id", config.MchID);
            inputObj.SetValue("spbill_create_ip", config.IPAddress);
            inputObj.SetValue("nonce_str", GenerateNonceStr());
            inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
            string xml = inputObj.ToXml();
            DateTime now = DateTime.Now;
            string str3 = HttpService.Post(xml, url, false, config, timeOut);
            TimeSpan span = (TimeSpan) (DateTime.Now - now);
            double totalMilliseconds = span.TotalMilliseconds;
            WxPayData data = new WxPayData();
            data.FromXml(str3, config.Key);
            return data;
        }
    }
}

