namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VNoticeDetail : VshopTemplatedWebControl
    {
        private Literal litMemo;
        private Literal litPubTime;
        private Literal litTitle;

        protected override void AttachChildControls()
        {
            string str = Globals.RequestQueryStr("type");
            int id = Globals.RequestQueryNum("id");
            this.litTitle = (Literal) this.FindControl("litTitle");
            this.litPubTime = (Literal) this.FindControl("litPubTime");
            this.litMemo = (Literal) this.FindControl("litMemo");
            NoticeInfo noticeInfo = NoticeBrowser.GetNoticeInfo(id);
            if (noticeInfo != null)
            {
                int sendType = noticeInfo.SendType;
                string str2 = "公告";
                if (sendType == 1)
                {
                    str2 = "消息";
                }
                this.litTitle.Text = noticeInfo.Title;
                DateTime time = noticeInfo.PubTime.HasValue ? noticeInfo.PubTime.Value : noticeInfo.AddTime;
                this.litPubTime.Text = "<span>" + time.ToString("yyyy-MM-dd") + "</span><span><i class='glyphicon glyphicon-time'></i>" + time.ToString("HH:mm") + "</span>";
                this.litMemo.Text = string.Concat(new object[] { noticeInfo.Memo, "<p class=\"lookall\"><a href=\"notice.aspx?type=", sendType, "\">更多", str2, "&gt;&gt;</a></p>" });
                PageTitle.AddSiteNameTitle(noticeInfo.Title);
                if (str != "view")
                {
                    if (noticeInfo.IsPub == 1)
                    {
                        NoticeBrowser.ViewNotice(MemberProcessor.GetCurrentMember().UserId, id);
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("文章未发布！");
                        HttpContext.Current.Response.End();
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/default.aspx");
                HttpContext.Current.Response.End();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vNoticeDetail.html";
            }
            base.OnInit(e);
        }
    }
}

