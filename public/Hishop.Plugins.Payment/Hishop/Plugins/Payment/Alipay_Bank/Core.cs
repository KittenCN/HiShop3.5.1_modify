namespace Hishop.Plugins.Payment.Alipay_Bank
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class Core
    {
        public static string CreateLinkString(Dictionary<string, string> dicArray)
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

        public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dicArray)
            {
                builder.Append(pair.Key + "=" + HttpUtility.UrlEncode(pair.Value, code) + "&");
            }
            int length = builder.Length;
            builder.Remove(length - 1, 1);
            return builder.ToString();
        }

        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in dicArrayPre)
            {
                if (((pair.Key.ToLower() != "sign") && (pair.Key.ToLower() != "sign_type")) && ((pair.Value != "") && (pair.Value != null)))
                {
                    dictionary.Add(pair.Key, pair.Value);
                }
            }
            return dictionary;
        }

        public static string GetAbstractToMD5(Stream sFile)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(sFile);
            StringBuilder builder = new StringBuilder(0x20);
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
            }
            return builder.ToString();
        }

        public static string GetAbstractToMD5(byte[] dataFile)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(dataFile);
            StringBuilder builder = new StringBuilder(0x20);
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
            }
            return builder.ToString();
        }

        public static void LogResult(string sWord)
        {
            StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("log") + @"\" + DateTime.Now.ToString().Replace(":", "") + ".txt", false, Encoding.Default);
            writer.Write(sWord);
            writer.Close();
        }
    }
}

