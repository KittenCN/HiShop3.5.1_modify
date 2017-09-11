namespace Hidistro.UI.Web.Admin.AliFuwu
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ReplyEdit : AdminPage
    {
        protected HtmlForm aspnetForm;
        protected Button btnSave;
        protected ucUeditor fkContent;
        protected HiddenField hdfArticleID;
        protected HiddenField hdfIsOldArticle;
        protected HiddenField hdfMessageType;
        protected string htmlInfo;
        protected string htmlTitle;
        protected Literal litInfo;
        protected RadioButtonList rbtlMatchType;
        protected int replyID;
        protected TextBox txtKeys;
        protected string type;

        protected ReplyEdit() : base("m11", "fwp06")
        {
            this.htmlTitle = "新增自动回复";
            this.htmlInfo = string.Empty;
            this.replyID = Globals.RequestQueryNum("ID");
            this.type = Globals.RequestQueryStr("type");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NewsReplyInfo info;
            int num = Globals.ToNum(this.hdfMessageType.Value);
            int articleIDByOldArticle = Globals.ToNum(this.hdfArticleID.Value);
            MessageType msgtype = (MessageType) num;
            int num3 = Globals.ToNum(this.rbtlMatchType.SelectedValue);
            if (string.IsNullOrEmpty(this.txtKeys.Text.Trim()) && (this.type != "subscribe"))
            {
                this.ShowMsg("请输入关键词！", false);
                return;
            }
            if (this.txtKeys.Text.Trim().Length > 50)
            {
                this.ShowMsg("关键词必须少于50个字！", false);
                return;
            }
            if (num3 == 0)
            {
                this.ShowMsg("请选择匹配类型！", false);
                return;
            }
            if ((articleIDByOldArticle < 1) && (msgtype != MessageType.Text))
            {
                if (this.hdfIsOldArticle.Value == "0")
                {
                    this.ShowMsg("请先选择图文！", false);
                    return;
                }
                if (this.replyID > 0)
                {
                    articleIDByOldArticle = AliFuwuReplyHelper.GetArticleIDByOldArticle(this.replyID, msgtype);
                }
            }
            switch (msgtype)
            {
                case MessageType.Text:
                    if (this.fkContent.Text.Length <= 0x3e8)
                    {
                        TextReplyInfo reply = new TextReplyInfo {
                            Keys = this.txtKeys.Text.Trim(),
                            MatchType = (num3 == 2) ? MatchType.Equal : MatchType.Like
                        };
                        if (reply.Keys == "*")
                        {
                            reply.ReplyType = ReplyType.NoMatch;
                            if (AliFuwuReplyHelper.GetNoMatchReplyID(this.replyID) > 0)
                            {
                                this.ShowMsg("无关键字回复回复内容已存在！", false);
                                return;
                            }
                        }
                        else if (this.type == "subscribe")
                        {
                            if (AliFuwuReplyHelper.GetSubscribeID(this.replyID) > 0)
                            {
                                this.ShowMsg("首次关注回复内容已存在！", false);
                                return;
                            }
                            reply.ReplyType = ReplyType.Subscribe;
                            reply.Keys = "";
                        }
                        else
                        {
                            reply.ReplyType = ReplyType.Keys;
                            if (AliFuwuReplyHelper.HasReplyKey(reply.Keys, this.replyID))
                            {
                                this.ShowMsg("该关键词已存在！", false);
                                return;
                            }
                        }
                        reply.MessageType = msgtype;
                        reply.IsDisable = false;
                        reply.ArticleID = articleIDByOldArticle;
                        string str = Regex.Replace(Regex.Replace(this.fkContent.Text, "</?([^>^a^p]*)>", ""), "<img([^>]*)>", "").Replace("<p>", "").Replace("</p>", "\r").Trim(new char[] { '\r' }).Replace("\r", "\r\n");
                        reply.Text = str;
                        reply.Id = this.replyID;
                        if (string.IsNullOrEmpty(reply.Text))
                        {
                            this.ShowMsg("请填写文本内容！", false);
                            return;
                        }
                        if (this.replyID > 0)
                        {
                            AliFuwuReplyHelper.UpdateReply(reply);
                        }
                        else
                        {
                            AliFuwuReplyHelper.SaveReply(reply);
                        }
                        goto Label_0406;
                    }
                    this.ShowMsg("回复内容必须1000字以内！", false);
                    return;

                case MessageType.News:
                case MessageType.List:
                    info = new NewsReplyInfo {
                        Keys = this.txtKeys.Text.Trim(),
                        MatchType = (num3 == 2) ? MatchType.Equal : MatchType.Like
                    };
                    if (!(info.Keys == "*"))
                    {
                        if (this.type == "subscribe")
                        {
                            info.ReplyType = ReplyType.Subscribe;
                            info.Keys = "";
                            if (AliFuwuReplyHelper.GetSubscribeID(this.replyID) > 0)
                            {
                                this.ShowMsg("首次关注回复已存在！", false);
                                return;
                            }
                        }
                        else
                        {
                            info.ReplyType = ReplyType.Keys;
                            if (AliFuwuReplyHelper.HasReplyKey(info.Keys, this.replyID))
                            {
                                this.ShowMsg("该关键词已存在！", false);
                                return;
                            }
                        }
                        break;
                    }
                    info.ReplyType = ReplyType.NoMatch;
                    if (AliFuwuReplyHelper.GetNoMatchReplyID(this.replyID) <= 0)
                    {
                        break;
                    }
                    this.ShowMsg("无关键词回复已存在！", false);
                    return;

                default:
                    goto Label_0406;
            }
            info.MessageType = msgtype;
            info.IsDisable = false;
            info.ArticleID = articleIDByOldArticle;
            info.Id = this.replyID;
            if (num3 < 1)
            {
                this.ShowMsg("请选择类型！", false);
                return;
            }
            if (this.replyID > 0)
            {
                AliFuwuReplyHelper.UpdateReply(info);
            }
            else
            {
                AliFuwuReplyHelper.SaveReply(info);
            }
        Label_0406:
            if (this.replyID > 0)
            {
                this.ShowMsgAndReUrl("自动回复修改成功！", true, "replyonkey.aspx");
            }
            else
            {
                this.ShowMsgAndReUrl("自动回复添加成功！", true, "replyonkey.aspx");
            }
        }

        [PrivilegeCheck(Privilege.Summary)]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsPostBack)
            {
                return;
            }
            if (!(this.type == "getarticleinfo"))
            {
                if ((this.type == "subscribe") && (this.replyID == 0))
                {
                    this.replyID = AliFuwuReplyHelper.GetSubscribeID(0);
                    if (this.replyID > 0)
                    {
                        base.Response.Redirect("replyedit.aspx?type=subscribe&id=" + this.replyID);
                        base.Response.End();
                    }
                    this.rbtlMatchType.SelectedIndex = 0;
                    if (string.IsNullOrEmpty(this.htmlInfo))
                    {
                        this.htmlInfo = "<div class=\"exit-shop-info\">内容区</div>";
                    }
                    this.litInfo.Text = this.htmlInfo;
                    return;
                }
                if (this.replyID > 0)
                {
                    this.htmlTitle = "修改自动回复";
                    Hidistro.Entities.VShop.ReplyInfo reply = AliFuwuReplyHelper.GetReply(this.replyID);
                    if (reply == null)
                    {
                        base.Response.Redirect("replyonkey.aspx");
                        base.Response.End();
                    }
                    else
                    {
                        MessageType messageType = reply.MessageType;
                        if (ReplyType.NoMatch == reply.ReplyType)
                        {
                            this.txtKeys.Text = "*";
                        }
                        else if (ReplyType.Subscribe == reply.ReplyType)
                        {
                            this.txtKeys.Text = "";
                            if (this.type != "subscribe")
                            {
                                base.Response.Redirect("replyedit.aspx?type=subscribe&id=" + this.replyID);
                                base.Response.End();
                            }
                        }
                        else
                        {
                            this.txtKeys.Text = reply.Keys.Trim();
                        }
                        for (int i = 0; i < this.rbtlMatchType.Items.Count; i++)
                        {
                            if (this.rbtlMatchType.Items[i].Value == ((int) reply.MatchType).ToString())
                            {
                                this.rbtlMatchType.Items[i].Selected = true;
                                break;
                            }
                        }
                        this.hdfMessageType.Value = ((int) reply.MessageType).ToString();
                        this.hdfArticleID.Value = reply.ArticleID.ToString();
                        int articleID = reply.ArticleID;
                        switch (messageType)
                        {
                            case MessageType.Text:
                            {
                                TextReplyInfo info7 = AliFuwuReplyHelper.GetReply(this.replyID) as TextReplyInfo;
                                if (info7 != null)
                                {
                                    string str2 = Regex.Replace(Regex.Replace(info7.Text, "</?([^>^a^p]*)>", ""), "<img([^>]*)>", "").Replace("</p>", "\r\n").Replace("<p>", "");
                                    this.fkContent.Text = str2;
                                }
                                break;
                            }
                            case MessageType.News:
                                if (articleID <= 0)
                                {
                                    this.hdfIsOldArticle.Value = "1";
                                    NewsReplyInfo info6 = AliFuwuReplyHelper.GetReply(this.replyID) as NewsReplyInfo;
                                    if (((info6 != null) && (info6.NewsMsg != null)) && (info6.NewsMsg.Count != 0))
                                    {
                                        this.htmlInfo = "<div class=\"mate-inner\"><h3 id=\"singelTitle\">" + info6.NewsMsg[0].Title + "</h3><span>" + info6.LastEditDate.ToString("M月d日") + "</span><div class=\"mate-img\"><img id=\"img1\" src=\"" + info6.NewsMsg[0].PicUrl + "\" class=\"img-responsive\"></div><div class=\"mate-info\" id=\"Lbmsgdesc\">" + info6.NewsMsg[0].Description + "</div><div class=\"red-all clearfix\"><strong class=\"fl\">查看全文</strong><em class=\"fr\">&gt;</em></div></div>";
                                    }
                                }
                                break;

                            case MessageType.List:
                                if (articleID <= 0)
                                {
                                    this.hdfIsOldArticle.Value = "1";
                                    NewsReplyInfo info4 = AliFuwuReplyHelper.GetReply(this.replyID) as NewsReplyInfo;
                                    if (info4 != null)
                                    {
                                        StringBuilder builder2 = new StringBuilder();
                                        if ((info4.NewsMsg != null) && (info4.NewsMsg.Count > 0))
                                        {
                                            int num4 = 0;
                                            foreach (NewsMsgInfo info5 in info4.NewsMsg)
                                            {
                                                num4++;
                                                if (num4 == 1)
                                                {
                                                    builder2.Append("<div class=\"mate-inner top\">                 <div class=\"mate-img\" >                     <img id=\"img1\" src=\"" + info5.PicUrl + "\" class=\"img-responsive\">                     <div class=\"title\" id=\"title1\">" + info5.Title + "</div>                 </div>             </div>");
                                                }
                                                else
                                                {
                                                    builder2.Append("             <div class=\"mate-inner\">                 <div class=\"child-mate\">                     <div class=\"child-mate-title clearfix\">                         <div class=\"title\">" + info5.Title + "</div>                         <div class=\"img\">                             <img src=\"" + info5.PicUrl + "\" class=\"img-responsive\">                         </div>                     </div>                 </div>             </div>");
                                                }
                                            }
                                            this.htmlInfo = builder2.ToString();
                                        }
                                    }
                                }
                                break;
                        }
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
            string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
            int articleid = Globals.RequestFormNum("articleid");
            if (articleid > 0)
            {
                ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(articleid);
                if (articleInfo != null)
                {
                    StringBuilder builder = new StringBuilder();
                    switch (articleInfo.ArticleType)
                    {
                        case ArticleType.News:
                            s = string.Concat(new object[] { "{\"type\":\"1\",\"articletype\":", (int) articleInfo.ArticleType, ",\"title\":\"", String2Json(articleInfo.Title), "\",\"date\":\"", String2Json(articleInfo.PubTime.ToString("M月d日")), "\",\"imgurl\":\"", String2Json(articleInfo.ImageUrl), "\",\"memo\":\"", String2Json(articleInfo.Memo), "\"}" });
                            goto Label_0301;

                        case ArticleType.List:
                            foreach (ArticleItemsInfo info2 in articleInfo.ItemsInfo)
                            {
                                builder.Append("{\"title\":\"" + String2Json(info2.Title) + "\",\"imgurl\":\"" + String2Json(info2.ImageUrl) + "\"},");
                            }
                            s = string.Concat(new object[] { "{\"type\":\"1\",\"articletype\":", (int) articleInfo.ArticleType, ",\"title\":\"", String2Json(articleInfo.Title), "\",\"date\":\"", String2Json(articleInfo.PubTime.ToString("M月d日")), "\",\"imgurl\":\"", String2Json(articleInfo.ImageUrl), "\",\"items\":[", builder.ToString().Trim(new char[] { ',' }), "]}" });
                            goto Label_0301;
                    }
                    s = string.Concat(new object[] { "{\"type\":\"1\",\"articletype\":", (int) articleInfo.ArticleType, ",\"title\":\"", String2Json(articleInfo.Title), "\",\"date\":\"", String2Json(articleInfo.PubTime.ToString("M月d日")), "\",\"imgurl\":\"", String2Json(articleInfo.ImageUrl), "\",\"memo\":\"", String2Json(articleInfo.Content), "\"}" });
                }
            }
        Label_0301:
            base.Response.Write(s);
            base.Response.End();
        }

        private static string String2Json(string s)
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

