namespace Hishop.Plugins.Payment.WS_WapPay
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    public class Submit
    {
        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp, string input_charset, string key, string sign_type)
        {
            string str = "";
            str = Function.BuildMysign(sParaTemp, key, sign_type, input_charset);
            Dictionary<string, string> dictionary = Function.FilterPara(sParaTemp);
            dictionary.Add("sign", str);
            return dictionary;
        }

        private static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, string input_charset, string key, string sign_type)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            return Function.CreateLinkString(BuildRequestPara(sParaTemp, input_charset, key, sign_type));
        }

        public static string SendPostInfo(SortedDictionary<string, string> sParaTemp, string gateway, string input_charset, string key, string sign_type)
        {
            string s = BuildRequestParaToString(sParaTemp, input_charset, key, sign_type);
            Encoding encoding = Encoding.GetEncoding(input_charset);
            byte[] bytes = encoding.GetBytes(s);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(gateway);
            request.Method = "post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), encoding);
                return reader.ReadToEnd();
            }
        }

        public static string SendPostRedirect(string req_url, SortedDictionary<string, string> sParaTemp, string gateway, string input_charset, string key, string sign_type)
        {
            string str = BuildRequestParaToString(sParaTemp, input_charset, key, sign_type);
            return (req_url + "?" + str);
        }
    }
}

