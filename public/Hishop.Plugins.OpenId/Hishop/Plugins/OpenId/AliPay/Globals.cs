namespace Hishop.Plugins.OpenId.AliPay
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    public class Globals
    {
        public static string BuildSign(Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
        {
            return Sign(CreateLinkstring(dicArray) + key, sign_type, _input_charset);
        }

        public static string CreateLinkstring(Dictionary<string, string> dicArray)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dicArray)
            {
                builder.Append(pair.Key + "=" + pair.Value + "&");
            }
            int length = builder.Length;
            builder.Remove(length - 1, 1);
            return builder.ToString();
        }

        public static Dictionary<string, string> Parameterfilter(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in dicArrayPre)
            {
                if (((pair.Key.ToLower() != "sign") && (pair.Key.ToLower() != "sign_type")) && ((pair.Value != "") && (pair.Value != null)))
                {
                    string key = pair.Key.ToLower();
                    dictionary.Add(key, pair.Value);
                }
            }
            return dictionary;
        }

        public static string Sign(string prestr, string sign_type, string _input_charset)
        {
            StringBuilder builder = new StringBuilder(0x20);
            if (sign_type.ToUpper() == "MD5")
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return builder.ToString();
        }
    }
}

