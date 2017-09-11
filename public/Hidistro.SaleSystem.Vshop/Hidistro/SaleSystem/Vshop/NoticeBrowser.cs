namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Store;
    using System;
    using System.Data;

    public static class NoticeBrowser
    {
        public static NoticeInfo GetNoticeInfo(int id)
        {
            return new NoticeDao().GetNoticeInfo(id);
        }

        public static int GetNoticeNotReadCount(NoticeQuery query)
        {
            return new NoticeDao().GetNoticeNotReadCount(query);
        }

        public static DataTable GetNoticeNotReadDt(NoticeQuery query)
        {
            return new NoticeDao().GetNoticeNotReadDt(query);
        }

        public static DbQueryResult GetNoticeRequest(NoticeQuery query)
        {
            return new NoticeDao().GetNoticeRequest(query);
        }

        public static bool IsView(int userid, int noticeid)
        {
            return new NoticeDao().IsView(userid, noticeid);
        }

        public static int SaveNotice(NoticeInfo info)
        {
            return new NoticeDao().SaveNotice(info);
        }

        public static void ViewNotice(int userid, int noticeid)
        {
            new NoticeDao().ViewNotice(userid, noticeid);
        }
    }
}

