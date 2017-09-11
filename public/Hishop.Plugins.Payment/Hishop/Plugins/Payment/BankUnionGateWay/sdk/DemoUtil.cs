namespace Hishop.Plugins.Payment.BankUnionGateWay.sdk
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    public class DemoUtil
    {
        public static string md5string(string sourcestring, Encoding encoder)
        {
            string str = "";
            byte[] buffer = MD5.Create().ComputeHash(encoder.GetBytes(sourcestring));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                str = str + buffer[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }

        public static string pasMap(Dictionary<string, string> data)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (KeyValuePair<string, string> pair in data)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair2 in dictionary)
            {
                builder.Append(pair2.Key + "=" + pair2.Value + "&");
            }
            return builder.ToString();
        }

        public static Dictionary<string, string> pasRes(string res)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(res))
            {
                string[] strArray = res.Split(new char[] { '&' });
                foreach (string str in strArray)
                {
                    if (str.StartsWith("fileContent"))
                    {
                        dictionary.Add("fileContent", str.Substring(12));
                    }
                    else
                    {
                        string[] strArray2 = str.Split(new char[] { '=' });
                        if (2 == strArray2.Length)
                        {
                            dictionary.Add(strArray2[0], strArray2[1]);
                        }
                    }
                }
            }
            return dictionary;
        }
    }
}

