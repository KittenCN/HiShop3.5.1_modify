namespace Hidistro.UI.Web.API
{
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class FootJs1 : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                base.Response.Write(";");
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                int userId = currentMember.UserId;
                NoticeQuery query = new NoticeQuery {
                    SendType = 0,
                    UserId = new int?(userId)
                };
                bool flag = DistributorsBrower.GetDistributorInfo(userId) != null;
                query.IsDistributor = new bool?(flag);
                query.IsDel = 0;
                DataTable noticeNotReadDt = NoticeBrowser.GetNoticeNotReadDt(query);
                if ((noticeNotReadDt != null) && (noticeNotReadDt.Rows.Count > 0))
                {
                    if (noticeNotReadDt.Select("SendType='0'") != null)
                    {
                        builder.Append("$('.my-message').html('<i></i>');");
                    }
                    else
                    {
                        builder.Append("$('.my-message').html('<i></i>').attr('href','notice.aspx?type=1');");
                    }
                    for (int i = 0; i < noticeNotReadDt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(noticeNotReadDt.Rows[i]["SendType"]) == 0)
                        {
                            builder.Append(string.Concat(new object[] { "$('.new_message ul').append('<li><a href=\"NoticeDetail.aspx?Id=", noticeNotReadDt.Rows[i]["Id"], "\">", noticeNotReadDt.Rows[i]["Title"], "</a></li>');" }));
                        }
                        else
                        {
                            builder.Append(string.Concat(new object[] { "$('.new_message ul').append('<li><a  href=\"NoticeDetail.aspx?Id=", noticeNotReadDt.Rows[i]["Id"], "\">", noticeNotReadDt.Rows[i]["Title"], "</a></li>');" }));
                        }
                    }
                }
                base.Response.Write(builder.ToString());
            }
            base.Response.End();
        }
    }
}

