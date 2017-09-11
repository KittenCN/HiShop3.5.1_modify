namespace Hishop.Plugins.Payment.BankUnion
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class QuickPayUtils
    {
        public int checkSecurity(string[] valueVo)
        {
            SortedDictionary<string, string> map = new SortedDictionary<string, string>(StringComparer.Ordinal);
            for (int i = 0; i < valueVo.Length; i++)
            {
                string[] strArray = valueVo[i].Split(new char[] { '=' });
                map.Add(strArray[0], (strArray.Length >= 2) ? valueVo[i].Substring(strArray[0].Length + 1) : "");
            }
            if (map["signature"] == "")
            {
                return 2;
            }
            if (string.Compare(QuickPayConf.signType, map["signMethod"], true) == 0)
            {
                string strA = map["signature"];
                map.Remove("signature");
                map.Remove("signMethod");
                if (string.Compare(strA, this.md5(this.joinMapValue(map, '&') + this.md5(QuickPayConf.securityKey))) == 0)
                {
                    return 1;
                }
                return 0;
            }
            return 0;
        }

        public bool checkSign(string[] valueVo, string signMethod, string signature)
        {
            SortedDictionary<string, string> map = new SortedDictionary<string, string>(StringComparer.Ordinal);
            for (int i = 0; i < QuickPayConf.notifyVo.Length; i++)
            {
                map.Add(QuickPayConf.notifyVo[i], valueVo[i]);
            }
            if (signature == null)
            {
                return false;
            }
            return ((string.Compare(QuickPayConf.signType, signMethod, true) == 0) && (string.Compare(signature, this.md5(this.joinMapValue(map, '&') + this.md5(QuickPayConf.securityKey))) == 0));
        }

        public string createBackStr(string[] valueVo, string[] keyVo)
        {
            int num;
            SortedDictionary<string, string> map = new SortedDictionary<string, string>(StringComparer.Ordinal);
            for (num = 0; num < keyVo.Length; num++)
            {
                map.Add(keyVo[num], valueVo[num]);
            }
            SortedDictionary<string, string> dictionary2 = new SortedDictionary<string, string>(StringComparer.Ordinal);
            for (num = 0; num < keyVo.Length; num++)
            {
                dictionary2.Add(keyVo[num], HttpUtility.UrlEncode(valueVo[num]));
            }
            dictionary2.Add("signature", this.signMap(map, QuickPayConf.signType));
            dictionary2.Add("signMethod", QuickPayConf.signType);
            return this.joinMapValue(dictionary2, '&');
        }

        public string createPayHtml(string[] valueVo)
        {
            return this.createPayHtml(valueVo, null);
        }

        public string createPayHtml(string[] valueVo, string bank)
        {
            SortedDictionary<string, string> map = new SortedDictionary<string, string>(StringComparer.Ordinal);
            for (int i = 0; i < QuickPayConf.reqVo.Length; i++)
            {
                map.Add(QuickPayConf.reqVo[i], valueVo[i]);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("<script language=\"javascript\">window.onload=function(){document.pay_form.submit();}</script>");
            builder.Append("<form id=\"pay_form\" name=\"pay_form\" action=\"").Append(QuickPayConf.gateWay).Append("\" method=\"post\">");
            foreach (KeyValuePair<string, string> pair in map)
            {
                builder.Append("<input type=\"hidden\" name=\"" + pair.Key + "\" id=\"" + pair.Key + "\" value=\"" + pair.Value + "\" />");
            }
            builder.Append("<input type=\"hidden\" name=\"signature\" id=\"signature\" value=\"" + this.signMap(map, QuickPayConf.signType) + "\">");
            builder.Append("<input type=\"hidden\" name=\"signMethod\" id=\"signMethod\" value=\"" + QuickPayConf.signType + "\" />");
            if ((bank != null) && (bank != ""))
            {
                builder.Append("<input type=\"hidden\" name=\"t\" id=\"t\" value=\"5\" />");
                builder.Append("<input type=\"hidden\" name=\"bank\" id=\"bank\" value=\"" + bank + "\" />");
            }
            builder.Append("</form>");
            return builder.ToString();
        }

        public string doPostQueryCmd(string strURL, string req)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(strURL);
            Encoding encoding = Encoding.GetEncoding(QuickPayConf.charset);
            byte[] bytes = encoding.GetBytes(req);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Flush();
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), encoding);
            string str = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return str;
        }

        public string[] getResArr(string str)
        {
            string str2 = "cupReserved=";
            int index = str.IndexOf(str2);
            int startIndex = str.IndexOf('{', index + str2.Length);
            int num3 = str.IndexOf('}', startIndex);
            string oldValue = str.Substring(startIndex, (num3 - startIndex) + 1);
            str = str.Replace(oldValue, "");
            string[] strArray = str.Split(new char[] { '&' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (string.Compare(strArray[i], "cupReserved=") == 0)
                {
                    string[] strArray3;
                    IntPtr ptr;
                    (strArray3 = strArray)[(int) (ptr = (IntPtr) i)] = strArray3[(int) ptr] + oldValue;
                }
            }
            return strArray;
        }

        private string joinMapValue(SortedDictionary<string, string> map, char connector)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in map)
            {
                builder.Append(pair.Key);
                builder.Append('=');
                if (pair.Value != null)
                {
                    builder.Append(pair.Value);
                }
                builder.Append(connector);
            }
            return builder.ToString();
        }

        public string md5(string str)
        {
            if (str == null)
            {
                return null;
            }
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.GetEncoding(QuickPayConf.charset).GetBytes(str);
            byte[] buffer2 = md.ComputeHash(bytes);
            string str2 = null;
            for (int i = 0; i < buffer2.Length; i++)
            {
                string str3 = buffer2[i].ToString("X2").ToLower();
                str2 = str2 + str3;
            }
            return str2;
        }

        public string signMap(SortedDictionary<string, string> map, string signMethod)
        {
            if (string.Compare(QuickPayConf.signType, signMethod, true) == 0)
            {
                return this.md5(this.joinMapValue(map, '&') + this.md5(QuickPayConf.securityKey));
            }
            return "";
        }
    }
}

