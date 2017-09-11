namespace Hishop.Weixin.Pay
{
    using Hishop.Weixin.Pay.Domain;
    using Hishop.Weixin.Pay.Util;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;

    public class RedPackClient
    {
        private static object LockLog = new object();
        public static readonly string QueryRedPackUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/gethbinfo";
        public static readonly string SendRedPack_Url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";

        private static object CheckType(object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }
            return Convert.ChangeType(value, conversionType);
        }

        public static T ConvertDic<T>(Dictionary<string, object> dic)
        {
            T local = Activator.CreateInstance<T>();
            PropertyInfo[] properties = local.GetType().GetProperties();
            if ((properties.Length > 0) && (dic.Count > 0))
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    if (dic.ContainsKey(properties[i].Name))
                    {
                        if (properties[i].PropertyType.IsEnum)
                        {
                            object obj2 = System.Enum.ToObject(properties[i].PropertyType, dic[properties[i].Name]);
                            properties[i].SetValue(local, obj2, null);
                        }
                        else
                        {
                            properties[i].SetValue(local, CheckType(dic[properties[i].Name], properties[i].PropertyType), null);
                        }
                    }
                }
            }
            return local;
        }

        public string CreatRedpackId(string mch_id)
        {
            return (mch_id + DateTime.Now.ToString("yyyymmdd") + DateTime.Now.ToString("MMddHHmmss"));
        }

        public static void Debuglog(string log, string logname = "_DebugRedPacklog.txt")
        {
            lock (LockLog)
            {
                try
                {
                    StreamWriter writer = System.IO.File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "App_Data/" + (DateTime.Now.ToString("yyyyMMdd") + logname));
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                    writer.WriteLine("---------------");
                    writer.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        public static Dictionary<string, object> FromXml(string xml)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            foreach (XmlNode node2 in document.FirstChild.ChildNodes)
            {
                XmlElement element = (XmlElement) node2;
                dictionary[element.Name] = element.InnerText;
            }
            return dictionary;
        }

        public RedPackInfo GetRedpackInfo(string appId, string mch_id, string mch_billno, string partnerkey, string weixincertpath, string weixincertpassword)
        {
            PayDictionary parameters = new PayDictionary();
            parameters.Add("nonce_str", Utils.CreateNoncestr());
            parameters.Add("mch_billno", mch_billno);
            parameters.Add("mch_id", mch_id);
            parameters.Add("appid", appId);
            parameters.Add("bill_type", "MCHT");
            string str = SignHelper.SignPackage(parameters, partnerkey);
            parameters.Add("sign", str);
            string data = SignHelper.BuildXml(parameters, false);
            string message = "";
            try
            {
                message = Send(weixincertpath, weixincertpassword, data, QueryRedPackUrl);
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            if (!string.IsNullOrEmpty(message) && message.Contains("return_code"))
            {
                return ConvertDic<RedPackInfo>(FromXml(message));
            }
            return new RedPackInfo { return_code = "FAIL", return_msg = message, status = "" };
        }

        public static string PostData(string url, string postData)
        {
            string xml = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = postData.Length;
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(postData);
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        xml = new StreamReader(stream, encoding).ReadToEnd();
                        XmlDocument document = new XmlDocument();
                        try
                        {
                            document.LoadXml(xml);
                        }
                        catch (Exception exception)
                        {
                            xml = string.Format("获取信息错误doc.load：{0}", exception.Message) + xml;
                        }
                        try
                        {
                            if (document == null)
                            {
                                return xml;
                            }
                            XmlNode node = document.SelectSingleNode("xml/return_code");
                            if (node == null)
                            {
                                return xml;
                            }
                            if (node.InnerText == "SUCCESS")
                            {
                                XmlNode node2 = document.SelectSingleNode("xml/prepay_id");
                                if (node2 == null)
                                {
                                    return xml;
                                }
                                return node2.InnerText;
                            }
                            return document.InnerXml;
                        }
                        catch (Exception exception2)
                        {
                            xml = string.Format("获取信息错误node.load：{0}", exception2.Message) + xml;
                        }
                        return xml;
                    }
                }
            }
            catch (Exception exception3)
            {
                xml = string.Format("获取信息错误post error：{0}", exception3.Message) + xml;
            }
            return xml;
        }

        public static string Send(string cert, string password, string data, string url)
        {
            return Send(cert, password, Encoding.GetEncoding("UTF-8").GetBytes(data), url);
        }

        public static string Send(string cert, string password, byte[] data, string url)
        {
            X509Certificate2 certificate;
            Stream responseStream;
            ServicePointManager.ServerCertificateValidationCallback = (s, ch, er, c) => true;
            try
            {
                byte[] buffer;
                using (FileStream stream = new FileStream(cert, FileMode.Open, FileAccess.Read))
                {
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Close();
                }
                certificate = new X509Certificate2(buffer, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            request.UserAgent = "Hishop";
            request.ContentType = "text/xml";
            request.ClientCertificates.Add(certificate);
            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception2)
            {
                throw exception2;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            return str;
        }

        public string SendRedpack(SendRedPackInfo sendredpack)
        {
            PayDictionary parameters = new PayDictionary();
            parameters.Add("nonce_str", Utils.CreateNoncestr());
            if (sendredpack.EnableSP)
            {
                if (!string.IsNullOrEmpty(sendredpack.SendRedpackRecordID))
                {
                    parameters.Add("mch_billno", sendredpack.SendRedpackRecordID);
                }
                else
                {
                    parameters.Add("mch_billno", this.CreatRedpackId(sendredpack.Main_Mch_ID));
                }
                parameters.Add("mch_id", sendredpack.Main_Mch_ID);
                parameters.Add("sub_mch_id", sendredpack.Sub_Mch_Id);
                parameters.Add("wxappid", sendredpack.Main_AppId);
                parameters.Add("msgappid", sendredpack.Main_AppId);
            }
            else
            {
                if (!string.IsNullOrEmpty(sendredpack.SendRedpackRecordID))
                {
                    parameters.Add("mch_billno", sendredpack.SendRedpackRecordID);
                }
                else
                {
                    parameters.Add("mch_billno", this.CreatRedpackId(sendredpack.Mch_Id));
                }
                parameters.Add("mch_id", sendredpack.Mch_Id);
                parameters.Add("wxappid", sendredpack.WXAppid);
                parameters.Add("nick_name", sendredpack.Nick_Name);
                parameters.Add("min_value", sendredpack.Total_Amount);
                parameters.Add("max_value", sendredpack.Total_Amount);
            }
            parameters.Add("send_name", sendredpack.Send_Name);
            parameters.Add("re_openid", sendredpack.Re_Openid);
            parameters.Add("total_amount", sendredpack.Total_Amount);
            parameters.Add("total_num", sendredpack.Total_Num);
            parameters.Add("wishing", sendredpack.Wishing);
            parameters.Add("client_ip", sendredpack.Client_IP);
            parameters.Add("act_name", sendredpack.Act_Name);
            parameters.Add("remark", sendredpack.Remark);
            string str2 = SignHelper.SignPackage(parameters, sendredpack.PartnerKey);
            parameters.Add("sign", str2);
            string log = SignHelper.BuildXml(parameters, false);
            Debuglog(log, "_DebugRedPacklog.txt");
            string str4 = Send(sendredpack.WeixinCertPath, sendredpack.WeixinCertPassword, log, SendRedPack_Url);
            Debuglog(str4, "_DebugRedPacklog.txt");
            if ((!string.IsNullOrEmpty(str4) && str4.Contains("SUCCESS")) && !str4.Contains("<![CDATA[FAIL]]></result_code>"))
            {
                return "success";
            }
            Match match = new Regex(@"<return_msg><!\[CDATA\[(?<code>(.*))\]\]></return_msg>").Match(str4);
            if (match.Success)
            {
                return match.Groups["code"].Value;
            }
            return str4;
        }

        public string SendRedpack(string appId, string mch_id, string sub_mch_id, string nick_name, string send_name, string re_openid, string wishing, string client_ip, string act_name, string remark, int amount, string partnerkey, string weixincertpath, string weixincertpassword, string mch_billno, bool enablesp, string main_appId, string main_mch_id, string main_paykey)
        {
            SendRedPackInfo sendredpack = new SendRedPackInfo {
                WXAppid = appId,
                Mch_Id = mch_id,
                Sub_Mch_Id = mch_id,
                Main_AppId = main_appId,
                Main_Mch_ID = main_mch_id,
                Main_PayKey = main_paykey,
                EnableSP = enablesp,
                Nick_Name = nick_name,
                Send_Name = send_name,
                Re_Openid = re_openid,
                Wishing = wishing,
                Client_IP = client_ip,
                Act_Name = act_name,
                Remark = remark,
                Total_Amount = amount,
                PartnerKey = partnerkey,
                WeixinCertPath = weixincertpath,
                WeixinCertPassword = weixincertpassword,
                SendRedpackRecordID = mch_billno
            };
            return this.SendRedpack(sendredpack);
        }

        public static void writeLog(IDictionary<string, string> param, string sign, string url, string msg)
        {
        }
    }
}

