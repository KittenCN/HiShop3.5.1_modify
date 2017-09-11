namespace Hishop.Weixin.MP.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;

    public sealed class WebUtils
    {
        public string BuildGetUrl(string url, IDictionary<string, string> parameters)
        {
            if ((parameters != null) && (parameters.Count > 0))
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                    return url;
                }
                url = url + "?" + BuildQuery(parameters);
            }
            return url;
        }

        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
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
                    builder.Append(HttpUtility.UrlEncode(str2, Encoding.UTF8));
                    flag = true;
                }
            }
            return builder.ToString();
        }

        public string DoGet(string url, IDictionary<string, string> parameters)
        {
            if ((parameters != null) && (parameters.Count > 0))
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }
            return this.HttpSend(url, null);
        }

        public string DoPost(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(BuildQuery(parameters));
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
            return this.GetResponseAsString(rsp, Encoding.UTF8);
        }

        public string DoPost(string url, string value)
        {
            return this.HttpSend(url, value);
        }

        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream responseStream = null;
            StreamReader reader = null;
            string str;
            try
            {
                responseStream = rsp.GetResponseStream();
                reader = new StreamReader(responseStream, encoding);
                str = reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return str;
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest request = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = (s, ce, ch, er) => true;
                request = (HttpWebRequest) WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                request = (HttpWebRequest) WebRequest.Create(url);
            }
            request.ServicePoint.Expect100Continue = false;
            request.Method = method;
            request.KeepAlive = true;
            request.Timeout = 0x4e20;
            request.UserAgent = "Hishop";
            return request;
        }

        public string HttpSend(string url, string value)
        {
            try
            {
                string method = "GET";
                if (!string.IsNullOrEmpty(value))
                {
                    method = "POST";
                }
                HttpWebRequest webRequest = this.GetWebRequest(url, method);
                webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(value);
                    Stream requestStream = webRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
                return this.GetResponseAsString(rsp, Encoding.UTF8);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
    }
}

