namespace ControlPanel.Promotions
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class ActivityHelper
    {
        private static ActivityDao _act = new ActivityDao();

        public static bool AddActivitiesMember(int ActivitiesId, int Userid)
        {
            return _act.AddActivitiesMember(ActivitiesId, Userid, null);
        }

        public static bool AddProducts(int couponId, int productID)
        {
            return _act.AddProducts(couponId, productID);
        }

        public static bool AddProducts(int couponId, bool IsAllProduct, IList<string> productIDs)
        {
            return _act.AddProducts(couponId, IsAllProduct, productIDs);
        }

        public static int Create(ActivityInfo act, ref string msg)
        {
            return _act.Create(act, ref msg);
        }

        public static bool Delete(int Id)
        {
            return _act.Delete(Id);
        }

        public static bool DeleteProducts(int couponId, string productIds)
        {
            return _act.DeleteProducts(couponId, productIds);
        }

        public static bool EndAct(int Aid)
        {
            return _act.EndAct(Aid);
        }

        public static ActivityInfo GetAct(int Id)
        {
            return _act.GetAct(Id);
        }

        public static ActivityInfo GetAct(string name)
        {
            return _act.GetAct(name);
        }

        public static DataTable GetActivities()
        {
            return _act.GetActivities();
        }

        public static DataTable GetActivities_Detail(int actId)
        {
            return _act.GetActivities_Detail(actId);
        }

        public static int GetActivitiesMember(int Userid, int ActivitiesId)
        {
            return _act.GetActivitiesMember(Userid, ActivitiesId);
        }

        public static DataTable GetActivitiesProducts(int actid, int ProductID)
        {
            return _act.GetActivitiesProducts(actid, ProductID);
        }

        public static DataTable GetActivityTopics(string types = "0")
        {
            return _act.GetActivityTopics(types);
        }

        public static int GetActivityTopicsNum(string types = "0")
        {
            return _act.GetActivityTopicsNum(types);
        }

        public static int GetHishop_Activities(int Activities_DetailID)
        {
            return _act.GetHishop_Activities(Activities_DetailID);
        }

        public static bool HasPartProductAct()
        {
            return _act.HasPartProductAct();
        }

        public static DbQueryResult Query(ActivitySearch query)
        {
            return _act.Query(query);
        }

        public static DataTable QueryProducts()
        {
            return _act.QueryProducts();
        }

        public static DataTable QueryProducts(int actid)
        {
            return _act.QueryProducts(actid);
        }

        public static DataTable QuerySelProducts()
        {
            return _act.QuerySelProducts();
        }

        public static bool SetProductsStatus(int couponId, int status, string productIds)
        {
            return _act.SetProductsStatus(couponId, status, productIds);
        }

        public static bool Update(ActivityInfo act, ref string msg)
        {
            return _act.Update(act, ref msg);
        }
    }
}

