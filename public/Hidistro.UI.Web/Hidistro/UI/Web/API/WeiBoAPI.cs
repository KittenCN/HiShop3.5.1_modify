namespace Hidistro.UI.Web.API
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Entities.Weibo;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;

    public class WeiBoAPI : IHttpHandler, IRequiresSessionState
    {
        public void commentscreate(HttpContext context)
        {
            string s = new global::ControlPanel.WeiBo.WeiBo().commentscreate(context.Request["id"], context.Request["comment"]);
            context.Response.Write(s);
        }

        public void createmenu(HttpContext context)
        {
            global::ControlPanel.WeiBo.WeiBo bo = new global::ControlPanel.WeiBo.WeiBo();
            string s = "";
            if (!string.IsNullOrEmpty(context.Request["comment"]))
            {
                s = bo.createmenu(context.Request["comment"]);
            }
            context.Response.Write(s);
        }

        public void deletemenu(HttpContext context)
        {
            global::ControlPanel.WeiBo.WeiBo bo = new global::ControlPanel.WeiBo.WeiBo();
            string s = "";
            s = bo.deletemenu();
            context.Response.Write(s);
        }

        public void friends_timeline(HttpContext context)
        {
            string s = new global::ControlPanel.WeiBo.WeiBo().friends_timeline(int.Parse(context.Request["page"]));
            context.Response.Write(s);
        }

        public void getfriends(HttpContext context)
        {
            string s = new global::ControlPanel.WeiBo.WeiBo().getfriends();
            context.Response.Write(s);
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["action"])
            {
                case "friends_timeline":
                    this.friends_timeline(context);
                    return;

                case "userinfo":
                    this.userinfo(context);
                    return;

                case "statusesupdate":
                    this.statusesupdate(context);
                    return;

                case "usertimeline":
                    this.user_timeline(context);
                    return;

                case "getfriends":
                    this.getfriends(context);
                    return;

                case "sendtouidmessage":
                    this.SendToUIDMessage(context);
                    return;

                case "commentscreate":
                    this.commentscreate(context);
                    return;

                case "repost":
                    this.repost(context);
                    return;

                case "createmenu":
                    this.createmenu(context);
                    return;

                case "showemenu":
                    this.showemenu(context);
                    return;

                case "deletemenu":
                    this.deletemenu(context);
                    return;

                case "sendmessage":
                    this.sendmessage(context);
                    return;
            }
        }

        public void repost(HttpContext context)
        {
            string s = new global::ControlPanel.WeiBo.WeiBo().repost(context.Request["id"], context.Request["comment"]);
            context.Response.Write(s);
        }

        public void sendmessage(HttpContext context)
        {
            global::ControlPanel.WeiBo.WeiBo bo = new global::ControlPanel.WeiBo.WeiBo();
            string type = context.Request["msgtype"];
            string str2 = context.Request["SenderId"];
            string s = context.Request["MessageId"];
            string msgtype = context.Request["msgtype"];
            string displayname = context.Request["displayname"];
            string summary = context.Request["summary"];
            string image = context.Request["image"];
            string url = context.Request["url"];
            string content = context.Request["Content"];
            string articleId = context.Request["ArticleId"];
            string data = this.SendToUIDMessage(msgtype, displayname, summary, image, url, content, articleId);
            string json = bo.sendmessage(type, str2, data);
            JObject obj2 = JObject.Parse(json);
            if ((obj2["result"] != null) && (obj2["result"].ToString() == "true"))
            {
                MessageInfo messageInfo = new MessageInfo();
                if (!string.IsNullOrEmpty(articleId))
                {
                    messageInfo.ArticleId = int.Parse(articleId);
                }
                messageInfo.DisplayName = displayname;
                messageInfo.Summary = summary;
                messageInfo.SenderMessage = content;
                messageInfo.Url = url;
                messageInfo.Image = image;
                messageInfo.SenderDate = DateTime.Now;
                messageInfo.MessageId = int.Parse(s);
                messageInfo.Status = 1;
                WeiboHelper.UpdateMessage(messageInfo);
            }
            context.Response.Write(json);
        }

        public void SendToUIDMessage(HttpContext context)
        {
            global::ControlPanel.WeiBo.WeiBo bo = new global::ControlPanel.WeiBo.WeiBo();
            string msgtype = context.Request["msgtype"];
            string displayname = context.Request["displayname"];
            string summary = context.Request["summary"];
            string image = context.Request["image"];
            string url = context.Request["url"];
            string content = context.Request["Content"];
            string articleId = context.Request["ArticleId"];
            string s = bo.SendToUIDMessage(msgtype, displayname, summary, image, url, content, articleId);
            context.Response.Write(s);
        }

        public string SendToUIDMessage(string msgtype, string displayname, string summary, string image, string url, string Content, string ArticleId)
        {
            string str = Content;
            string str2 = "{\"text\": \"" + str + "\"}";
            if (!(msgtype == "articles"))
            {
                return str2;
            }
            str2 = "";
            if (summary == "")
            {
                summary = displayname;
            }
            string str3 = "{\"display_name\": \"" + displayname + "\",\"summary\":\"" + summary + "\",\"image\":\"" + image + "\",\"url\":\"" + url + "\"},";
            IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(ArticleId));
            if (articleItems.Count > 0)
            {
                string title = "";
                foreach (ArticleItemsInfo info in articleItems)
                {
                    if (info.Content.Trim() == "")
                    {
                        title = info.Title;
                    }
                    string str5 = str3;
                    str3 = str5 + "{\"display_name\": \"" + info.Title + "\",\"summary\":\"" + title + "\",\"image\":\"http://" + Globals.DomainName + info.ImageUrl + "\",\"url\":\"" + info.Url + "\"},";
                }
            }
            str3 = str3.Substring(0, str3.Length - 1);
            return ("{\"articles\": [" + str3 + "]}");
        }

        public void showemenu(HttpContext context)
        {
            global::ControlPanel.WeiBo.WeiBo bo = new global::ControlPanel.WeiBo.WeiBo();
            string s = "";
            s = bo.showemenu();
            context.Response.Write(s);
        }

        public void statusesupdate(HttpContext context)
        {
            global::ControlPanel.WeiBo.WeiBo bo = new global::ControlPanel.WeiBo.WeiBo();
            string s = "";
            if (!string.IsNullOrEmpty(context.Request["status"]))
            {
                s = bo.statusesupdate(context.Request["status"].ToString().Replace("alert", "Alert"), context.Request["img"]);
            }
            context.Response.Write(s);
        }

        public void user_timeline(HttpContext context)
        {
            string s = new global::ControlPanel.WeiBo.WeiBo().user_timeline(int.Parse(context.Request["page"]));
            context.Response.Write(s);
        }

        public void userinfo(HttpContext context)
        {
            string s = new global::ControlPanel.WeiBo.WeiBo().userinfo();
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

