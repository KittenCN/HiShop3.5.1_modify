namespace Hidistro.UI.Web.API
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Weibo;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.SessionState;

    public class WeiboProcess : IHttpHandler, IRequiresSessionState
    {
        public void AddMenus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"status\":\"1\"}";
            MenuInfo menu = new MenuInfo {
                Content = context.Request["Content"].Trim(),
                Name = context.Request["Name"].Trim()
            };
            if (context.Request["ParentMenuId"] != null)
            {
                menu.ParentMenuId = (context.Request["ParentMenuId"] == "") ? 0 : int.Parse(context.Request["ParentMenuId"]);
            }
            else
            {
                menu.ParentMenuId = 0;
            }
            menu.Type = context.Request["Type"];
            if (WeiboHelper.CanAddMenu(menu.ParentMenuId))
            {
                if (WeiboHelper.SaveMenu(menu))
                {
                    s = "{\"status\":\"0\"}";
                }
            }
            else
            {
                s = "{\"status\":\"2\"}";
            }
            context.Response.Write(s);
        }

        public void addreply(HttpContext context)
        {
            ReplyInfo replyInfo = new ReplyInfo();
            string s = "{\"status\":\"0\"}";
            replyInfo.EditDate = DateTime.Now;
            replyInfo.IsDisable = true;
            replyInfo.Type = int.Parse(context.Request["Type"]);
            replyInfo.ReplyKeyId = int.Parse(context.Request["ReplyKeyId"]);
            replyInfo.ReceiverType = context.Request["ReceiverType"];
            if (replyInfo.ReceiverType == "text")
            {
                replyInfo.Content = context.Request["Content"];
            }
            else
            {
                replyInfo.Url = context.Request["Url"];
                string str2 = "";
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(masterSettings.ShopHomePic))
                {
                    str2 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.ShopHomePic;
                }
                else
                {
                    str2 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[] { '|' })[0];
                }
                replyInfo.Image = str2;
                if ((string.IsNullOrEmpty(context.Request["Image"]) && string.IsNullOrEmpty(context.Request["Display_name"])) && string.IsNullOrEmpty(context.Request["Summary"]))
                {
                    replyInfo.Displayname = masterSettings.SiteName;
                    replyInfo.Summary = string.IsNullOrEmpty(masterSettings.ShopIntroduction) ? masterSettings.SiteName : masterSettings.ShopIntroduction;
                }
                else
                {
                    replyInfo.Image = string.IsNullOrEmpty(context.Request["Image"]) ? str2 : context.Request["Image"];
                    replyInfo.Displayname = context.Request["Display_name"];
                    replyInfo.Summary = context.Request["Summary"];
                    replyInfo.ArticleId = int.Parse(context.Request["ArticleId"]);
                }
            }
            if (WeiboHelper.SaveReplyInfo(replyInfo))
            {
                s = "{\"status\":\"1\"}";
            }
            context.Response.Write(s);
        }

        public void delmenu(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"status\":\"1\"}";
            int result = 0;
            if (!int.TryParse(context.Request["MenuId"], out result))
            {
                s = "{\"status\":\"1\"}";
            }
            else
            {
                if (WeiboHelper.GetMenusByParentId(result).Count > 0)
                {
                    s = "{\"status\":\"2\"}";
                }
                else if (WeiboHelper.DeleteMenu(result))
                {
                    s = "{\"status\":\"0\"}";
                }
                context.Response.Write(s);
            }
        }

        public void EditMenus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{\"status\":\"1\"}";
            MenuInfo menu = new MenuInfo {
                Content = context.Request["Content"],
                Name = context.Request["Name"],
                Type = context.Request["Type"]
            };
            if (!string.IsNullOrEmpty(context.Request["ParentMenuId"]))
            {
                menu.ParentMenuId = int.Parse(context.Request["ParentMenuId"]);
            }
            else
            {
                menu.ParentMenuId = 0;
            }
            int result = 0;
            if (!int.TryParse(context.Request["MenuId"], out result))
            {
                s = "{\"status\":\"1\"}";
            }
            else
            {
                menu.MenuId = result;
                if (WeiboHelper.UpdateMenu(menu))
                {
                    s = "{\"status\":\"0\"}";
                }
                context.Response.Write(s);
            }
        }

        public void editmessage(HttpContext context)
        {
            ReplyInfo replyInfoMes = new ReplyInfo();
            string s = "{\"status\":\"0\"}";
            replyInfoMes = WeiboHelper.GetReplyInfoMes(int.Parse(context.Request["id"].ToString()));
            if (replyInfoMes != null)
            {
                s = "{\"status\":\"1\",";
                object obj2 = s + "\"Content\":\"" + Globals.String2Json(replyInfoMes.Content) + "\",";
                object obj3 = ((((string.Concat(new object[] { obj2, "\"Type\":\"", replyInfoMes.Type, "\"," }) + "\"ReceiverType\":\"" + replyInfoMes.ReceiverType + "\",") + "\"Displayname\":\"" + Globals.String2Json(replyInfoMes.Displayname) + "\",") + "\"Summary\":\"" + Globals.String2Json(replyInfoMes.Summary) + "\",") + "\"Image\":\"" + replyInfoMes.Image + "\",") + "\"Url\":\"" + replyInfoMes.Url + "\",";
                object obj4 = string.Concat(new object[] { obj3, "\"Article\":\"", replyInfoMes.ArticleId, "\"," });
                s = string.Concat(new object[] { obj4, "\"ReplyKeyId\":\"", replyInfoMes.ReplyKeyId, "\"" }) + "}";
            }
            context.Response.Write(s);
        }

        public void editreply(HttpContext context)
        {
            ReplyInfo replyInfo = new ReplyInfo();
            string s = "{\"status\":\"0\"}";
            replyInfo.Id = int.Parse(context.Request["id"]);
            replyInfo.EditDate = DateTime.Now;
            replyInfo.ReceiverType = context.Request["ReceiverType"];
            replyInfo.Type = int.Parse(context.Request["Type"]);
            if (replyInfo.ReceiverType == "text")
            {
                replyInfo.Content = context.Request["Content"];
            }
            else
            {
                replyInfo.Url = context.Request["Url"];
                string str2 = "";
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(masterSettings.ShopHomePic))
                {
                    str2 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.ShopHomePic;
                }
                else
                {
                    str2 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[] { '|' })[0];
                }
                replyInfo.Image = str2;
                if ((string.IsNullOrEmpty(context.Request["Image"]) && string.IsNullOrEmpty(context.Request["Display_name"])) && string.IsNullOrEmpty(context.Request["Summary"]))
                {
                    replyInfo.Displayname = masterSettings.SiteName;
                    replyInfo.Summary = string.IsNullOrEmpty(masterSettings.ShopIntroduction) ? masterSettings.SiteName : masterSettings.ShopIntroduction;
                }
                else
                {
                    replyInfo.Image = string.IsNullOrEmpty(context.Request["Image"]) ? str2 : context.Request["Image"];
                    replyInfo.Displayname = context.Request["Display_name"];
                    replyInfo.Summary = context.Request["Summary"];
                    replyInfo.ArticleId = int.Parse(context.Request["ArticleId"]);
                }
            }
            if (WeiboHelper.UpdateReplyInfo(replyInfo))
            {
                s = "{\"status\":\"1\"}";
            }
            context.Response.Write(s);
        }

        public void GetMenu(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{";
            MenuInfo menu = new MenuInfo();
            int result = 0;
            if (!int.TryParse(context.Request["MenuId"], out result))
            {
                s = "\"status\":\"1\"";
            }
            else
            {
                menu = WeiboHelper.GetMenu(result);
                if (menu != null)
                {
                    object obj2 = s + "\"status\":\"0\",\"data\":[";
                    s = (((string.Concat(new object[] { obj2, "{\"menuid\": \"", menu.MenuId, "\"," }) + "\"type\": \"" + menu.Type + "\",") + "\"name\": \"" + Globals.String2Json(menu.Name) + "\",") + "\"content\": \"" + Globals.String2Json(menu.Content) + "\"}") + "]";
                }
                s = s + "}";
                context.Response.Write(s);
            }
        }

        public void GetMessageInfo(HttpContext context)
        {
            string s = "{\"status\":\"0\"}";
            MessageInfo messageInfo = WeiboHelper.GetMessageInfo(int.Parse(context.Request["MessageId"]));
            if (messageInfo != null)
            {
                s = "{\"status\":\"1\",";
                object obj2 = ((((s + "\"SenderMessage\":\"" + Globals.String2Json(messageInfo.SenderMessage) + "\",") + "\"DisplayName\":\"" + Globals.String2Json(messageInfo.DisplayName) + "\",") + "\"Summary\":\"" + Globals.String2Json(messageInfo.Summary) + "\",") + "\"Image\":\"" + messageInfo.Image + "\",") + "\"Url\":\"" + messageInfo.Url + "\",";
                s = string.Concat(new object[] { obj2, "\"ArticleId\":\"", messageInfo.ArticleId, "\"" }) + "}";
            }
            context.Response.Write(s);
        }

        public void GetTopMenus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string s = "{";
            IList<MenuInfo> topMenus = WeiboHelper.GetTopMenus();
            if (topMenus.Count <= 0)
            {
                s = s + "\"status\":\"-1\"";
            }
            else
            {
                s = s + "\"status\":\"0\",\"data\":[";
                foreach (MenuInfo info in topMenus)
                {
                    IList<MenuInfo> menusByParentId = WeiboHelper.GetMenusByParentId(info.MenuId);
                    object obj2 = s;
                    s = string.Concat(new object[] { obj2, "{\"menuid\": \"", info.MenuId, "\"," });
                    s = s + "\"childdata\":[";
                    if (menusByParentId.Count > 0)
                    {
                        foreach (MenuInfo info2 in menusByParentId)
                        {
                            object obj3 = s;
                            s = string.Concat(new object[] { obj3, "{\"menuid\": \"", info2.MenuId, "\"," });
                            object obj4 = s;
                            s = string.Concat(new object[] { obj4, "\"parentmenuid\": \"", info2.ParentMenuId, "\"," });
                            s = s + "\"type\": \"" + info2.Type + "\",";
                            s = s + "\"name\": \"" + Globals.String2Json(info2.Name) + "\",";
                            s = s + "\"content\": \"" + Globals.String2Json(info2.Content) + "\"},";
                        }
                        s = s.Substring(0, s.Length - 1);
                    }
                    s = s + "],";
                    s = s + "\"type\": \"" + info.Type + "\",";
                    s = s + "\"name\": \"" + Globals.String2Json(info.Name) + "\",";
                    s = s + "\"content\": \"" + Globals.String2Json(info.Content) + "\"},";
                }
                s = s.Substring(0, s.Length - 1) + "]" + "}";
                context.Response.Write(s);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["action"])
            {
                case "gettopmenus":
                    this.GetTopMenus(context);
                    return;

                case "addmenu":
                    this.AddMenus(context);
                    return;

                case "editmenu":
                    this.EditMenus(context);
                    return;

                case "getmenu":
                    this.GetMenu(context);
                    return;

                case "delmenu":
                    this.delmenu(context);
                    return;

                case "savemenu":
                    this.savemenu(context);
                    return;

                case "reply":
                    this.reply(context);
                    return;

                case "replydel":
                    this.replydel(context);
                    return;

                case "editreply":
                    this.editreply(context);
                    return;

                case "addreply":
                    this.addreply(context);
                    return;

                case "editmessage":
                    this.editmessage(context);
                    return;

                case "setenable":
                    this.setenable(context);
                    return;

                case "getmessageinfo":
                    this.GetMessageInfo(context);
                    return;
            }
        }

        public void reply(HttpContext context)
        {
            ReplyKeyInfo replyKeyInfo = new ReplyKeyInfo();
            string s = "{\"status\":\"0\"}";
            replyKeyInfo.Id = int.Parse(context.Request["ID"]);
            replyKeyInfo.Matching = context.Request["Matching"];
            if (WeiboHelper.UpdateMatching(replyKeyInfo))
            {
                s = "{\"status\":\"1\"}";
            }
            context.Response.Write(s);
        }

        public void replydel(HttpContext context)
        {
            string s = "{\"status\":\"0\"}";
            if (WeiboHelper.DeleteReplyInfo(int.Parse(context.Request["id"])))
            {
                s = "{\"status\":\"1\"}";
            }
            context.Response.Write(s);
        }

        public void savemenu(HttpContext context)
        {
            string comment = "{";
            IList<MenuInfo> topMenus = WeiboHelper.GetTopMenus();
            if (topMenus.Count > 0)
            {
                comment = comment + "\"button\":[";
                foreach (MenuInfo info in topMenus)
                {
                    IList<MenuInfo> menusByParentId = WeiboHelper.GetMenusByParentId(info.MenuId);
                    comment = comment + "{\"name\": \"" + Globals.String2Json(info.Name) + "\",";
                    if (menusByParentId.Count > 0)
                    {
                        comment = comment + "\"sub_button\":[";
                        foreach (MenuInfo info2 in menusByParentId)
                        {
                            comment = comment + "{\"type\": \"" + info2.Type + "\",";
                            comment = comment + "\"name\": \"" + Globals.String2Json(info2.Name) + "\",";
                            if (info2.Type == "click")
                            {
                                comment = comment + "\"key\": \"" + Globals.String2Json(info2.Content) + "\"},";
                            }
                            else
                            {
                                comment = comment + "\"url\": \"" + Globals.String2Json(info2.Content) + "\"},";
                            }
                        }
                        comment = comment.Substring(0, comment.Length - 1);
                        comment = comment + "]},";
                    }
                    else
                    {
                        comment = comment + "\"type\": \"" + info.Type + "\",";
                        if (info.Type == "click")
                        {
                            comment = comment + "\"key\": \"" + Globals.String2Json(info.Content) + "\"},";
                        }
                        else
                        {
                            comment = comment + "\"url\": \"" + Globals.String2Json(info.Content) + "\"},";
                        }
                    }
                }
                comment = comment.Substring(0, comment.Length - 1) + "]" + "}";
                comment = new global::ControlPanel.WeiBo.WeiBo().createmenu(comment);
                context.Response.Write(comment);
            }
        }

        public void setenable(HttpContext context)
        {
            string s = "{\"status\":\"1\"}";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (context.Request["type"] == "1")
            {
                masterSettings.CustomReply = bool.Parse(context.Request["enable"]);
            }
            if (context.Request["type"] == "2")
            {
                masterSettings.SubscribeReply = bool.Parse(context.Request["enable"]);
            }
            if (context.Request["type"] == "3")
            {
                masterSettings.ByRemind = bool.Parse(context.Request["enable"]);
            }
            SettingsManager.Save(masterSettings);
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

