namespace Hishop.Plugins.Payment.Alipay_Bank
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;

    public class Submit
    {
        private static string _input_charset = "";
        private static string _key = "";
        private static string _sign_type = "";
        private static string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";

        static Submit()
        {
            _key = Config.Key.Trim();
            _input_charset = Config.Input_charset.Trim().ToLower();
            _sign_type = Config.Sign_type.Trim().ToUpper();
        }

        public static string BuildRequest(SortedDictionary<string, string> sParaTemp)
        {
            Encoding code = Encoding.GetEncoding(_input_charset);
            string s = BuildRequestParaToString(sParaTemp, code);
            byte[] bytes = code.GetBytes(s);
            string requestUriString = GATEWAY_NEW + "_input_charset=" + _input_charset;
            try
            {
                string str3;
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                request.Method = "post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                Stream responseStream = ((HttpWebResponse) request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, code);
                StringBuilder builder = new StringBuilder();
                while ((str3 = reader.ReadLine()) != null)
                {
                    builder.Append(str3);
                }
                responseStream.Close();
                return builder.ToString();
            }
            catch (Exception exception)
            {
                return ("报错：" + exception.Message);
            }
        }

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

        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string fileName, byte[] data, string contentType, int lengthFile)
        {
            Stream responseStream;
            string str;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary = BuildRequestPara(sParaTemp);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(GATEWAY_NEW + "_input_charset=" + _input_charset);
            request.Method = strMethod;
            string str2 = DateTime.Now.Ticks.ToString("x");
            string str3 = "--" + str2;
            request.ContentType = "\r\nmultipart/form-data; boundary=" + str2;
            request.KeepAlive = true;
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                builder.Append(str3 + "\r\nContent-Disposition: form-data; name=\"" + pair.Key + "\"\r\n\r\n" + pair.Value + "\r\n");
            }
            builder.Append(str3 + "\r\nContent-Disposition: form-data; name=\"withhold_file\"; filename=\"");
            builder.Append(fileName);
            builder.Append("\"\r\nContent-Type: " + contentType + "\r\n\r\n");
            string s = builder.ToString();
            Encoding encoding = Encoding.GetEncoding(_input_charset);
            byte[] bytes = encoding.GetBytes(s);
            byte[] buffer = Encoding.ASCII.GetBytes("\r\n" + str3 + "--\r\n");
            long num = (bytes.Length + lengthFile) + buffer.Length;
            request.ContentLength = num;
            Stream requestStream = request.GetRequestStream();
            try
            {
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Write(data, 0, lengthFile);
                requestStream.Write(buffer, 0, buffer.Length);
                responseStream = ((HttpWebResponse) request.GetResponse()).GetResponseStream();
            }
            catch (WebException exception)
            {
                return exception.ToString();
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
            }
            StreamReader reader = new StreamReader(responseStream, encoding);
            StringBuilder builder2 = new StringBuilder();
            while ((str = reader.ReadLine()) != null)
            {
                builder2.Append(str);
            }
            responseStream.Close();
            return builder2.ToString();
        }

        private static string BuildRequestMysign(Dictionary<string, string> sPara)
        {
            string str;
            string prestr = Core.CreateLinkString(sPara);
            if (((str = _sign_type) != null) && (str == "MD5"))
            {
                return AlipayMD5.Sign(prestr, _key, _input_charset);
            }
            return "";
        }

        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            string str = "";
            sPara = Core.FilterPara(sParaTemp);
            str = BuildRequestMysign(sPara);
            sPara.Add("sign", str);
            sPara.Add("sign_type", _sign_type);
            return sPara;
        }

        private static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            return Core.CreateLinkStringUrlencode(BuildRequestPara(sParaTemp), code);
        }

        public static string Query_timestamp()
        {
            XmlTextReader reader = new XmlTextReader(GATEWAY_NEW + "service=query_timestamp&partner=" + Config.Partner + "&_input_charset=" + Config.Input_charset);
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            return document.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;
        }
    }
}

