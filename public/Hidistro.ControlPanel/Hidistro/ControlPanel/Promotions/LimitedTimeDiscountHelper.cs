namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;

    public class LimitedTimeDiscountHelper
    {
        private static LimitedTimeDiscountDao _act = new LimitedTimeDiscountDao();

        public static int AddLimitedTimeDiscount(LimitedTimeDiscountInfo info)
        {
            return _act.AddLimitedTimeDiscount(info);
        }

        public static bool AddLimitedTimeDiscountProduct(LimitedTimeDiscountProductInfo info)
        {
            return _act.AddLimitedTimeDiscountProduct(info);
        }

        public static bool ChangeDiscountProductStatus(string ids, int status)
        {
            return _act.ChangeDiscountProductStatus(ids, status);
        }

        public static bool DeleteDiscountProduct(string ids)
        {
            return _act.DeleteDiscountProduct(ids);
        }

        public static LimitedTimeDiscountInfo GetDiscountInfo(int Id)
        {
            return _act.GetDiscountInfo(Id);
        }

        public static DbQueryResult GetDiscountProduct(ProductQuery query)
        {
            return _act.GetDiscountProduct(query);
        }

        public static DbQueryResult GetDiscountProducted(ProductQuery query, int discountId)
        {
            return _act.GetDiscountProducted(query, discountId);
        }

        public static LimitedTimeDiscountProductInfo GetDiscountProductInfoById(int id)
        {
            return _act.GetDiscountProductInfoById(id);
        }

        public static LimitedTimeDiscountProductInfo GetDiscountProductInfoByProductId(int productId)
        {
            return _act.GetDiscountProductInfoByProductId(productId);
        }

        public static DbQueryResult GetDiscountQuery(ActivitySearch query)
        {
            return _act.GetDiscountQuery(query);
        }

        public static bool UpdateDiscountStatus(int Id, DiscountStatus status)
        {
            return _act.UpdateDiscountStatus(Id, status);
        }

        public static bool UpdateLimitedTimeDiscount(LimitedTimeDiscountInfo info)
        {
            return _act.UpdateLimitedTimeDiscount(info);
        }

        public static bool UpdateLimitedTimeDiscountProduct(LimitedTimeDiscountProductInfo info)
        {
            return _act.UpdateLimitedTimeDiscountProduct(info);
        }

        public static bool UpdateLimitedTimeDiscountProductById(LimitedTimeDiscountProductInfo info)
        {
            return _act.UpdateLimitedTimeDiscountProductById(info);
        }
    }
}

