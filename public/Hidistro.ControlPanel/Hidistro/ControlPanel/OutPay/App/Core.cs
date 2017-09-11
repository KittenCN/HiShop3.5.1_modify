namespace Hidistro.ControlPanel.OutPay.App
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class Core
    {
        public static string _input_charset = "";
        public static string _partner = "";
        public static string _private_key = "";
        public static string _sign_type = "";
        public static string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";

        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary = BuildRequestPara(sParaTemp);
            StringBuilder builder = new StringBuilder();
            builder.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + GATEWAY_NEW + "_input_charset=" + _input_charset + "' method='" + strMethod.ToLower().Trim() + "'>");
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                builder.Append("<input type='hidden' name='" + pair.Key + "' value='" + pair.Value + "'/>");
            }
            builder.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");
            builder.Append("<script>document.forms['alipaysubmit'].submit();</script>");
            return builder.ToString();
        }

        private static string BuildRequestMysign(Dictionary<string, string> sPara)
        {
            string content = CreateLinkString(sPara);
            switch (_sign_type)
            {
                case "RSA":
                    return RSAFromPkcs8.sign(content, _private_key, _input_charset);

                case "MD5":
                    return GetMD5(content + _private_key, _input_charset);
            }
            return "";
        }

        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            string str = "";
            sPara = FilterPara(sParaTemp);
            str = BuildRequestMysign(sPara);
            sPara.Add("sign", str);
            sPara.Add("sign_type", _sign_type);
            return sPara;
        }

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

        public static string GetMD5(string myString, string _input_charset = "utf-8")
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.GetEncoding(_input_charset).GetBytes(myString);
            byte[] buffer2 = md.ComputeHash(bytes);
            string str = null;
            for (int i = 0; i < buffer2.Length; i++)
            {
                str = str + buffer2[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }

        public static void LogResult(string sWord)
        {
            StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("log") + @"\" + DateTime.Now.ToString().Replace(":", "") + ".txt", false, Encoding.Default);
            writer.Write(sWord);
            writer.Close();
        }

        public static void setConfig(string partner, string sing_type, string private_key, string input_charset)
        {
            _partner = partner;
            _sign_type = sing_type;
            _input_charset = input_charset;
            _private_key = private_key;
        }
    }
}

