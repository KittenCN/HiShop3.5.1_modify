namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web;

    public static class VshopBrowser
    {
        public static Hidistro.Entities.VShop.ActivityInfo GetActivity(int activityId)
        {
            return new Hidistro.SqlDal.VShop.ActivityDao().GetActivity(activityId);
        }

        public static IList<BannerInfo> GetAllBanners()
        {
            return new BannerDao().GetAllBanners();
        }

        public static IList<NavigateInfo> GetAllNavigate()
        {
            return new BannerDao().GetAllNavigate();
        }

        public static DataTable GetHomeProducts()
        {
            return new HomeProductDao().GetHomeProducts();
        }

        public static string GetLimitedTimeDiscountName(int limitedTimeDiscountId)
        {
            string activityName = string.Empty;
            LimitedTimeDiscountInfo discountInfo = new LimitedTimeDiscountDao().GetDiscountInfo(limitedTimeDiscountId);
            if (discountInfo != null)
            {
                activityName = discountInfo.ActivityName;
            }
            return activityName;
        }

        public static string GetLimitedTimeDiscountNameStr(int limitedTimeDiscountId)
        {
            string limitedTimeDiscountName = GetLimitedTimeDiscountName(limitedTimeDiscountId);
            if (!string.IsNullOrEmpty(limitedTimeDiscountName))
            {
                limitedTimeDiscountName = "<span style='background-color: rgb(246, 187, 66); border-color: rgb(246, 187, 66); color: rgb(255, 255, 255);'>" + HttpContext.Current.Server.HtmlEncode(limitedTimeDiscountName) + "</span>";
            }
            return limitedTimeDiscountName;
        }

        public static MessageInfo GetMessage(int messageId)
        {
            return new ReplyDao().GetMessage(messageId);
        }

        public static DataTable GetVote(int voteId, out string voteName, out int checkNum, out int voteNum)
        {
            return new VoteDao().LoadVote(voteId, out voteName, out checkNum, out voteNum);
        }

        public static bool IsPassAutoToDistributor(MemberInfo cuser, bool isNeedToCheckAutoToDistributor = true)
        {
            bool enableMemberAutoToDistributor = false;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings.DistributorApplicationCondition)
            {
                decimal expenditure = cuser.Expenditure;
                int finishedOrderMoney = masterSettings.FinishedOrderMoney;
                if (finishedOrderMoney > 0)
                {
                    decimal num2 = 0M;
                    DataTable userOrderPaidWaitFinish = new OrderDao().GetUserOrderPaidWaitFinish(cuser.UserId);
                    decimal total = 0M;
                    OrderInfo orderInfo = null;
                    for (int i = 0; i < userOrderPaidWaitFinish.Rows.Count; i++)
                    {
                        orderInfo = new OrderDao().GetOrderInfo(userOrderPaidWaitFinish.Rows[i]["orderid"].ToString());
                        if (orderInfo != null)
                        {
                            total = orderInfo.GetTotal();
                            if (total > 0M)
                            {
                                num2 += total;
                            }
                        }
                    }
                    if ((cuser.Expenditure + num2) >= finishedOrderMoney)
                    {
                        enableMemberAutoToDistributor = true;
                    }
                }
                if (((!enableMemberAutoToDistributor && masterSettings.EnableDistributorApplicationCondition) && (!string.IsNullOrEmpty(masterSettings.DistributorProductsDate) && !string.IsNullOrEmpty(masterSettings.DistributorProducts))) && masterSettings.DistributorProductsDate.Contains("|"))
                {
                    DateTime result = new DateTime();
                    DateTime time2 = new DateTime();
                    bool flag2 = DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[0].ToString(), out result);
                    bool flag3 = DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[1].ToString(), out time2);
                    if (((flag2 && flag3) && ((DateTime.Now.CompareTo(result) >= 0) && (DateTime.Now.CompareTo(time2) < 0))) && MemberProcessor.CheckMemberIsBuyProds(cuser.UserId, masterSettings.DistributorProducts, new DateTime?(result), new DateTime?(time2)))
                    {
                        enableMemberAutoToDistributor = true;
                    }
                }
                if ((!enableMemberAutoToDistributor && (masterSettings.RechargeMoneyToDistributor > 0M)) && (MemberAmountProcessor.GetUserMaxAmountDetailed(cuser.UserId) >= masterSettings.RechargeMoneyToDistributor))
                {
                    enableMemberAutoToDistributor = true;
                }
            }
            else
            {
                enableMemberAutoToDistributor = true;
            }
            if (isNeedToCheckAutoToDistributor && enableMemberAutoToDistributor)
            {
                enableMemberAutoToDistributor = masterSettings.EnableMemberAutoToDistributor;
            }
            return enableMemberAutoToDistributor;
        }

        public static bool IsVote(int voteId)
        {
            return new VoteDao().IsVote(voteId);
        }

        public static bool Vote(int voteId, string itemIds)
        {
            return new VoteDao().Vote(voteId, itemIds);
        }
    }
}

