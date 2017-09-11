﻿namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class CouponHelper
    {
        public static bool AddCouponProducts(int couponId, int productID)
        {
            return new CouponDao().AddCouponProducts(couponId, productID);
        }

        public static bool AddCouponProducts(int couponId, bool IsAllProduct, IList<string> productIDs)
        {
            return new CouponDao().AddCouponProducts(couponId, IsAllProduct, productIDs);
        }

        public static bool CheckCouponsIsUsed(int MemberCouponsId)
        {
            return new CouponDao().CheckCouponsIsUsed(MemberCouponsId);
        }

        public static CouponActionStatus CreateCoupon(CouponInfo coupon)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().CreateCoupon(coupon);
        }

        public static CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            Globals.EntityCoding(coupon, true);
            lotNumber = "";
            return new CouponDao().CreateCoupon(coupon);
        }

        public static bool DeleteCoupon(int couponId)
        {
            return new CouponDao().DeleteCoupon(couponId);
        }

        public static bool DeleteProducts(int couponId, string productIds)
        {
            return new CouponDao().DeleteProducts(couponId, productIds);
        }

        public static CouponInfo GetCoupon(int couponId)
        {
            return new CouponDao().GetCouponDetails(couponId);
        }

        public static CouponInfo GetCoupon(string couponName)
        {
            return new CouponDao().GetCouponDetails(couponName);
        }

        public static DbQueryResult GetCouponInfos(CouponsSearch search)
        {
            return new CouponDao().GetCouponInfos(search);
        }

        public static IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            return null;
        }

        public static string GetCouponProductIds(int couponId)
        {
            DataTable couponProducts = GetCouponProducts(couponId);
            StringBuilder builder = new StringBuilder();
            if (couponProducts != null)
            {
                int count = couponProducts.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    builder.Append(couponProducts.Rows[i]["ProductId"].ToString());
                    if (i != (count - 1))
                    {
                        builder.Append("_");
                    }
                }
            }
            return builder.ToString();
        }

        public static DataTable GetCouponProducts(int couponId)
        {
            return new CouponDao().GetCouponProducts(couponId);
        }

        public static DbQueryResult GetCouponsList(CouponItemInfoQuery query)
        {
            return new CouponDao().GetCouponsList(query);
        }

        public static DataTable GetCouponsListByIds(int[] CouponId)
        {
            return new CouponDao().GetCouponsListByIds(CouponId);
        }

        public static string GetCouponsProductIds(int CouponId)
        {
            return new CouponDao().GetCouponsProductIds(CouponId);
        }

        public static string GetCouponsProductIdsByMemberCouponIDByRedPagerId(int redPagerId)
        {
            return new CouponDao().GetCouponsProductIdsByMemberCouponIDByRedPagerId(redPagerId);
        }

        public static DataTable GetMemberCoupons(MemberCouponsSearch search, ref int total)
        {
            return new CouponDao().GetMemberCoupons(search, ref total);
        }

        public static int GetMemberCouponsNumbyUserId(int UserId)
        {
            return new CouponDao().GetMemberCouponsNumbyUserId(UserId);
        }

        public static int GetMemeberNumBySearch(string gradeIds, string referralUserId, string beginCreateDate, string endCreateDate, int userType, string customGroup)
        {
            int currentManagerUserId = Globals.GetCurrentManagerUserId();
            return new MemberDao().GetMemeberNumBySearch(gradeIds, referralUserId, beginCreateDate, endCreateDate, userType, customGroup, currentManagerUserId);
        }

        public static DbQueryResult GetNewCoupons(Pagination page)
        {
            return new CouponDao().GetNewCoupons(page);
        }

        public static DataTable GetUnFinishedCoupon(DateTime end, CouponType? type = new CouponType?())
        {
            return new CouponDao().GetUnFinishedCoupon(end, type);
        }

        public static SendCouponResult IsCanSendCouponToMember(int couponId, int userId)
        {
            CouponInfo couponDetails = new CouponDao().GetCouponDetails(couponId);
            if (!MemberProcessor.CheckCurrentMemberIsInRange(couponDetails.MemberGrades, couponDetails.DefualtGroup, couponDetails.CustomGroup))
            {
                return SendCouponResult.会员不在此活动范内;
            }
            return new CouponDao().IsCanSendCouponToMember(couponId, userId);
        }

        public static bool SaveWeiXinPromptInfo(CouponInfo_MemberWeiXin info)
        {
            return new CouponDao().SaveWeiXinPromptInfo(info);
        }

        public static bool SelectCouponWillExpiredList(int DayLimit, ref List<CouponInfo_MemberWeiXin> SendToUserList)
        {
            return new CouponDao().SelectCouponWillExpiredList(DayLimit, ref SendToUserList);
        }

        public static void SendClaimCodes(int couponId, IList<CouponItemInfo> listCouponItem)
        {
            foreach (CouponItemInfo info in listCouponItem)
            {
                new CouponDao().SendClaimCodes(couponId, info);
            }
        }

        public static SendCouponResult SendCouponToMember(int couponId, int userId)
        {
            CouponInfo couponDetails = new CouponDao().GetCouponDetails(couponId);
            if (!MemberProcessor.CheckCurrentMemberIsInRange(couponDetails.MemberGrades, couponDetails.DefualtGroup, couponDetails.CustomGroup))
            {
                return SendCouponResult.会员不在此活动范内;
            }
            return new CouponDao().SendCouponToMember(couponId, userId);
        }

        public static bool SendCouponToMemebers(int couponId)
        {
            int currentManagerUserId = Globals.GetCurrentManagerUserId();
            return new CouponDao().SendCouponToMemebers(couponId, currentManagerUserId);
        }

        public static bool SendCouponToMemebers(int couponId, string userIds)
        {
            if (string.IsNullOrWhiteSpace(userIds))
            {
                throw new ArgumentNullException("userIds不能为空");
            }
            string[] strArray = userIds.Split(new char[] { '_' });
            List<int> list = new List<int>();
            foreach (string str in strArray)
            {
                try
                {
                    list.Add(int.Parse(str));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return new CouponDao().SendCouponToMemebers(couponId, list);
        }

        public static bool setCouponFinished(int couponId, bool flag)
        {
            return new CouponDao().setCouponFinished(couponId, flag);
        }

        public static bool SetProductsStatus(int couponId, int status, string productIds)
        {
            return new CouponDao().SetProductsStatus(couponId, status, productIds);
        }

        public static CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            return CouponActionStatus.UnknowError;
        }

        public static string UpdateCoupon(int couponId, CouponEdit coupon, ref string msg)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().UpdateCoupon(couponId, coupon, ref msg);
        }
    }
}

