namespace Hishop.Plugins.Payment.BankUnionGateWay.sdk
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public class SDKUtil
    {
        public static string CoverDictionaryToString(Dictionary<string, string> data)
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
            return builder.ToString().Substring(0, builder.Length - 1);
        }

        public static Dictionary<string, string> CoverstringToDictionary(string data)
        {
            if ((data == null) || (0 == data.Length))
            {
                return null;
            }
            string[] strArray = data.Split(new char[] { '&' });
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string str in strArray)
            {
                int index = str.IndexOf("=");
                string key = str.Substring(0, index);
                string str3 = str.Substring(index + 1);
                Console.WriteLine(key + "=" + str3);
                dictionary.Add(key, str3);
            }
            return dictionary;
        }

        public static string CreateAutoSubmitForm(string url, Dictionary<string, string> data, Encoding encoder)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", encoder.BodyName);
            builder.AppendLine("</head>");
            builder.AppendLine("<body onload=\"OnLoadSubmit();\">");
            builder.AppendFormat("<form id=\"pay_form\" action=\"{0}\" method=\"post\">", url);
            foreach (KeyValuePair<string, string> pair in data)
            {
                builder.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", pair.Key, pair.Value);
            }
            builder.AppendLine("</form>");
            builder.AppendLine("<script type=\"text/javascript\">");
            builder.AppendLine("<!--");
            builder.AppendLine("function OnLoadSubmit()");
            builder.AppendLine("{");
            builder.AppendLine("document.getElementById(\"pay_form\").submit();");
            builder.AppendLine("}");
            builder.AppendLine("//-->");
            builder.AppendLine("</script>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            return builder.ToString();
        }

        public static string encryptData(string data, string encoding)
        {
            X509Certificate2 certificate = new X509Certificate2(SDKConfig.publicCertPath);
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key = (RSACryptoServiceProvider) certificate.PublicKey.Key;
            return Convert.ToBase64String(key.Encrypt(Encoding.UTF8.GetBytes(data), false));
        }

        public static string encryptPin(string card, string pwd, string encoding)
        {
            byte[] b = SecurityUtil.pin2PinBlockWithCardNO(pwd, card);
            printHexString(b);
            X509Certificate2 certificate = new X509Certificate2(SDKConfig.publicCertPath);
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key = (RSACryptoServiceProvider) certificate.PublicKey.Key;
            return Convert.ToBase64String(key.Encrypt(b, false));
        }

        public static string PrintDictionaryToString(Dictionary<string, string> data)
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
            return builder.ToString().Substring(0, builder.Length - 1);
        }

        public static string printHexString(byte[] b)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                string str = Convert.ToString((int) (b[i] & 0xff), 0x10);
                if (str.Length == 1)
                {
                    str = '0' + str;
                }
                builder.Append("0x");
                builder.Append(str + " ");
            }
            builder.Append("");
            return builder.ToString();
        }

        public static bool Sign(Dictionary<string, string> data, Encoding encoder)
        {
            data["certId"] = CertUtil.GetSignCertId();
            string dataStr = CoverDictionaryToString(data);
            string str2 = null;
            string s = BitConverter.ToString(SecurityUtil.Sha1X16(dataStr, encoder)).Replace("-", "").ToLower();
            str2 = Convert.ToBase64String(SecurityUtil.SignBySoft(CertUtil.GetSignProviderFromPfx(), encoder.GetBytes(s)));
            data["signature"] = str2;
            return true;
        }

        public static bool Validate(Dictionary<string, string> data, Encoding encoder)
        {
            string s = data["signature"];
            byte[] buffer = Convert.FromBase64String(s);
            data.Remove("signature");
            string str3 = BitConverter.ToString(SecurityUtil.Sha1X16(CoverDictionaryToString(data), encoder)).Replace("-", "").ToLower();
            RSACryptoServiceProvider validateProviderFromPath = CertUtil.GetValidateProviderFromPath(data["certId"]);
            if (null == validateProviderFromPath)
            {
                return false;
            }
            return SecurityUtil.ValidateBySoft(validateProviderFromPath, buffer, encoder.GetBytes(str3));
        }
    }
}

