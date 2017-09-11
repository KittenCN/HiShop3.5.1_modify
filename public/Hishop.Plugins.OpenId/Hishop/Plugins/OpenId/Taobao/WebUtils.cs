namespace Hishop.Plugins.OpenId.Taobao
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    public sealed class WebUtils
    {
        private int _timeout = 0x186a0;

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
                    builder.Append(Uri.EscapeDataString(str2));
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
            HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return this.GetResponseAsString(rsp, encoding);
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
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return this.GetResponseAsString(rsp, encoding);
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
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.ServicePoint.Expect100Continue = false;
            request.Method = method;
            request.KeepAlive = true;
            request.UserAgent = "Top4Net";
            request.Timeout = this._timeout;
            return request;
        }

        public int Timeout
        {
            get
            {
                return this._timeout;
            }
            set
            {
                this._timeout = value;
            }
        }
    }
}

