namespace Hidistro.UI.Web.Admin.AliFuwu
{
    using Aop.Api.Response;
    using  global:: ControlPanel.WeiBo;
    using  global:: ControlPanel.WeiXin;
    using global::ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.Weibo;
    using Hidistro.Entities.WeiXin;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using Hishop.AlipayFuwu.Api.Model;
    using Hishop.AlipayFuwu.Api.Util;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SendAllEdit : AdminPage
    {
        protected HtmlForm aspnetForm;
        private string errcode;
        protected ucUeditor fkContent;
        protected HiddenField hdfArticleID;
        protected HiddenField hdfIsOldArticle;
        protected HiddenField hdfMessageType;
        protected HiddenField hdfSendID;
        protected string htmlInfo;
        protected Literal litInfo;
        protected int LocalArticleID;
        protected int sendID;
        protected TextBox txtTitle;
        protected string type;

        protected SendAllEdit() : base("m11", "fwp03")
        {
            this.errcode = string.Empty;
            this.htmlInfo = string.Empty;
            this.sendID = Globals.RequestQueryNum("ID");
            this.LocalArticleID = Globals.RequestQueryNum("aid");
            this.type = Globals.RequestQueryStr("type");
        }

        private string FormatSendContent(string content)
        {
            string input = content;
            return Regex.Replace(Regex.Replace(input, "</?([^>^a^p]*)>", ""), "<img([^>]*)>", "").Replace("<p>", "").Replace("</p>", "\r").Trim(new char[] { '\r' }).Replace("\r", "\r\n");
        }

        private string FormatUrl(string url)
        {
            string str = url;
            if (url.StartsWith("/"))
            {
                str = "http://" + Globals.DomainName + url;
            }
            return str;
        }

        private Articles GetAlipayArticlesFromArticleInfo(ArticleInfo articleInfo, string storeUrl)
        {
            Articles articles = null;
            if (articleInfo != null)
            {
                articles = new Articles {
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

        private string GetKFSendImageJson(string openid, ArticleInfo articleinfo)
        {
            StringBuilder builder = new StringBuilder();
            if ((articleinfo != null) && (articleinfo != null))
            {
                switch (articleinfo.ArticleType)
                {
                    case ArticleType.News:
                        builder.Append("{\"title\":\"" + this.String2Json(articleinfo.Title) + "\",\"description\":\"" + this.String2Json(articleinfo.Memo) + "\",\"url\":\"" + this.String2Json(this.FormatUrl(articleinfo.Url)) + "\",\"picurl\":\"" + this.String2Json(this.FormatUrl(articleinfo.ImageUrl)) + "\"}");
                        break;

                    case ArticleType.List:
                        builder.Append("{\"title\":\"" + this.String2Json(articleinfo.Title) + "\",\"description\":\"" + this.String2Json(articleinfo.Memo) + "\",\"url\":\"" + this.String2Json(this.FormatUrl(articleinfo.Url)) + "\",\"picurl\":\"" + this.String2Json(this.FormatUrl(articleinfo.ImageUrl)) + "\"}");
                        foreach (ArticleItemsInfo info in articleinfo.ItemsInfo)
                        {
                            builder.Append(",{\"title\":\"" + this.String2Json(info.Title) + "\",\"description\":\"\",\"url\":\"" + this.String2Json(this.FormatUrl(info.Url)) + "\",\"picurl\":\"" + this.String2Json(this.FormatUrl(info.ImageUrl)) + "\"}");
                        }
                        break;
                }
            }
            return ("{\"touser\":\"" + openid + "\",\"msgtype\":\"news\",\"news\":{\"articles\": [" + builder.ToString() + "]}}");
        }

        [PrivilegeCheck(Privilege.Summary)]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
                return;
            if (this.type == "getarticleinfo")
            {
                this.Response.ContentType = "application/json";
                string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
                int articleid = Globals.RequestFormNum("articleid");
                if (articleid > 0)
                {
                    ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(articleid);
                    if (articleInfo != null)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        switch (articleInfo.ArticleType)
                        {
                            case ArticleType.News:
                                s = "{\"type\":\"1\",\"articletype\":" + (object)articleInfo.ArticleType + ",\"title\":\"" + this.String2Json(articleInfo.Title) + "\",\"date\":\"" + this.String2Json(articleInfo.PubTime.ToString("M月d日")) + "\",\"imgurl\":\"" + this.String2Json(articleInfo.ImageUrl) + "\",\"memo\":\"" + this.String2Json(articleInfo.Memo) + "\"}";
                                break;
                            case ArticleType.List:
                                foreach (ArticleItemsInfo articleItemsInfo in (IEnumerable<ArticleItemsInfo>)articleInfo.ItemsInfo)
                                    stringBuilder.Append("{\"title\":\"" + this.String2Json(articleItemsInfo.Title) + "\",\"imgurl\":\"" + this.String2Json(articleItemsInfo.ImageUrl) + "\"},");
                                s = "{\"type\":\"1\",\"articletype\":" + (object)articleInfo.ArticleType + ",\"title\":\"" + this.String2Json(articleInfo.Title) + "\",\"date\":\"" + this.String2Json(articleInfo.PubTime.ToString("M月d日")) + "\",\"imgurl\":\"" + this.String2Json(articleInfo.ImageUrl) + "\",\"items\":[" + ((object)stringBuilder).ToString().Trim(',') + "]}";
                                break;
                            default:
                                s = "{\"type\":\"1\",\"articletype\":" + (object)articleInfo.ArticleType + ",\"title\":\"" + this.String2Json(articleInfo.Title) + "\",\"date\":\"" + this.String2Json(articleInfo.PubTime.ToString("M月d日")) + "\",\"imgurl\":\"" + this.String2Json(articleInfo.ImageUrl) + "\",\"memo\":\"" + this.String2Json(articleInfo.Content) + "\"}";
                                break;
                        }
                    }
                }
                this.Response.Write(s);
                this.Response.End();
            }
            else if (this.type == "postdata")
            {
                this.Response.ContentType = "application/json";
                this.sendID = Globals.RequestFormNum("sendid");
                int num = Globals.RequestFormNum("sendtype");
                int msgType = Globals.RequestFormNum("msgtype");
                int articleid = Globals.RequestFormNum("articleid");
                string title = Globals.RequestFormStr("title");
                string content = Globals.RequestFormStr("content");
                int isoldarticle = Globals.RequestFormNum("isoldarticle");
                string str1 = this.SavePostData(msgType, articleid, title, content, isoldarticle, this.sendID, true);
                string s;
                if (string.IsNullOrEmpty(str1))
                {
                    MessageType messageType = (MessageType)msgType;
                    string str2 = string.Empty;
                    Articles Articles = new Articles();
                    Articles.msgType = "text";
                    string storeUrl = Globals.HostPath(HttpContext.Current.Request.Url);
                    if (messageType == MessageType.List || messageType == MessageType.News)
                    {
                        this.sendID = Globals.ToNum((object)this.SavePostData(msgType, articleid, title, content, isoldarticle, this.sendID, false));
                        ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(articleid);
                        if (articleInfo == null)
                        {
                            this.Response.Write("{\"type\":\"0\",\"tips\":\"素材不存在了\"}");
                            this.Response.End();
                        }
                        Articles = this.GetAlipayArticlesFromArticleInfo(articleInfo, storeUrl);
                    }
                    else
                    {
                        this.sendID = Globals.ToNum((object)this.SavePostData(msgType, articleid, title, content, isoldarticle, this.sendID, false));
                        Articles.text = new MessageText()
                        {
                            content = Globals.StripHtmlXmlTags(content)
                        };
                    }
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                    if (AlipayFuwuConfig.appId.Length < 15)
                        AlipayFuwuConfig.CommSetConfig(masterSettings.AlipayAppid, this.Server.MapPath("~/"), "GBK");
                    if (num == 1)
                    {
                        AlipayMobilePublicMessageTotalSendResponse totalSendResponse = AliOHHelper.TotalSend(Articles);
                        if (!totalSendResponse.IsError && totalSendResponse.Code == "200")
                        {
                            s = "{\"type\":\"1\",\"tips\":\"服务窗群发成功，请于一天后到服务窗后台查询送达结果！\"}";
                            string msgid = "";
                            if (!string.IsNullOrEmpty(totalSendResponse.Data) && totalSendResponse.Data.Length > 50)
                                msgid = totalSendResponse.Data.Substring(0, 49);
                            int alypayUserNum = WeiXinHelper.getAlypayUserNum();
                            WeiXinHelper.UpdateMsgId(this.sendID, msgid, 1, alypayUserNum, alypayUserNum, "");
                        }
                        else
                        {
                            s = "{\"type\":\"0\",\"tips\":\"" + totalSendResponse.Msg + "\"}";
                            WeiXinHelper.UpdateMsgId(this.sendID, "", 2, 0, 0, totalSendResponse.Body);
                        }
                    }
                    else
                    {
                        List<string> sendList = new List<string>();
                        DataTable rencentAliOpenId = WeiXinHelper.GetRencentAliOpenID();
                        if (rencentAliOpenId != null)
                        {
                            for (int index = 0; index < rencentAliOpenId.Rows.Count; ++index)
                                sendList.Add(rencentAliOpenId.Rows[index][0].ToString());
                        }
                        if (sendList.Count > 0)
                        {
                            WeiXinHelper.UpdateMsgId(this.sendID, "", 0, 0, sendList.Count, "");
                            new Thread((ThreadStart)(() =>
                            {
                                try
                                {
                                    bool flag = false;
                                    foreach (string str in sendList)
                                    {
                                        if (str.Length > 16)
                                        {
                                            Articles.toUserId = str;
                                            AlipayMobilePublicMessageCustomSendResponse customSendResponse = AliOHHelper.CustomSend(Articles);
                                            if (customSendResponse != null && customSendResponse.IsError)
                                            {
                                                AliOHHelper.log(customSendResponse.Body);
                                            }
                                            else
                                            {
                                                flag = true;
                                                WeiXinHelper.UpdateAddSendCount(this.sendID, 1, -1);
                                            }
                                            Thread.Sleep(10);
                                        }
                                    }
                                    if (flag)
                                        WeiXinHelper.UpdateAddSendCount(this.sendID, 0, 1);
                                    else
                                        WeiXinHelper.UpdateAddSendCount(this.sendID, 0, 2);
                                    Thread.Sleep(10);
                                }
                                catch (Exception ex)
                                {
                                    AliOHHelper.log(((object)ex.Message).ToString());
                                }
                            })).Start();
                            s = "{\"type\":\"1\",\"tips\":\"信息正在后台推送中，请稍后刷新群发列表查看结果\"}";
                        }
                        else
                            s = "{\"type\":\"0\",\"tips\":\"暂时没有关注的用户可以发送信息\"}";
                    }
                }
                else
                    s = "{\"type\":\"0\",\"tips\":\"" + str1 + "\"}";
                this.Response.Write(s);
                this.Response.End();
            }
            else
            {
                if (this.sendID > 0)
                {
                    this.hdfSendID.Value = this.sendID.ToString();
                    SendAllInfo sendAllInfo = WeiXinHelper.GetSendAllInfo(this.sendID);
                    if (sendAllInfo != null)
                    {
                        MessageType messageType = sendAllInfo.MessageType;
                        this.hdfMessageType.Value = ((int)sendAllInfo.MessageType).ToString();
                        int articleId = sendAllInfo.ArticleID;
                        this.hdfArticleID.Value = articleId.ToString();
                        this.txtTitle.Text = sendAllInfo.Title;
                        switch (messageType)
                        {
                            case MessageType.Text:
                                this.fkContent.Text = sendAllInfo.Content;
                                break;
                            case MessageType.News:
                                if (articleId <= 0)
                                {
                                    this.hdfIsOldArticle.Value = "1";
                                    NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(this.sendID) as NewsReplyInfo;
                                    if (newsReplyInfo != null && newsReplyInfo.NewsMsg != null && newsReplyInfo.NewsMsg.Count != 0)
                                    {
                                        this.htmlInfo = "<div class=\"mate-inner\"><h3 id=\"singelTitle\">" + newsReplyInfo.NewsMsg[0].Title + "</h3><span>" + newsReplyInfo.LastEditDate.ToString("M月d日") + "</span><div class=\"mate-img\"><img id=\"img1\" src=\"" + newsReplyInfo.NewsMsg[0].PicUrl + "\" class=\"img-responsive\"></div><div class=\"mate-info\" id=\"Lbmsgdesc\">" + newsReplyInfo.NewsMsg[0].Description + "</div><div class=\"red-all clearfix\"><strong class=\"fl\">查看全文</strong><em class=\"fr\">&gt;</em></div></div>";
                                        break;
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;
                            case MessageType.List:
                                if (articleId <= 0)
                                {
                                    this.hdfIsOldArticle.Value = "1";
                                    NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(this.sendID) as NewsReplyInfo;
                                    if (newsReplyInfo != null)
                                    {
                                        StringBuilder stringBuilder = new StringBuilder();
                                        if (newsReplyInfo.NewsMsg != null && newsReplyInfo.NewsMsg.Count > 0)
                                        {
                                            int num = 0;
                                            foreach (NewsMsgInfo newsMsgInfo in (IEnumerable<NewsMsgInfo>)newsReplyInfo.NewsMsg)
                                            {
                                                ++num;
                                                if (num == 1)
                                                    stringBuilder.Append("<div class=\"mate-inner top\">                 <div class=\"mate-img\" >                     <img id=\"img1\" src=\"" + newsMsgInfo.PicUrl + "\" class=\"img-responsive\">                     <div class=\"title\" id=\"title1\">" + newsMsgInfo.Title + "</div>                 </div>             </div>");
                                                else
                                                    stringBuilder.Append("             <div class=\"mate-inner\">                 <div class=\"child-mate\">                     <div class=\"child-mate-title clearfix\">                         <div class=\"title\">" + newsMsgInfo.Title + "</div>                         <div class=\"img\">                             <img src=\"" + newsMsgInfo.PicUrl + "\" class=\"img-responsive\">                         </div>                     </div>                 </div>             </div>");
                                            }
                                            this.htmlInfo = ((object)stringBuilder).ToString();
                                            break;
                                        }
                                        else
                                            break;
                                    }
                                    else
                                        break;
                                }
                                else
                                    break;
                        }
                    }
                    else
                    {
                        this.Response.Redirect("sendalllist.aspx");
                        this.Response.End();
                    }
                }
                else if (this.LocalArticleID > 0)
                {
                    ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.LocalArticleID);
                    if (articleInfo != null)
                    {
                        this.hdfArticleID.Value = this.LocalArticleID.ToString();
                        this.hdfMessageType.Value = ((int)articleInfo.ArticleType).ToString();
                    }
                }
                if (string.IsNullOrEmpty(this.htmlInfo))
                    this.htmlInfo = "<div class=\"exit-shop-info\">内容区</div>";
                this.litInfo.Text = this.htmlInfo;
            }
        }
        private string SavePostData(int msgType, int articleid, string title, string content, int isoldarticle, int sendid, bool isonlycheck)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(title))
            {
                return "请输入标题！";
            }
            MessageType msgtype = (MessageType) msgType;
            if ((articleid < 1) && (msgtype != MessageType.Text))
            {
                if (isoldarticle == 0)
                {
                    return "请先选择图文！";
                }
                if ((sendid > 0) && !isonlycheck)
                {
                    articleid = ReplyHelper.GetArticleIDByOldArticle(sendid, msgtype);
                }
            }
            if (!isonlycheck)
            {
                SendAllInfo sendAllInfo = new SendAllInfo();
                if (sendid > 0)
                {
                    sendAllInfo = WeiXinHelper.GetSendAllInfo(sendid);
                }
                sendAllInfo.Title = title;
                sendAllInfo.MessageType = msgtype;
                sendAllInfo.ArticleID = articleid;
                sendAllInfo.Content = content;
                sendAllInfo.SendState = 0;
                sendAllInfo.SendTime = DateTime.Now;
                sendAllInfo.SendCount = 0;
                int num = Globals.ToNum(WeiXinHelper.SaveSendAllInfo(sendAllInfo, 1));
                if (num == 0)
                {
                    return "服务窗群发保存失败！";
                }
                if (num > 0)
                {
                    str = num.ToString();
                }
            }
            return str;
        }

        private string String2Json(string s)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s.ToCharArray()[i];
                switch (ch)
                {
                    case '/':
                    {
                        builder.Append(@"\/");
                        continue;
                    }
                    case '\\':
                    {
                        builder.Append(@"\\");
                        continue;
                    }
                    case '\b':
                    {
                        builder.Append(@"\b");
                        continue;
                    }
                    case '\t':
                    {
                        builder.Append(@"\t");
                        continue;
                    }
                    case '\n':
                    {
                        builder.Append(@"\n");
                        continue;
                    }
                    case '\f':
                    {
                        builder.Append(@"\f");
                        continue;
                    }
                    case '\r':
                    {
                        builder.Append(@"\r");
                        continue;
                    }
                    case '"':
                    {
                        builder.Append("\\\"");
                        continue;
                    }
                }
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}

