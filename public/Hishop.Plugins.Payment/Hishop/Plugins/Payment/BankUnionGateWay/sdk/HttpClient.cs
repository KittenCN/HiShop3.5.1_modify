namespace Hishop.Plugins.Payment.BankUnionGateWay.sdk
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;

    public class HttpClient
    {
        private string requestUrl = "";
        private string result;

        public HttpClient(string url)
        {
            this.requestUrl = url;
        }

        private static string BuildRequestParaToString(Dictionary<string, string> sParaTemp, Encoding code)
        {
            return CreateLinkstringUrlencode(sParaTemp, code);
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static string CreateLinkstringUrlencode(Dictionary<string, string> dicArray, Encoding code)
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

        public int Send(Dictionary<string, string> sParaTemp, Encoding encoder)
        {
            string s = BuildRequestParaToString(sParaTemp, encoder);
            byte[] bytes = encoder.GetBytes(s);
            HttpWebResponse response = null;
            try
            {
                string str2;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(this.requestUrl);
                request.Method = "post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse) request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, encoder);
                StringBuilder builder = new StringBuilder();
                while ((str2 = reader.ReadLine()) != null)
                {
                    builder.Append(str2);
                }
                responseStream.Close();
                this.result = builder.ToString();
            }
            catch (Exception exception)
            {
                this.result = "报错：" + exception.Message;
            }
            return (int) response.StatusCode;
        }

        public string Result
        {
            get
            {
                return this.result;
            }
            set
            {
                this.result = value;
            }
        }
    }
}

