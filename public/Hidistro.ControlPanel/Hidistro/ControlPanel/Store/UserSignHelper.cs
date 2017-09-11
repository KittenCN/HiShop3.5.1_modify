namespace Hidistro.ControlPanel.Store
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Data;

    public static class UserSignHelper
    {
        public static int AddPoint(UserSign us)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (!masterSettings.sign_score_Enable)
            {
                return 0;
            }
            IntegralDetailInfo point = new IntegralDetailInfo {
                IntegralSourceType = 1,
                IntegralSource = "签到",
                Userid = us.UserID,
                IntegralChange = masterSettings.SignPoint,
                IntegralStatus = Convert.ToInt32(IntegralDetailStatus.SignToIntegral)
            };
            if (masterSettings.sign_score_Enable && (us.Continued >= masterSettings.SignWhere))
            {
                point.IntegralChange += masterSettings.SignWherePoint;
                us.Continued = 0;
            }
            IntegralDetailHelp.AddIntegralDetail(point, null);
            return Convert.ToInt32(point.IntegralChange);
        }

        public static int InsertUserSign(UserSign us)
        {
            return new UserSignDao().InsertUserSign(us);
        }

        public static bool IsSign(int userID)
        {
            DataTable table = SignInfoByUser(userID);
            if (table.Rows.Count >= 1)
            {
                DateTime time = Convert.ToDateTime(table.Rows[0]["SignDay"]);
                if (DateTime.Now.ToString("yyyyMMdd") == time.ToString("yyyyMMdd"))
                {
                    return false;
                }
            }
            return true;
        }

        public static int MaxContinued(DateTime t1, DateTime t2)
        {
            TimeSpan span = (TimeSpan) (t2 - t1);
            return span.Days;
        }

        public static DataTable SignInfoByUser(int userID)
        {
            return new UserSignDao().SignInfoByUser(userID);
        }

        public static int UpdateUserSign(UserSign us)
        {
            return new UserSignDao().UpdateUserSign(us);
        }

        public static int USign(int userID)
        {
            DataTable table = SignInfoByUser(userID);
            UserSign us = new UserSign();
            if (table.Rows.Count < 1)
            {
                us.UserID = userID;
                us.Continued = 1;
                InsertUserSign(us);
            }
            else
            {
                us.ID = Convert.ToInt32(table.Rows[0]["ID"]);
                us.SignDay = DateTime.Now;
                us.UserID = Convert.ToInt32(table.Rows[0]["UserID"]);
                us.Continued = Convert.ToInt32(table.Rows[0]["Continued"]);
                int num = MaxContinued(Convert.ToDateTime(table.Rows[0]["SignDay"]).Date, us.SignDay.Date);
                if (num <= 0)
                {
                    return -1;
                }
                if (num == 1)
                {
                    us.Continued++;
                }
                else if (num > 1)
                {
                    us.Continued = 1;
                }
            }
            int num2 = AddPoint(us);
            UpdateUserSign(us);
            return num2;
        }
    }
}

