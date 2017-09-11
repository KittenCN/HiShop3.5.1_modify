namespace Hidistro.UI.Web.Admin.WeiXin
{
    using global::ControlPanel.WeiBo;
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
    using Hishop.Weixin.MP.Api;
    using Hishop.Weixin.MP.Domain;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Text;
    using System.Text.RegularExpressions;
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

        protected SendAllEdit() : base("m06", "wxp10")
        {
            this.errcode = string.Empty;
            this.htmlInfo = string.Empty;
            this.sendID = Globals.RequestQueryNum("ID");
            this.LocalArticleID = Globals.RequestQueryNum("aid");
            this.type = Globals.RequestQueryStr("type");
        }

        public string CreateTxtNewsJson(string content)
        {
            string s = this.FormatSendContent(content);
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"filter\":{\"is_to_all\":true},");
            builder.Append("\"text\":{\"content\":\"" + this.String2Json(s) + "\"},");
            builder.Append("\"msgtype\":\"text\"");
            builder.Append("}");
            return builder.ToString();
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

        public string GetArticlesJsonStr(ArticleInfo info)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"articles\":[");
            builder.Append("{");
            builder.Append("\"thumb_media_id\":\"" + info.MediaId + "\",");
            builder.Append("\"author\":\"\",");
            builder.Append("\"title\":\"" + this.String2Json(info.Title) + "\",");
            builder.Append("\"content_source_url\":\"" + this.String2Json(info.Url) + "\",");
            builder.Append("\"content\":\"" + this.String2Json(info.Content) + "\",");
            builder.Append("\"digest\":\"" + this.String2Json(info.Memo) + "\",");
            builder.Append("\"show_cover_pic\":\"1\"}");
            if (info.ArticleType == ArticleType.List)
            {
                foreach (ArticleItemsInfo info2 in info.ItemsInfo)
                {
                    builder.Append(",{");
                    builder.Append("\"thumb_media_id\":\"" + info2.MediaId + "\",");
                    builder.Append("\"author\":\"\",");
                    builder.Append("\"title\":\"" + this.String2Json(info2.Title) + "\",");
                    builder.Append("\"content_source_url\":\"" + this.String2Json(info2.Url) + "\",");
                    builder.Append("\"content\":\"" + this.String2Json(info2.Content) + "\",");
                    builder.Append("\"digest\":\"\",");
                    builder.Append("\"show_cover_pic\":\"0\"}");
                }
            }
            builder.Append("]}");
            return builder.ToString();
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
            string str2;
            if (base.IsPostBack)
            {
                return;
            }
            if (!(this.type == "getarticleinfo"))
            {
                if (!(this.type == "postdata"))
                {
                    if (this.sendID > 0)
                    {
                        this.hdfSendID.Value = this.sendID.ToString();
                        SendAllInfo sendAllInfo = WeiXinHelper.GetSendAllInfo(this.sendID);
                        if (sendAllInfo != null)
                        {
                            MessageType messageType = sendAllInfo.MessageType;
                            this.hdfMessageType.Value = ((int) sendAllInfo.MessageType).ToString();
                            int articleID = sendAllInfo.ArticleID;
                            this.hdfArticleID.Value = articleID.ToString();
                            this.txtTitle.Text = sendAllInfo.Title;
                            switch (messageType)
                            {
                                case MessageType.Text:
                                    this.fkContent.Text = sendAllInfo.Content;
                                    break;

                                case MessageType.News:
                                    if (articleID <= 0)
                                    {
                                        this.hdfIsOldArticle.Value = "1";
                                        NewsReplyInfo reply = ReplyHelper.GetReply(this.sendID) as NewsReplyInfo;
                                        if (((reply != null) && (reply.NewsMsg != null)) && (reply.NewsMsg.Count != 0))
                                        {
                                            this.htmlInfo = "<div class=\"mate-inner\"><h3 id=\"singelTitle\">" + reply.NewsMsg[0].Title + "</h3><span>" + reply.LastEditDate.ToString("M月d日") + "</span><div class=\"mate-img\"><img id=\"img1\" src=\"" + reply.NewsMsg[0].PicUrl + "\" class=\"img-responsive\"></div><div class=\"mate-info\" id=\"Lbmsgdesc\">" + reply.NewsMsg[0].Description + "</div><div class=\"red-all clearfix\"><strong class=\"fl\">查看全文</strong><em class=\"fr\">&gt;</em></div></div>";
                                        }
                                    }
                                    break;

                                case MessageType.List:
                                    if (articleID <= 0)
                                    {
                                        this.hdfIsOldArticle.Value = "1";
                                        NewsReplyInfo info6 = ReplyHelper.GetReply(this.sendID) as NewsReplyInfo;
                                        if (info6 != null)
                                        {
                                            StringBuilder builder2 = new StringBuilder();
                                            if ((info6.NewsMsg != null) && (info6.NewsMsg.Count > 0))
                                            {
                                                int num15 = 0;
                                                foreach (NewsMsgInfo info7 in info6.NewsMsg)
                                                {
                                                    num15++;
                                                    if (num15 == 1)
                                                    {
                                                        builder2.Append("<div class=\"mate-inner top\">                 <div class=\"mate-img\" >                     <img id=\"img1\" src=\"" + info7.PicUrl + "\" class=\"img-responsive\">                     <div class=\"title\" id=\"title1\">" + info7.Title + "</div>                 </div>             </div>");
                                                    }
                                                    else
                                                    {
                                                        builder2.Append("             <div class=\"mate-inner\">                 <div class=\"child-mate\">                     <div class=\"child-mate-title clearfix\">                         <div class=\"title\">" + info7.Title + "</div>                         <div class=\"img\">                             <img src=\"" + info7.PicUrl + "\" class=\"img-responsive\">                         </div>                     </div>                 </div>             </div>");
                                                    }
                                                }
                                                this.htmlInfo = builder2.ToString();
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            base.Response.Redirect("sendalllist.aspx");
                            base.Response.End();
                        }
                    }
                    else if (this.LocalArticleID > 0)
                    {
                        ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(this.LocalArticleID);
                        if (articleInfo != null)
                        {
                            this.hdfArticleID.Value = this.LocalArticleID.ToString();
                            this.hdfMessageType.Value = ((int) articleInfo.ArticleType).ToString();
                        }
                    }
                    if (string.IsNullOrEmpty(this.htmlInfo))
                    {
                        this.htmlInfo = "<div class=\"exit-shop-info\">内容区</div>";
                    }
                    this.litInfo.Text = this.htmlInfo;
                    return;
                }
                base.Response.ContentType = "application/json";
                str2 = "{\"type\":\"1\",\"tips\":\"操作成功\"}";
                this.sendID = Globals.RequestFormNum("sendid");
                int num2 = Globals.RequestFormNum("sendtype");
                int msgType = Globals.RequestFormNum("msgtype");
                int num4 = Globals.RequestFormNum("articleid");
                string title = Globals.RequestFormStr("title");
                string content = Globals.RequestFormStr("content");
                int isoldarticle = Globals.RequestFormNum("isoldarticle");
                string str5 = this.SavePostData(msgType, num4, title, content, isoldarticle, this.sendID, true);
                if (!string.IsNullOrEmpty(str5))
                {
                    str2 = "{\"type\":\"0\",\"tips\":\"" + str5 + "\"}";
                }
                else
                {
                    MessageType type = (MessageType) msgType;
                    string str6 = string.Empty;
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string str7 = JsonConvert.DeserializeObject<Token>(TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret)).access_token;
                    switch (type)
                    {
                        case MessageType.News:
                        case MessageType.List:
                        {
                            bool flag = true;
                            ArticleInfo info3 = ArticleHelper.GetArticleInfo(num4);
                            if (info3.MediaId.Length < 1)
                            {
                                string jsonValue = NewsApi.GetJsonValue(NewsApi.GetMedia_IDByPath(str7, info3.ImageUrl), "media_id");
                                if (string.IsNullOrEmpty(jsonValue))
                                {
                                    flag = false;
                                    str2 = "{\"type\":\"2\",\"tips\":\"" + NewsApi.GetErrorCodeMsg(NewsApi.GetJsonValue(jsonValue, "errcode")) + "111111\"}";
                                }
                                else
                                {
                                    ArticleHelper.UpdateMedia_Id(0, info3.ArticleId, jsonValue);
                                }
                            }
                            if (type == MessageType.List)
                            {
                                foreach (ArticleItemsInfo info4 in info3.ItemsInfo)
                                {
                                    if ((info4.MediaId == null) || (info4.MediaId.Length < 1))
                                    {
                                        string msg = NewsApi.GetMedia_IDByPath(str7, info4.ImageUrl);
                                        string mediaid = NewsApi.GetJsonValue(msg, "media_id");
                                        if (mediaid.Length == 0)
                                        {
                                            this.errcode = NewsApi.GetJsonValue(msg, "errcode");
                                            msg = "";
                                            flag = false;
                                            str2 = "{\"type\":\"2\",\"tips\":\"" + NewsApi.GetErrorCodeMsg(this.errcode) + "\"}";
                                            break;
                                        }
                                        ArticleHelper.UpdateMedia_Id(1, info4.Id, mediaid);
                                    }
                                }
                            }
                            if (flag)
                            {
                                string articlesJsonStr = this.GetArticlesJsonStr(info3);
                                string str12 = NewsApi.UploadNews(str7, articlesJsonStr);
                                this.sendID = Globals.ToNum(this.SavePostData(msgType, num4, title, content, isoldarticle, this.sendID, false));
                                string str13 = NewsApi.GetJsonValue(str12, "media_id");
                                if (str13.Length > 0)
                                {
                                    if (num2 == 1)
                                    {
                                        str6 = NewsApi.SendAll(str7, NewsApi.CreateImageNewsJson(str13));
                                        if (!string.IsNullOrWhiteSpace(str6))
                                        {
                                            string str14 = NewsApi.GetJsonValue(str6, "msg_id");
                                            if (!string.IsNullOrEmpty(str14))
                                            {
                                                WeiXinHelper.UpdateMsgId(this.sendID, str14, 0, 0, 0, "");
                                            }
                                            else
                                            {
                                                this.errcode = NewsApi.GetJsonValue(str6, "errcode");
                                                string errorCodeMsg = NewsApi.GetErrorCodeMsg(this.errcode);
                                                WeiXinHelper.UpdateMsgId(this.sendID, str14, 2, 0, 0, errorCodeMsg);
                                                str2 = "{\"type\":\"2\",\"tips\":\"" + errorCodeMsg + "!!\"}";
                                            }
                                        }
                                        else
                                        {
                                            str2 = "{\"type\":\"2\",\"tips\":\"type参数错误\"}";
                                        }
                                    }
                                    else
                                    {
                                        DataTable rencentOpenID = WeiXinHelper.GetRencentOpenID(100);
                                        int count = rencentOpenID.Rows.Count;
                                        int sendcount = 0;
                                        string returnjsondata = string.Empty;
                                        for (int i = 0; i < rencentOpenID.Rows.Count; i++)
                                        {
                                            string str17 = NewsApi.KFSend(str7, this.GetKFSendImageJson(rencentOpenID.Rows[i][0].ToString(), info3));
                                            this.errcode = NewsApi.GetJsonValue(str17, "errcode");
                                            if (this.errcode == "0")
                                            {
                                                sendcount++;
                                            }
                                            else
                                            {
                                                returnjsondata = NewsApi.GetErrorCodeMsg(this.errcode);
                                            }
                                        }
                                        int sendstate = (sendcount > 0) ? 1 : 2;
                                        WeiXinHelper.UpdateMsgId(this.sendID, "", sendstate, sendcount, count, returnjsondata);
                                    }
                                }
                                else
                                {
                                    this.errcode = NewsApi.GetJsonValue(str12, "errcode");
                                    str2 = "{\"type\":\"2\",\"tips\":\"" + NewsApi.GetErrorCodeMsg(this.errcode) + "！\"}";
                                }
                            }
                            goto Label_091D;
                        }
                    }
                    this.sendID = Globals.ToNum(this.SavePostData(msgType, num4, title, content, isoldarticle, this.sendID, false));
                    if (num2 == 1)
                    {
                        str6 = NewsApi.SendAll(str7, this.CreateTxtNewsJson(content));
                        if (!string.IsNullOrWhiteSpace(str6))
                        {
                            string msgid = NewsApi.GetJsonValue(str6, "msg_id");
                            if (msgid.Length == 0)
                            {
                                this.errcode = NewsApi.GetJsonValue(str6, "errcode");
                                string str19 = NewsApi.GetErrorCodeMsg(this.errcode);
                                WeiXinHelper.UpdateMsgId(this.sendID, msgid, 2, 0, 0, str19);
                                str2 = "{\"type\":\"2\",\"tips\":\"" + str19 + "\"}";
                            }
                            else
                            {
                                WeiXinHelper.UpdateMsgId(this.sendID, msgid, 0, 0, 0, "");
                            }
                        }
                        else
                        {
                            str2 = "{\"type\":\"2\",\"tips\":\"type参数错误\"}";
                        }
                    }
                    else
                    {
                        DataTable table2 = WeiXinHelper.GetRencentOpenID(100);
                        int totalcount = table2.Rows.Count;
                        int num11 = 0;
                        string str20 = string.Empty;
                        for (int j = 0; j < table2.Rows.Count; j++)
                        {
                            string str21 = NewsApi.KFSend(str7, NewsApi.CreateKFTxtNewsJson(table2.Rows[j][0].ToString(), this.String2Json(this.FormatSendContent(content))));
                            this.errcode = NewsApi.GetJsonValue(str21, "errcode");
                            if (this.errcode == "0")
                            {
                                num11++;
                            }
                            else
                            {
                                str20 = NewsApi.GetErrorCodeMsg(this.errcode);
                            }
                        }
                        int num13 = (num11 > 0) ? 1 : 2;
                        WeiXinHelper.UpdateMsgId(this.sendID, "", num13, num11, totalcount, str20);
                        if (num11 == 0)
                        {
                            str2 = "{\"type\":\"0\",\"tips\":\"发送失败\"}";
                        }
                    }
                }
                goto Label_091D;
            }
            base.Response.ContentType = "application/json";
            string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
            int articleid = Globals.RequestFormNum("articleid");
            if (articleid > 0)
            {
                ArticleInfo info = ArticleHelper.GetArticleInfo(articleid);
                if (info != null)
                {
                    StringBuilder builder = new StringBuilder();
                    switch (info.ArticleType)
                    {
                        case ArticleType.News:
                            s = string.Concat(new object[] { "{\"type\":\"1\",\"articletype\":", (int) info.ArticleType, ",\"title\":\"", this.String2Json(info.Title), "\",\"date\":\"", this.String2Json(info.PubTime.ToString("M月d日")), "\",\"imgurl\":\"", this.String2Json(info.ImageUrl), "\",\"memo\":\"", this.String2Json(info.Memo), "\"}" });
                            goto Label_030E;

                        case ArticleType.List:
                            foreach (ArticleItemsInfo info2 in info.ItemsInfo)
                            {
                                builder.Append("{\"title\":\"" + this.String2Json(info2.Title) + "\",\"imgurl\":\"" + this.String2Json(info2.ImageUrl) + "\"},");
                            }
                            s = string.Concat(new object[] { "{\"type\":\"1\",\"articletype\":", (int) info.ArticleType, ",\"title\":\"", this.String2Json(info.Title), "\",\"date\":\"", this.String2Json(info.PubTime.ToString("M月d日")), "\",\"imgurl\":\"", this.String2Json(info.ImageUrl), "\",\"items\":[", builder.ToString().Trim(new char[] { ',' }), "]}" });
                            goto Label_030E;
                    }
                    s = string.Concat(new object[] { "{\"type\":\"1\",\"articletype\":", (int) info.ArticleType, ",\"title\":\"", this.String2Json(info.Title), "\",\"date\":\"", this.String2Json(info.PubTime.ToString("M月d日")), "\",\"imgurl\":\"", this.String2Json(info.ImageUrl), "\",\"memo\":\"", this.String2Json(info.Content), "\"}" });
                }
            }
        Label_030E:
            base.Response.Write(s);
            base.Response.End();
            return;
        Label_091D:
            base.Response.Write(str2);
            base.Response.End();
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
                int num = Globals.ToNum(WeiXinHelper.SaveSendAllInfo(sendAllInfo, 0));
                if (num == 0)
                {
                    return "微信群发保存失败！";
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

