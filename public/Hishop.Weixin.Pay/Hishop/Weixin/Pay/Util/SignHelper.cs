namespace Hishop.Weixin.Pay.Util
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Web.Security;

    internal class SignHelper
    {
        public static string BuildQuery(IDictionary<string, string> dict, bool encode)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(dict);
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            IEnumerator<KeyValuePair<string, string>> enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                KeyValuePair<string, string> pair2 = enumerator.Current;
                string str2 = pair2.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str2))
                {
                    if (flag)
                    {
                        builder.Append("&");
                    }
                    builder.Append(key);
                    builder.Append("=");
                    if (encode)
                    {
                        builder.Append(HttpUtility.UrlEncode(str2, Encoding.UTF8));
                    }
                    else
                    {
                        builder.Append(str2);
                    }
                    flag = true;
                }
            }
            return builder.ToString();
        }

        public static string BuildXml(IDictionary<string, string> dict, bool encode)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(dict);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<xml>");
            IEnumerator<KeyValuePair<string, string>> enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                KeyValuePair<string, string> pair2 = enumerator.Current;
                string str2 = pair2.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str2))
                {
                    decimal result = 0M;
                    bool flag = false;
                    if (!decimal.TryParse(str2, out result))
                    {
                        flag = true;
                    }
                    if (encode)
                    {
                        builder.AppendLine("<" + key + ">" + (flag ? "<![CDATA[" : "") + HttpUtility.UrlEncode(str2, Encoding.UTF8) + (flag ? "]]>" : "") + "</" + key + ">");
                    }
                    else
                    {
                        builder.AppendLine("<" + key + ">" + (flag ? "<![CDATA[" : "") + str2 + (flag ? "]]>" : "") + "</" + key + ">");
                    }
                }
            }
            builder.AppendLine("</xml>");
            return builder.ToString();
        }

        public static string SignPackage(IDictionary<string, string> parameters, string partnerKey)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(BuildQuery(parameters, false) + string.Format("&key={0}", partnerKey), "MD5").ToUpper();
        }

        public static string SignPay(IDictionary<string, string> parameters, string key = "")
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(BuildQuery(parameters, false) + string.Format("&key={0}", key), "MD5").ToUpper();
        }
    }
}

