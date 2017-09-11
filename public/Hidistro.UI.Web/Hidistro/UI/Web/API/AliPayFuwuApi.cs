namespace Hidistro.UI.Web.API
{
    using  global:: ControlPanel.WeiBo;
    using  global:: ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.Weibo;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.AlipayFuwu.Api.Model;
    using Hishop.AlipayFuwu.Api.Util;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class AliPayFuwuApi : IHttpHandler
    {
        private string appid;
        private SiteSettings siteSettings;
        private string UserInfo;

        private Articles GetAlipayArticlesFromArticleInfo(ArticleInfo articleInfo, string storeUrl, string FromUserid)
        {
            Articles articles = null;
            if (articleInfo != null)
            {
                articles = new Articles {
                    toUserId = FromUserid,
                    msgType = "image-text",
                    articles = new List<article>()
                };
                string imageUrl = articleInfo.ImageUrl;
                if (!imageUrl.ToLower().StartsWith("http"))
                {
                    imageUrl = storeUrl + imageUrl;
                }
                string str2 = Globals.StripAllTags(articleInfo.Content);
                if (str2.Length > 30)
                {
                    str2 = str2.Substring(0, 30);
                }
                article item = new article {
                    actionName = "立即查看",
                    title = articleInfo.Title,
                    desc = str2,
                    imageUrl = imageUrl,
                    url = articleInfo.Url
                };
                articles.articles.Add(item);
                if ((articleInfo.ItemsInfo == null) || (articleInfo.ItemsInfo.Count <= 0))
                {
                    return articles;
                }
                foreach (ArticleItemsInfo info in articleInfo.ItemsInfo)
                {
                    string str3 = info.ImageUrl;
                    if (!str3.ToLower().StartsWith("http"))
                    {
                        str3 = storeUrl + str3;
                    }
                    string str4 = Globals.StripAllTags(info.Content);
                    if (str4.Length > 30)
                    {
                        str4 = str4.Substring(0, 30);
                    }
                    article article2 = new article {
                        actionName = "立即查看",
                        title = info.Title,
                        desc = str4,
                        imageUrl = str3,
                        url = info.Url
                    };
                    articles.articles.Add(article2);
                }
            }
            return articles;
        }

        public void ProcessRequest(HttpContext context)
        {
            Articles articles2;
            this.siteSettings = SettingsManager.GetMasterSettings(true);
            this.appid = this.siteSettings.AlipayAppid;
            AliOHHelper.log(context.Request.Form.ToString());
            if ((AlipayFuwuConfig.appId.Length < 15) && !AlipayFuwuConfig.CommSetConfig(this.appid, context.Server.MapPath("~/"), "GBK"))
            {
                context.Response.Write(AlipayFuwuConfig.errstr);
                return;
            }
            if ("alipay.service.check".Equals(AliOHHelper.getRequestString("service", context)))
            {
                AliOHHelper.verifygw(context);
                return;
            }
            if (!"alipay.mobile.public.message.notify".Equals(AliOHHelper.getRequestString("service", context)))
            {
                return;
            }
            string xml = AliOHHelper.getRequestString("biz_content", context);
            string eventType = AliOHHelper.getXmlNode(xml, "EventType");
            string str3 = AliOHHelper.getXmlNode(xml, "FromUserId");
            this.UserInfo = AliOHHelper.getXmlNode(xml, "UserInfo");
            string actionParam = AliOHHelper.getXmlNode(xml, "ActionParam");
            AliOHHelper.getXmlNode(xml, "AgreementId");
            AliOHHelper.getXmlNode(xml, "AccountNo");
            AliOHHelper.getXmlNode(xml, "AppId");
            AliOHHelper.getXmlNode(xml, "CreateTime");
            string str5 = AliOHHelper.getXmlNode(xml, "MsgType");
            string textContent = AliOHHelper.getXmlNode(xml, "Content");
            string str7 = str5;
            if (str7 != null)
            {
                if (!(str7 == "event"))
                {
                    if (str7 == "text")
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(str3))
                            {
                                WeiXinHelper.UpdateRencentAliOpenID(str3);
                            }
                        }
                        catch (Exception exception2)
                        {
                            AliOHHelper.log(exception2.Message.ToString());
                        }
                        try
                        {
                            this.replyAction(str3, "", textContent, actionParam);
                            goto Label_022F;
                        }
                        catch (Exception exception3)
                        {
                            AliOHHelper.log(exception3.Message.ToString());
                            goto Label_022F;
                        }
                        goto Label_01E3;
                    }
                    if (str7 == "image")
                    {
                        goto Label_01E3;
                    }
                }
                else
                {
                    try
                    {
                        this.replyAction(str3, eventType, textContent, actionParam);
                    }
                    catch (Exception exception)
                    {
                        AliOHHelper.log(exception.Message.ToString());
                    }
                }
            }
            goto Label_022F;
        Label_01E3:
            articles2 = new Articles();
            articles2.toUserId = str3;
            articles2.msgType = "text";
            MessageText text = new MessageText {
                content = "服务窗暂时不接收图片消息"
            };
            articles2.text = text;
            Articles articles = articles2;
            AliOHHelper.log(AliOHHelper.CustomSend(articles).Body);
        Label_022F:
            AliOHHelper.verifyRequestFromAliPay(context, str3, this.appid);
        }

        private void replyAction(string FromUserId, string eventType, string textContent, string ActionParam)
        {
            Articles articles6 = new Articles {
                toUserId = FromUserId,
                msgType = "text"
            };
            MessageText text5 = new MessageText {
                content = "系统未找到相关信息！"
            };
            articles6.text = text5;
            Articles articles = articles6;
            if (eventType != "")
            {
                if ("follow".Equals(eventType))
                {
                    if (!string.IsNullOrEmpty(FromUserId))
                    {
                        MemberProcessor.AddFuwuFollowUser(FromUserId);
                    }
                    string aliOHFollowRelayTitle = this.siteSettings.AliOHFollowRelayTitle;
                    Hidistro.Entities.VShop.ReplyInfo subscribeReply = AliFuwuReplyHelper.GetSubscribeReply();
                    if (subscribeReply != null)
                    {
                        if (subscribeReply.MessageType == Hidistro.Entities.VShop.MessageType.Text)
                        {
                            TextReplyInfo info2 = subscribeReply as TextReplyInfo;
                            articles.text.content = info2.Text;
                        }
                        else if (subscribeReply.ArticleID > 0)
                        {
                            ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(subscribeReply.ArticleID);
                            if (articleInfo != null)
                            {
                                articles = this.GetAlipayArticlesFromArticleInfo(articleInfo, Globals.HostPath(HttpContext.Current.Request.Url), FromUserId);
                            }
                            else
                            {
                                articles.text.content = aliOHFollowRelayTitle;
                            }
                        }
                    }
                    else
                    {
                        articles.text.content = aliOHFollowRelayTitle;
                    }
                }
                else if ("unfollow".Equals(eventType))
                {
                    if (!string.IsNullOrEmpty(FromUserId))
                    {
                        MemberProcessor.DelFuwuFollowUser(FromUserId);
                    }
                }
                else if ("click".Equals(eventType))
                {
                    int result = 0;
                    if (((ActionParam != "") && int.TryParse(ActionParam, out result)) && (result > 0))
                    {
                        Hidistro.Entities.VShop.MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(result);
                        if (fuwuMenu != null)
                        {
                            Hidistro.Entities.VShop.ReplyInfo reply = AliFuwuReplyHelper.GetReply(fuwuMenu.ReplyId);
                            if (reply != null)
                            {
                                ArticleInfo info6 = ArticleHelper.GetArticleInfo(reply.ArticleID);
                                if (info6 != null)
                                {
                                    articles = this.GetAlipayArticlesFromArticleInfo(info6, Globals.HostPath(HttpContext.Current.Request.Url), FromUserId);
                                }
                            }
                        }
                    }
                }
                else if ("enter".Equals(eventType))
                {
                    if (!string.IsNullOrEmpty(this.UserInfo))
                    {
                        MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(FromUserId, "fuwu");
                        if ((openIdMember != null) && openIdMember.AlipayLoginId.StartsWith("FW*"))
                        {
                            JObject obj2 = JsonConvert.DeserializeObject(this.UserInfo) as JObject;
                            string str2 = "";
                            string str3 = "";
                            if (obj2["logon_id"] != null)
                            {
                                str2 = obj2["logon_id"].ToString();
                            }
                            if (obj2["user_name"] != null)
                            {
                                str3 = obj2["user_name"].ToString();
                            }
                            if ((str3 != "") && (str3 != ""))
                            {
                                openIdMember.AlipayLoginId = str2;
                                openIdMember.AlipayUsername = str3;
                                MemberProcessor.SetAlipayInfos(openIdMember);
                            }
                        }
                    }
                    if (!ActionParam.Contains("sceneId"))
                    {
                        return;
                    }
                    JObject obj3 = JsonConvert.DeserializeObject(ActionParam) as JObject;
                    if (obj3["scene"]["sceneId"] != null)
                    {
                        string key = obj3["scene"]["sceneId"].ToString();
                        if (key.StartsWith("bind"))
                        {
                            if (AlipayFuwuConfig.BindAdmin.Count > 10)
                            {
                                AlipayFuwuConfig.BindAdmin.Clear();
                            }
                            if (AlipayFuwuConfig.BindAdmin.ContainsKey(key))
                            {
                                AlipayFuwuConfig.BindAdmin[key] = FromUserId;
                            }
                            else
                            {
                                AlipayFuwuConfig.BindAdmin.Add(key, FromUserId);
                            }
                            articles.text.content = "您正在尝试绑定服务窗管理员身份！";
                        }
                    }
                }
            }
            else if (textContent != "")
            {
                articles = null;
                IList<Hidistro.Entities.VShop.ReplyInfo> replies = AliFuwuReplyHelper.GetReplies(ReplyType.Keys);
                if ((replies != null) && (replies.Count > 0))
                {
                    foreach (Hidistro.Entities.VShop.ReplyInfo info8 in replies)
                    {
                        if (info8 != null)
                        {
                            if ((info8.MatchType == MatchType.Equal) && (info8.Keys == textContent))
                            {
                                if (info8.MessageType == Hidistro.Entities.VShop.MessageType.Text)
                                {
                                    Articles articles2 = new Articles {
                                        toUserId = FromUserId,
                                        msgType = "text"
                                    };
                                    MessageText text = new MessageText {
                                        content = ""
                                    };
                                    articles2.text = text;
                                    articles = articles2;
                                    TextReplyInfo info9 = info8 as TextReplyInfo;
                                    articles.text.content = info9.Text;
                                    break;
                                }
                                if (info8.ArticleID > 0)
                                {
                                    ArticleInfo info10 = ArticleHelper.GetArticleInfo(info8.ArticleID);
                                    if (info10 != null)
                                    {
                                        articles = this.GetAlipayArticlesFromArticleInfo(info10, Globals.HostPath(HttpContext.Current.Request.Url), FromUserId);
                                        if (articles != null)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            if ((info8.MatchType == MatchType.Like) && info8.Keys.Contains(textContent))
                            {
                                if (info8.MessageType == Hidistro.Entities.VShop.MessageType.Text)
                                {
                                    Articles articles3 = new Articles {
                                        toUserId = FromUserId,
                                        msgType = "text"
                                    };
                                    MessageText text2 = new MessageText {
                                        content = ""
                                    };
                                    articles3.text = text2;
                                    articles = articles3;
                                    TextReplyInfo info11 = info8 as TextReplyInfo;
                                    articles.text.content = info11.Text;
                                    break;
                                }
                                if (info8.ArticleID > 0)
                                {
                                    ArticleInfo info12 = ArticleHelper.GetArticleInfo(info8.ArticleID);
                                    if (info12 != null)
                                    {
                                        articles = this.GetAlipayArticlesFromArticleInfo(info12, Globals.HostPath(HttpContext.Current.Request.Url), FromUserId);
                                        if (articles != null)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (articles == null)
            {
                IList<Hidistro.Entities.VShop.ReplyInfo> list2 = AliFuwuReplyHelper.GetReplies(ReplyType.NoMatch);
                if ((list2 != null) && (list2.Count > 0))
                {
                    foreach (Hidistro.Entities.VShop.ReplyInfo info13 in list2)
                    {
                        if (info13.MessageType == Hidistro.Entities.VShop.MessageType.Text)
                        {
                            Articles articles4 = new Articles {
                                toUserId = FromUserId,
                                msgType = "text"
                            };
                            MessageText text3 = new MessageText {
                                content = ""
                            };
                            articles4.text = text3;
                            articles = articles4;
                            TextReplyInfo info14 = info13 as TextReplyInfo;
                            articles.text.content = info14.Text;
                            break;
                        }
                        if (info13.ArticleID > 0)
                        {
                            ArticleInfo info15 = ArticleHelper.GetArticleInfo(info13.ArticleID);
                            if (info15 != null)
                            {
                                articles = this.GetAlipayArticlesFromArticleInfo(info15, Globals.HostPath(HttpContext.Current.Request.Url), FromUserId);
                                if (articles != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Articles articles5 = new Articles {
                        toUserId = FromUserId,
                        msgType = "text"
                    };
                    MessageText text4 = new MessageText {
                        content = "系统未找到相关信息！"
                    };
                    articles5.text = text4;
                    articles = articles5;
                }
            }
            AliOHHelper.log(AliOHHelper.CustomSend(articles).Body);
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

