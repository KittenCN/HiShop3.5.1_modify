namespace Hishop.AlipayFuwu.Api.Util
{
    using Aop.Api;
    using Aop.Api.Request;
    using Aop.Api.Response;
    using Aop.Api.Util;
    using Hishop.AlipayFuwu.Api.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class AliOHHelper
    {
        private static object LockLog = new object();

        public static string AlipayAuthUrl(string returnUrl, string app_id, string scope = "auth_userinfo")
        {
            return ("https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?app_id=" + app_id + "&auth_skip=false&scope=auth_userinfo&redirect_uri=" + HttpUtility.UrlEncode(returnUrl));
        }

        public static AlipayMobilePublicMessageCustomSendResponse CustomSend(Articles Articles)
        {
            AlipayMobilePublicMessageCustomSendRequest request = new AlipayMobilePublicMessageCustomSendRequest {
                BizContent = SerializeObject(Articles, true)
            };
            IAopClient client = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
            return client.Execute<AlipayMobilePublicMessageCustomSendResponse>(request);
        }

        public static Dictionary<string, string> getAlipayRequstParams(HttpContext context)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("service", getRequestString("service", context));
            dictionary.Add("sign_type", getRequestString("sign_type", context));
            dictionary.Add("charset", getRequestString("charset", context));
            dictionary.Add("biz_content", getRequestString("biz_content", context));
            dictionary.Add("sign", getRequestString("sign", context));
            return dictionary;
        }

        public static AlipayUserUserinfoShareResponse GetAlipayUserUserinfo(string AccessToken)
        {
            AlipayOHClient client = new AlipayOHClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.alipay_public_key, AlipayFuwuConfig.merchant_private_key, AlipayFuwuConfig.merchant_public_key, "UTF-8");
            return client.GetAliUserInfo(AccessToken);
        }

        public static List<string> GetAllfollowList()
        {
            List<string> list = new List<string>();
            int result = 0x2710;
            string nextUserId = "";
            AlipayMobilePublicFollowListResponse followList = null;
            while (result == 0x2710)
            {
                followList = GetfollowList(nextUserId);
                if (((followList != null) && !followList.IsError) && ((followList.Data != null) && int.TryParse(followList.Count, out result)))
                {
                    if (followList.Data.UserIdList != null)
                    {
                        nextUserId = followList.Data.UserIdList[0];
                        foreach (string str2 in followList.Data.UserIdList)
                        {
                            list.Add(str2);
                        }
                    }
                }
                else
                {
                    result = 0;
                }
            }
            return list;
        }

        public static AlipayMobilePublicFollowListResponse GetfollowList(string nextUserId)
        {
            AlipayMobilePublicFollowListRequest request = new AlipayMobilePublicFollowListRequest {
                BizContent = "{\"nextUserId\": \"" + nextUserId + "\"}"
            };
            IAopClient client = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
            return client.Execute<AlipayMobilePublicFollowListResponse>(request);
        }

        public static AlipaySystemOauthTokenResponse GetOauthTokenResponse(string auth_code)
        {
            AlipayOHClient client = new AlipayOHClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.alipay_public_key, AlipayFuwuConfig.merchant_private_key, AlipayFuwuConfig.merchant_public_key, "UTF-8");
            return client.OauthTokenRequest(auth_code);
        }

        public static string getRequestString(string key, HttpContext context)
        {
            string str = null;
            if ((context.Request.Form.Get(key) != null) && (context.Request.Form.Get(key).ToString() != ""))
            {
                return context.Request.Form.Get(key).ToString();
            }
            if ((context.Request.QueryString[key] != null) && (context.Request.QueryString[key].ToString() != ""))
            {
                str = context.Request.QueryString[key].ToString();
            }
            return str;
        }

        public static Dictionary<string, string> getRequstParam(HttpContext context)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (context.Request.QueryString != null)
            {
                string[] allKeys = context.Request.QueryString.AllKeys;
                for (int i = 0; i < allKeys.Length; i++)
                {
                    string text1 = allKeys[i];
                }
            }
            if (context.Request.Form != null)
            {
                for (int j = 0; j < context.Request.Params.Count; j++)
                {
                    dictionary.Add(context.Request.Params.Keys[j].ToString(), context.Request.Params[j].ToString());
                }
            }
            return dictionary;
        }

        public static string GetUrlParam(Dictionary<string, string> param)
        {
            string str = "";
            if (param == null)
            {
                return str;
            }
            foreach (string str2 in param.Keys)
            {
                string str3 = str;
                str = str3 + str2 + "=" + param[str2] + "&";
            }
            return str.Substring(0, str.LastIndexOf('&'));
        }

        public static string getXmlNode(string xml, string node)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            string str = "";
            try
            {
                str = document.SelectSingleNode("//" + node).InnerText.ToString();
            }
            catch (Exception)
            {
            }
            return str;
        }

        public static void log(string log)
        {
            if (AlipayFuwuConfig.writeLog)
            {
                lock (LockLog)
                {
                    StreamWriter writer = File.AppendText(HttpRuntime.AppDomainAppPath.ToString() + "App_Data/" + (DateTime.Now.ToString("yyyyMMdd") + "_Fuwulog.txt"));
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + log);
                    writer.WriteLine("---------------");
                    writer.Close();
                }
            }
        }

        public static AlipayMobilePublicMenuAddResponse MenuAdd(FWMenu menu)
        {
            IAopClient client = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
            AlipayMobilePublicMenuAddRequest request = new AlipayMobilePublicMenuAddRequest {
                BizContent = SerializeObject(menu, true)
            };
            return client.Execute<AlipayMobilePublicMenuAddResponse>(request);
        }

        public static AlipayMobilePublicMenuGetResponse MenuGet()
        {
            IAopClient client = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
            AlipayMobilePublicMenuGetRequest request = new AlipayMobilePublicMenuGetRequest();
            return client.Execute<AlipayMobilePublicMenuGetResponse>(request);
        }

        public static AlipayMobilePublicMenuUpdateResponse MenuUpdate(FWMenu menu)
        {
            IAopClient client = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
            AlipayMobilePublicMenuUpdateRequest request = new AlipayMobilePublicMenuUpdateRequest {
                BizContent = SerializeObject(menu, true)
            };
            return client.Execute<AlipayMobilePublicMenuUpdateResponse>(request);
        }

        public static AlipayMobilePublicQrcodeCreateResponse QrcodeSend(QrcodeInfo codeInfo)
        {
            AlipayMobilePublicQrcodeCreateRequest request = new AlipayMobilePublicQrcodeCreateRequest {
                BizContent = SerializeObject(codeInfo, true)
            };
            IAopClient client = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
            return client.Execute<AlipayMobilePublicQrcodeCreateResponse>(request);
        }

        public static string ReturnXmlResponse(bool _success, string merchantPubKey, HttpContext context, string ToUserId, string AppId)
        {
            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.GetEncoding(AlipayFuwuConfig.charset);
            context.Response.Clear();
            XmlDocument document = new XmlDocument();
            XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", AlipayFuwuConfig.charset, null);
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("alipay");
            document.AppendChild(element);
            XmlNode node = document.SelectSingleNode("alipay");
            XmlElement element2 = document.CreateElement("response");
            XmlElement element3 = document.CreateElement("success");
            XmlElement element4 = document.CreateElement("XML");
            if (_success)
            {
                XmlElement element5 = document.CreateElement("MsgType");
                element5.InnerText = "ack";
                XmlElement element6 = document.CreateElement("CreateTime");
                element6.InnerText = TransferToMilStartWith1970(DateTime.Now).ToString("F0");
                XmlElement element7 = document.CreateElement("ToUserId");
                element7.InnerText = ToUserId;
                XmlElement element8 = document.CreateElement("AppId");
                element8.InnerText = AppId;
                element4.AppendChild(element5);
                element4.AppendChild(element6);
                element4.AppendChild(element7);
                element4.AppendChild(element8);
                element2.AppendChild(element4);
            }
            else
            {
                element3.InnerText = "false";
                element2.AppendChild(element3);
                XmlElement element9 = document.CreateElement("error_code");
                element9.InnerText = "VERIFY_FAILED";
                element2.AppendChild(element9);
            }
            node.AppendChild(element2);
            string str = AlipaySignature.RSASign(element2.InnerXml, AlipayFuwuConfig.merchant_private_key, AlipayFuwuConfig.charset);
            XmlElement element10 = document.CreateElement("sign");
            element10.InnerText = str;
            node.AppendChild(element10);
            XmlElement element11 = document.CreateElement("sign_type");
            element11.InnerText = "RSA";
            node.AppendChild(element11);
            log(document.InnerXml);
            context.Response.Output.Write(document.InnerXml);
            context.Response.End();
            return null;
        }

        public static string SerializeObject(object Articles, bool IgnoreNull = true)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            };
            if (IgnoreNull)
            {
                return JsonConvert.SerializeObject(Articles, settings);
            }
            return JsonConvert.SerializeObject(Articles);
        }

        public static AlipayMobilePublicMessageSingleSendResponse TemplateSend(AliTemplateMessage templateMessage)
        {
            AlipayMobilePublicMessageSingleSendRequest request = new AlipayMobilePublicMessageSingleSendRequest {
                BizContent = templateSendMessage(templateMessage, "查看详情")
            };
            IAopClient client = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
            return client.Execute<AlipayMobilePublicMessageSingleSendResponse>(request);
        }

        public static string templateSendMessage(AliTemplateMessage templateMessage, string actionName = "查看详情")
        {
            StringBuilder builder = new StringBuilder("{");
            builder.AppendFormat("\"toUserId\":\"{0}\",", templateMessage.Touser);
            builder.AppendFormat("\"template\":{{ \"templateId\":\"{0}\",", templateMessage.TemplateId.ToString());
            builder.AppendFormat("\"context\":{{ \"headColor\":\"{0}\",", templateMessage.Topcolor);
            builder.AppendFormat("\"actionName\":\"{0}\",", actionName);
            if (!string.IsNullOrEmpty(templateMessage.Url))
            {
                builder.AppendFormat("\"url\":\"{0}\",", templateMessage.Url);
            }
            foreach (AliTemplateMessage.MessagePart part in templateMessage.Data)
            {
                builder.AppendFormat("\"{0}\":{{\"value\":\"{1}\",\"color\":\"{2}\"}},", part.Name, part.Value, part.Color);
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("}}}");
            return builder.ToString();
        }

        public static AlipayMobilePublicMessageTotalSendResponse TotalSend(Articles Articles)
        {
            AlipayMobilePublicMessageTotalSendRequest request = new AlipayMobilePublicMessageTotalSendRequest {
                BizContent = SerializeObject(Articles, true)
            };
            IAopClient client = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
            return client.Execute<AlipayMobilePublicMessageTotalSendResponse>(request);
        }

        public static double TransferToMilStartWith1970(DateTime dateTime)
        {
            DateTime time = new DateTime(0x7b2, 1, 1);
            TimeSpan span = (TimeSpan) (dateTime - time);
            return span.TotalMilliseconds;
        }

        public static void verifygw(HttpContext context)
        {
            Dictionary<string, string> param = getAlipayRequstParams(context);
            string xml = param["biz_content"];
            if (!verifySignAlipayRequest(param))
            {
                verifygwResponse(false, RsaKeyHelper.GetRSAKeyContent(AlipayFuwuConfig.merchant_public_key, true), context);
            }
            if ("verifygw".Equals(getXmlNode(xml, "EventType")))
            {
                verifygwResponse(true, RsaKeyHelper.GetRSAKeyContent(AlipayFuwuConfig.merchant_public_key, true), context);
            }
        }

        public static string verifygwResponse(bool _success, string merchantPubKey, HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.GetEncoding(AlipayFuwuConfig.charset);
            context.Response.Clear();
            XmlDocument document = new XmlDocument();
            XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", AlipayFuwuConfig.charset, null);
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("alipay");
            document.AppendChild(element);
            XmlNode node = document.SelectSingleNode("alipay");
            XmlElement element2 = document.CreateElement("response");
            XmlElement element3 = document.CreateElement("success");
            if (_success)
            {
                element3.InnerText = "true";
                element2.AppendChild(element3);
            }
            else
            {
                element3.InnerText = "false";
                element2.AppendChild(element3);
                XmlElement element4 = document.CreateElement("error_code");
                element4.InnerText = "VERIFY_FAILED";
                element2.AppendChild(element4);
            }
            XmlElement element5 = document.CreateElement("biz_content");
            element5.InnerText = merchantPubKey;
            element2.AppendChild(element5);
            node.AppendChild(element2);
            string str = AlipaySignature.RSASign(element2.InnerXml, AlipayFuwuConfig.merchant_private_key, AlipayFuwuConfig.charset);
            XmlElement element6 = document.CreateElement("sign");
            element6.InnerText = str;
            node.AppendChild(element6);
            XmlElement element7 = document.CreateElement("sign_type");
            element7.InnerText = "RSA";
            node.AppendChild(element7);
            context.Response.Output.Write(document.InnerXml);
            context.Response.End();
            return null;
        }

        public static void verifyRequestFromAliPay(HttpContext context, string ToUserId, string AppId)
        {
            Dictionary<string, string> param = getAlipayRequstParams(context);
            string local1 = param["biz_content"];
            if (!verifySignAlipayRequest(param))
            {
                ReturnXmlResponse(false, RsaKeyHelper.GetRSAKeyContent(AlipayFuwuConfig.merchant_public_key, true), context, ToUserId, AppId);
            }
            else
            {
                ReturnXmlResponse(true, RsaKeyHelper.GetRSAKeyContent(AlipayFuwuConfig.merchant_public_key, true), context, ToUserId, AppId);
            }
        }

        public static bool verifySignAlipayRequest(Dictionary<string, string> param)
        {
            return AlipaySignature.RSACheckV2(param, AlipayFuwuConfig.alipay_public_key, AlipayFuwuConfig.charset);
        }

        private class EventType
        {
            public const string Click = "click";
            public const string Enter = "enter";
            public const string Follow = "follow";
            public const string UnFollow = "unfollow";
            public const string Verifygw = "verifygw";
        }
    }
}

