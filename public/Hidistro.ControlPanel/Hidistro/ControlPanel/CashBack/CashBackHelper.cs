namespace Hidistro.ControlPanel.CashBack
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.CashBack;
    using Hidistro.SqlDal.CashBack;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Runtime.InteropServices;

    public sealed class CashBackHelper
    {
        public static bool AddCashBack(CashBackInfo cashBackInfo)
        {
            return new CashBackDao().AddCashBack(cashBackInfo);
        }

        public static bool AddCashBackDetails(CashBackDetailsInfo cashBackDetailsInfo, DbTransaction dbTrans = null)
        {
            return new CashBackDao().AddCashBackDetails(cashBackDetailsInfo, dbTrans);
        }

        public static DbQueryResult GetCashBackByPager(CashBackQuery query)
        {
            return new CashBackDao().GetCashBackByPager(query);
        }

        public static DbQueryResult GetCashBackDetailsByPager(CashBackDetailsQuery query)
        {
            return new CashBackDao().GetCashBackDetailsByPager(query);
        }

        public static CashBackInfo GetCashBackInfo(int cashBackId)
        {
            return new CashBackDao().GetCashBackInfo(cashBackId);
        }

        public static IList<CashBackInfo> GetUnFinishedCashBackList()
        {
            return new CashBackDao().GetUnFinishedCashBackList();
        }

        public static bool UpdateCashBack(CashBackInfo cashBackInfo, DbTransaction dbTrans = null)
        {
            return new CashBackDao().UpdateCashBack(cashBackInfo, dbTrans);
        }
    }
}

