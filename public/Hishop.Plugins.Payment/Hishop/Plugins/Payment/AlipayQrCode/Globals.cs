namespace Hishop.Plugins.Payment.AlipayQrCode
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Security;

    internal static class Globals
    {
        internal static string[] BubbleSort(string[] r)
        {
            for (int i = 0; i < r.Length; i++)
            {
                bool flag = false;
                for (int j = r.Length - 2; j >= i; j--)
                {
                    if (string.CompareOrdinal(r[j + 1], r[j]) < 0)
                    {
                        string str = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = str;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return r;
                }
            }
            return r;
        }

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
                current = enumerator.Current;
                string str2 = current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str2))
                {
                    if (flag)
                    {
                        builder.Append("&");
                    }
                    builder.Append(key);
                    builder.Append("=");
                    if ((encode && (key.ToLower() != "service")) && (key.ToLower() != "_input_charset"))
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

        internal static string CreatDirectUrl(string gateway, string service, string partner, string _input_charset, string sign_type, string method, string timestamp, string qrcode, string biz_type, string biz_data, string key)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("service", service);
            dict.Add("partner", partner);
            dict.Add("_input_charset", _input_charset);
            dict.Add("timestamp", timestamp);
            dict.Add("method", method);
            dict.Add("qrcode", qrcode);
            dict.Add("biz_type", biz_type);
            dict.Add("biz_data", biz_data);
            string str2 = FormsAuthentication.HashPasswordForStoringInConfigFile(BuildQuery(dict, false) + key, "MD5").ToLower();
            StringBuilder builder = new StringBuilder();
            builder.Append(gateway);
            builder.Append(BuildQuery(dict, true));
            builder.Append("&sign=" + str2 + "&sign_type=" + sign_type);
            return builder.ToString();
        }

        internal static string GetMD5(string s, string _input_charset)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder builder = new StringBuilder(0x20);
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
            }
            return builder.ToString();
        }

        public static SortedDictionary<string, string> SortParam(IDictionary<string, string> param)
        {
            return new SortedDictionary<string, string>(param);
        }

        internal static void writeLog(IDictionary<string, string> param, string sign, string url, string msg)
        {
            DataTable table = new DataTable {
                TableName = "log"
            };
            table.Columns.Add(new DataColumn("OperTime"));
            foreach (KeyValuePair<string, string> pair in param)
            {
                table.Columns.Add(new DataColumn(pair.Key));
            }
            table.Columns.Add(new DataColumn("Msg"));
            table.Columns.Add(new DataColumn("Sign"));
            table.Columns.Add(new DataColumn("Url"));
            DataRow row = table.NewRow();
            row["OperTime"] = DateTime.Now;
            foreach (KeyValuePair<string, string> pair in param)
            {
                row[pair.Key] = pair.Value;
            }
            row["Msg"] = msg;
            row["Sign"] = sign;
            row["Url"] = url;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Server.MapPath("/log.xml"));
        }
    }
}

