namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Promotions;
    using System;

    public static class CouponProcessor
    {
        public static void RegisterSendCoupon(string sessionId)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings.IsRegisterSendCoupon)
            {
                DateTime now = DateTime.Now;
                if ((!masterSettings.RegisterSendCouponBeginTime.HasValue || (masterSettings.RegisterSendCouponBeginTime.Value <= now)) && (!masterSettings.RegisterSendCouponEndTime.HasValue || (masterSettings.RegisterSendCouponEndTime.Value >= now)))
                {
                    MemberInfo member = MemberProcessor.GetMember(sessionId);
                    if (member != null)
                    {
                        new CouponDao().SendCouponToMember(masterSettings.RegisterSendCouponId, member.UserId);
                    }
                }
            }
        }

        public static SendCouponResult SendCouponToMember(int couponId, int userId)
        {
            return new CouponDao().SendCouponToMember(couponId, userId);
        }
    }
}

