namespace ControlPanel.Promotions
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Data;

    public class ShareActHelper
    {
        private static ShareActDao _share = new ShareActDao();

        public static bool AddRecord(ShareActivityRecordInfo record)
        {
            return _share.AddRecord(record);
        }

        public static int Create(ShareActivityInfo act, ref string msg)
        {
            return _share.Create(act, ref msg);
        }

        public static bool Delete(int Id)
        {
            return _share.Delete(Id);
        }

        public static ShareActivityInfo GetAct(int Id)
        {
            return _share.GetAct(Id);
        }

        public static int GeTAttendCount(int actId, int shareUser)
        {
            return _share.GeTAttendCount(actId, shareUser);
        }

        public static DataTable GetOrderRedPager(string OrderID, int UserID)
        {
            return _share.GetOrderRedPager(OrderID, UserID);
        }

        public static DataTable GetShareActivity()
        {
            return _share.GetShareActivity();
        }

        public static ShareActivityInfo GetShareActivity(int CouponId)
        {
            return _share.GetShareActivity(CouponId);
        }

        public static bool HasAttend(int actId, int attendUser)
        {
            return _share.HasAttend(actId, attendUser);
        }

        public static DbQueryResult Query(ShareActivitySearch query)
        {
            return _share.Query(query);
        }

        public static bool Update(ShareActivityInfo act, ref string msg)
        {
            return _share.Update(act, ref msg);
        }
    }
}

