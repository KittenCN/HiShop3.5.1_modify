namespace Hidistro.UI.Web.Admin.WeiXin
{
    using  global:: ControlPanel.WeiBo;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ReplyOnKey : AdminPage
    {
        protected Repeater rptList;

        protected ReplyOnKey() : base("m06", "wxp03")
        {
        }

        private void BindArticleCategory()
        {
            List<Hidistro.Entities.VShop.ReplyInfo> list = ReplyHelper.GetAllReply().ToList<Hidistro.Entities.VShop.ReplyInfo>().FindAll(a => a.ReplyType < ReplyType.Wheel);
            this.rptList.DataSource = list;
            this.rptList.DataBind();
        }

        protected string GetReplyTypeName(object obj)
        {
            ReplyType en = (ReplyType) obj;
            string str = string.Empty;
            bool flag = false;
            if (ReplyType.Subscribe == (en & ReplyType.Subscribe))
            {
                str = str + "[<span style='color:orange;'>关注时回复</span>]";
                flag = true;
            }
            if (ReplyType.NoMatch == (en & ReplyType.NoMatch))
            {
                str = str + "[<span style='color:green;'>无匹配回复</span>]";
                flag = true;
            }
            if (ReplyType.Keys == (en & ReplyType.Keys))
            {
                str = str + "[关键字回复]";
                flag = true;
            }
            if (!flag)
            {
                str = en.ToShowText();
            }
            return str;
        }

        protected string GetTitleShow(object messagetypename, object articleid, object responseid)
        {
            string str = string.Empty;
            int num = Globals.ToNum(articleid);
            int id = Globals.ToNum(responseid);
            string str2 = messagetypename.ToString();
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "多图文"))
            {
                if (str2 == "单图文")
                {
                    if (num > 0)
                    {
                        ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(num);
                        if (articleInfo != null)
                        {
                            int num5 = 1;
                            str = string.Concat(new object[] { "<p>[<span style='color:green;'>图文", num5, "</span>] ", Globals.SubStr(articleInfo.Title, 40, "..."), "</p>" });
                        }
                        return str;
                    }
                    NewsReplyInfo info6 = ReplyHelper.GetReply(id) as NewsReplyInfo;
                    if (info6 == null)
                    {
                        return str;
                    }
                    StringBuilder builder2 = new StringBuilder();
                    if ((info6.NewsMsg != null) && (info6.NewsMsg.Count > 0))
                    {
                        int num6 = 0;
                        foreach (NewsMsgInfo info7 in info6.NewsMsg)
                        {
                            num6++;
                            builder2.Append(string.Concat(new object[] { "<p>[<span style='color:green;'>图文", num6, "</span>] ", Globals.SubStr(info7.Title, 40, "..."), "</p>" }));
                        }
                    }
                    return builder2.ToString();
                }
                if (str2 == "文本")
                {
                    TextReplyInfo info8 = ReplyHelper.GetReply(id) as TextReplyInfo;
                    if (info8 != null)
                    {
                        str = Globals.SubStr(Regex.Replace(Regex.Replace(info8.Text, "<[^>]+>", ""), "&[^;]+;", ""), 100, "...");
                        if (string.IsNullOrEmpty(str) && info8.Text.Contains("<img "))
                        {
                            str = "<span style='color:green;'>图文内容</span>";
                        }
                    }
                }
                return str;
            }
            if (num > 0)
            {
                ArticleInfo info = ArticleHelper.GetArticleInfo(num);
                if (info != null)
                {
                    int num3 = 1;
                    str = string.Concat(new object[] { "<p>[<span style='color:green;'>图文", num3, "</span>] ", Globals.SubStr(info.Title, 40, "..."), "</p>" });
                    if (info.ItemsInfo == null)
                    {
                        return str;
                    }
                    foreach (ArticleItemsInfo info2 in info.ItemsInfo)
                    {
                        num3++;
                        object obj2 = str;
                        str = string.Concat(new object[] { obj2, "<p>[<span style='color:green;'>图文", num3, "</span>] ", Globals.SubStr(info2.Title, 40, "..."), "</p>" });
                    }
                }
                return str;
            }
            NewsReplyInfo reply = ReplyHelper.GetReply(id) as NewsReplyInfo;
            if (reply == null)
            {
                return str;
            }
            StringBuilder builder = new StringBuilder();
            if ((reply.NewsMsg != null) && (reply.NewsMsg.Count > 0))
            {
                int num4 = 0;
                foreach (NewsMsgInfo info4 in reply.NewsMsg)
                {
                    num4++;
                    builder.Append(string.Concat(new object[] { "<p>[<span style='color:green;'>图文", num4, "</span>] ", Globals.SubStr(info4.Title, 40, "..."), "</p>" }));
                }
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindArticleCategory();
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Globals.ToNum(e.CommandArgument.ToString());
            if (e.CommandName == "Delete")
            {
                ReplyHelper.DeleteReply(id);
                this.BindArticleCategory();
                this.ShowMsg("删除成功！", true);
            }
            else if (e.CommandName == "Release")
            {
                ReplyHelper.UpdateReplyRelease(id);
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            else if (e.CommandName == "Edit")
            {
                Hidistro.Entities.VShop.ReplyInfo reply = ReplyHelper.GetReply(id);
                if (reply != null)
                {
                    switch (reply.MessageType)
                    {
                        case MessageType.Text:
                            base.Response.Redirect(string.Format("replyedit.aspx?id={0}", id));
                            return;

                        case MessageType.News:
                            base.Response.Redirect(string.Format("replyedit.aspx?id={0}", id));
                            return;

                        case (MessageType.News | MessageType.Text):
                            return;

                        case MessageType.List:
                            base.Response.Redirect(string.Format("replyedit.aspx?id={0}", id));
                            return;
                    }
                }
            }
        }
    }
}

