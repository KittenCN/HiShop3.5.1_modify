namespace Hishop.Plugins.Payment.GoPay
{
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    internal static class Globals
    {
        public static string Get_Http(string strUrl, int timeout)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(strUrl);
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                StringBuilder builder = new StringBuilder();
                while (-1 != reader.Peek())
                {
                    builder.Append(reader.ReadLine());
                }
                return builder.ToString();
            }
            catch (Exception exception)
            {
                return ("错误：" + exception.Message);
            }
        }

        public static string GetGopayServerTime()
        {
            return Get_Http("https://mertest.gopay.com.cn/PGServer/time", 0x2710);
        }

        public static string GetMD5(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string str2 = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str2 = str2 + bytes[i].ToString("x").PadLeft(2, '0');
            }
            return str2;
        }

        public static string getRealIp()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
    }
}

