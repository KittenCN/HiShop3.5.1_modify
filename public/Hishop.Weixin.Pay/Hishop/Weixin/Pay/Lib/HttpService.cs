namespace Hishop.Weixin.Pay.Lib
{
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Web;

    public class HttpService
    {
        private static object LockLog = new object();

        public static string Get(string url, string PROXY_URL = "")
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("PROXY_URL", PROXY_URL);
            GC.Collect();
            string str = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                ServicePointManager.DefaultConnectionLimit = 200;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = (s, ce, ch, e) => true;
                }
                request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "GET";
                if (!string.IsNullOrEmpty(PROXY_URL))
                {
                    WebProxy proxy = new WebProxy {
                        Address = new Uri(PROXY_URL)
                    };
                    request.Proxy = proxy;
                }
                response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                str = reader.ReadToEnd().Trim();
                reader.Close();
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (WebException exception)
            {
                dictionary.Add("HttpService", exception.ToString());
                if (exception.Status == WebExceptionStatus.ProtocolError)
                {
                    dictionary.Add("HttpService", "StatusCode : " + ((HttpWebResponse) exception.Response).StatusCode);
                    dictionary.Add("HttpService", "StatusDescription : " + ((HttpWebResponse) exception.Response).StatusDescription);
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return str;
        }

        public static string Post(string xml, string url, bool isUseCert, PayConfig config, int timeout)
        {
            GC.Collect();
            string str = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream requestStream = null;
            try
            {
                ServicePointManager.DefaultConnectionLimit = 200;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = (s, ce, ch, e) => true;
                }
                request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = timeout * 0x3e8;
                request.ContentType = "text/xml";
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                request.ContentLength = bytes.Length;
                if (isUseCert)
                {
                    byte[] buffer2;
                    using (FileStream stream2 = new FileStream(config.SSLCERT_PATH, FileMode.Open, FileAccess.Read))
                    {
                        buffer2 = new byte[stream2.Length];
                        stream2.Read(buffer2, 0, buffer2.Length);
                        stream2.Close();
                    }
                    X509Certificate2 certificate = new X509Certificate2(buffer2, config.SSLCERT_PASSWORD, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
                    request.ClientCertificates.Add(certificate);
                }
                requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                str = reader.ReadToEnd().Trim();
                reader.Close();
            }
            catch (Exception exception)
            {
                WxDebuglog(exception.Message, "_wxpay.txt");
                return ("POSTERROR:" + exception.Message);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return str;
        }

        public static void WxDebuglog(string log, string logname = "_wxpay.txt")
        {
            lock (LockLog)
            {
                try
                {
                    StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "App_Data/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + log);
                    writer.WriteLine("---------------");
                    writer.Close();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

