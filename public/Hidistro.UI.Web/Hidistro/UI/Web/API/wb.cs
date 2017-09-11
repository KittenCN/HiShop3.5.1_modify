namespace Hidistro.UI.Web.API
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Weibo;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class wb : IHttpHandler
    {
        private string app_secret = "";

        private static string articleMsg(string display_name, string summary, string image, string url, string ArticleId, int MessageId)
        {
            new JObject();
            string str = "[";
            if (summary == "")
            {
                summary = display_name;
            }
            string str3 = str;
            str = str3 + "{\"display_name\":\"" + Globals.String2Json(display_name) + "\",\"summary\":\"" + Globals.String2Json(summary) + "\",\"image\":\"" + image + "\",\"url\":\"" + url + "\"},";
            if (!string.IsNullOrEmpty(ArticleId) && (int.Parse(ArticleId) > 0))
            {
                IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(ArticleId));
                if (articleItems.Count > 0)
                {
                    foreach (ArticleItemsInfo info in articleItems)
                    {
                        string s = "";
                        if (info.Content.Trim() == "")
                        {
                            s = info.Title;
                        }
                        string str4 = str;
                        str = str4 + "{\"display_name\": \"" + Globals.String2Json(info.Title) + "\",\"summary\":\"" + Globals.String2Json(s) + "\",\"image\":\"http://" + Globals.DomainName + info.ImageUrl + "\",\"url\":\"" + info.Url + "\"},";
                    }
                }
            }
            MessageInfo messageInfo = new MessageInfo {
                SenderDate = DateTime.Now,
                DisplayName = display_name,
                Summary = summary,
                Image = image,
                Url = url,
                ArticleId = int.Parse(ArticleId),
                Status = 2,
                MessageId = MessageId
            };
            WeiboHelper.UpdateMessage(messageInfo);
            str = str.Substring(0, str.Length - 1) + "]";
            return ("{\"articles\":" + str.ToString() + "}");
        }

        private static string generateReplyMsg(string data, string type, string senderId, string receiverId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"result\":true,\"sender_id\":" + senderId + ",\"receiver_id\":" + receiverId + ",\"type\":\"" + type + "\",\"data\":\"" + HttpContext.Current.Server.UrlEncode(data) + "\"");
            builder.Append("}");
            return builder.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            this.app_secret = SettingsManager.GetMasterSettings(false).App_Secret;
            string signature = request["signature"];
            string nonce = request["nonce"];
            string timestamp = request["timestamp"];
            string s = request["echostr"];
            if (request.HttpMethod == "GET")
            {
                if (this.ValidateSHA(signature, nonce, timestamp))
                {
                    context.Response.Write(s);
                }
                else
                {
                    context.Response.Write("");
                }
                context.Response.End();
            }
            else
            {
                try
                {
                    StringBuilder builder = new StringBuilder();
                    string str5 = "";
                    byte[] buffer = new byte[request.InputStream.Length];
                    request.InputStream.Read(buffer, 0, buffer.Length);
                    string str6 = Encoding.UTF8.GetString(buffer);
                    str6 = HttpContext.Current.Server.UrlDecode(str6);
                    builder.Append(str6);
                    string senderId = "";
                    string receiverId = "";
                    int messageId = 0;
                    JObject obj2 = JObject.Parse(str6);
                    if ((((obj2["type"] != null) && (obj2["type"].ToString() == MessageType.text.ToString())) || ((obj2["type"] != null) && (obj2["type"].ToString() == MessageType.image.ToString()))) || ((obj2["type"] != null) && (obj2["type"].ToString() == MessageType.voice.ToString())))
                    {
                        MessageInfo messageInfo = new MessageInfo {
                            Created_at = DateTime.Now.ToString(),
                            Receiver_id = obj2["receiver_id"].ToString(),
                            Sender_id = obj2["sender_id"].ToString(),
                            Text = obj2["text"].ToString(),
                            Type = obj2["type"].ToString(),
                            Status = 0
                        };
                        if (obj2["data"] != null)
                        {
                            JObject obj3 = JObject.Parse(obj2["data"].ToString());
                            if (obj3["tovfid"] != null)
                            {
                                messageInfo.Tovfid = obj3["tovfid"].ToString();
                            }
                            if (obj3["vfid"] != null)
                            {
                                messageInfo.Vfid = obj3["vfid"].ToString();
                            }
                        }
                        messageInfo.Access_Token = SettingsManager.GetMasterSettings(false).Access_Token;
                        messageId = WeiboHelper.SaveMessage(messageInfo);
                    }
                    if (((obj2["type"] != null) && (obj2["type"].ToString() == MessageType.text.ToString())) && SettingsManager.GetMasterSettings(false).CustomReply)
                    {
                        senderId = obj2["receiver_id"].ToString();
                        receiverId = obj2["sender_id"].ToString();
                        DataView defaultView = WeiboHelper.GetReplyAll(1).DefaultView;
                        if (defaultView.Count > 0)
                        {
                            defaultView.RowFilter = "Keys='" + obj2["text"].ToString() + "'";
                            if (defaultView.Count > 0)
                            {
                                int num2 = new Random().Next(0, defaultView.Count);
                                if (defaultView[num2]["ReceiverType"].ToString() == "text")
                                {
                                    str5 = generateReplyMsg(textMsg(defaultView[num2]["Content"].ToString(), messageId), "text", senderId, receiverId);
                                }
                                else
                                {
                                    str5 = generateReplyMsg(articleMsg(defaultView[num2]["Display_name"].ToString(), defaultView[num2]["Summary"].ToString(), defaultView[num2]["Image"].ToString(), defaultView[num2]["Url"].ToString(), defaultView[num2]["ArticleId"].ToString(), messageId), "articles", senderId, receiverId);
                                }
                            }
                            else
                            {
                                defaultView.RowFilter = "Keys like '%" + obj2["text"].ToString() + "%'";
                                if (defaultView.Count > 0)
                                {
                                    int num3 = new Random().Next(0, defaultView.Count);
                                    if (defaultView[num3]["ReceiverType"].ToString() == "text")
                                    {
                                        str5 = generateReplyMsg(textMsg(defaultView[num3]["Content"].ToString(), messageId), "text", senderId, receiverId);
                                    }
                                    else
                                    {
                                        str5 = generateReplyMsg(articleMsg(defaultView[num3]["Display_name"].ToString(), defaultView[num3]["Summary"].ToString(), defaultView[num3]["Image"].ToString(), defaultView[num3]["Url"].ToString(), defaultView[num3]["ArticleId"].ToString(), messageId), "articles", senderId, receiverId);
                                    }
                                }
                            }
                        }
                    }
                    if ((obj2["type"] != null) && (obj2["type"].ToString() == "event"))
                    {
                        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                        JObject obj4 = JObject.Parse(obj2["data"].ToString());
                        if (((obj4["subtype"].ToString().Trim() != "click") && masterSettings.SubscribeReply) && (obj4["subtype"].ToString().Trim() == "subscribe"))
                        {
                            senderId = obj2["receiver_id"].ToString();
                            receiverId = obj2["sender_id"].ToString();
                            DataView view2 = WeiboHelper.GetWeibo_Reply(2).DefaultView;
                            if (view2.Count > 0)
                            {
                                int num4 = new Random().Next(0, view2.Count);
                                if (view2[num4]["ReceiverType"].ToString() == "text")
                                {
                                    str5 = generateReplyMsg(textMsg(view2[num4]["Content"].ToString(), messageId), "text", senderId, receiverId);
                                }
                                else
                                {
                                    str5 = generateReplyMsg(articleMsg(view2[num4]["Display_name"].ToString(), view2[num4]["Summary"].ToString(), view2[num4]["Image"].ToString(), view2[num4]["Url"].ToString(), view2[num4]["ArticleId"].ToString(), messageId), "articles", senderId, receiverId);
                                }
                            }
                        }
                    }
                    if (((obj2["type"] != null) && (obj2["type"].ToString() == "mention")) && SettingsManager.GetMasterSettings(false).ByRemind)
                    {
                        JObject.Parse(obj2["data"].ToString());
                        senderId = obj2["receiver_id"].ToString();
                        receiverId = obj2["sender_id"].ToString();
                        DataView view3 = WeiboHelper.GetWeibo_Reply(3).DefaultView;
                        if (view3.Count > 0)
                        {
                            int num5 = new Random().Next(0, view3.Count);
                            if (view3[num5]["ReceiverType"].ToString() == "text")
                            {
                                str5 = generateReplyMsg(textMsg(view3[num5]["Content"].ToString(), messageId), "text", senderId, receiverId);
                            }
                            else
                            {
                                str5 = generateReplyMsg(articleMsg(view3[num5]["Display_name"].ToString(), view3[num5]["Summary"].ToString(), view3[num5]["Image"].ToString(), view3[num5]["Url"].ToString(), view3[num5]["ArticleId"].ToString(), messageId), "articles", senderId, receiverId);
                            }
                        }
                    }
                    context.Response.Write(str5);
                }
                catch (Exception exception)
                {
                    Globals.Debuglog(exception.Message, "_DebugLogWB.txt");
                }
            }
        }

        private static string textMsg(string text, int MessageId)
        {
            JObject obj2 = new JObject();
            obj2.Add("text", text);
            MessageInfo messageInfo = new MessageInfo {
                SenderDate = DateTime.Now,
                SenderMessage = text,
                Status = 2,
                MessageId = MessageId
            };
            WeiboHelper.UpdateMessage(messageInfo);
            return obj2.ToString();
        }

        private bool ValidateSHA(string signature, string nonce, string timestamp)
        {
            if (((signature == null) || (nonce == null)) || (timestamp == null))
            {
                return false;
            }
            byte[] bytes = Encoding.Default.GetBytes(timestamp + nonce + this.app_secret);
            string str = BitConverter.ToString(SHA1.Create().ComputeHash(bytes)).Replace("-", "");
            if (!signature.ToUpper().Equals(str))
            {
                return false;
            }
            return true;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public enum MessageType
        {
            text,
            position,
            voice,
            image,
            mesevent,
            mention
        }
    }
}

