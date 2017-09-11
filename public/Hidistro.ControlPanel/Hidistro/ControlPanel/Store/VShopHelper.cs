namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.FenXiao;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public static class VShopHelper
    {
        private const string CacheKey = "Message-{0}";

        public static int AddActivities(ActivitiesInfo activity)
        {
            return new ActivitiesDao().AddActivities(activity);
        }

        public static bool AddHomeProdcut(int productId)
        {
            return new HomeProductDao().AddHomeProdcut(productId);
        }

        public static bool CanAddFuwuMenu(int parentId)
        {
            IList<MenuInfo> fuwuMenusByParentId = new MenuDao().GetFuwuMenusByParentId(parentId);
            if ((fuwuMenusByParentId == null) || (fuwuMenusByParentId.Count == 0))
            {
                return true;
            }
            if (parentId == 0)
            {
                return (fuwuMenusByParentId.Count < 3);
            }
            return (fuwuMenusByParentId.Count < 5);
        }

        public static bool CanAddMenu(int parentId)
        {
            IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId);
            if ((menusByParentId == null) || (menusByParentId.Count == 0))
            {
                return true;
            }
            if (parentId == 0)
            {
                return (menusByParentId.Count < 3);
            }
            return (menusByParentId.Count < 5);
        }

        public static bool DeleteActivities(int ActivitiesId)
        {
            return new ActivitiesDao().DeleteActivities(ActivitiesId);
        }

        public static bool DeleteAdminUserMsgList(MsgList myList, out string RetInfo)
        {
            return new MessageTemplateHelperDao().DeleteAdminUserMsgList(myList, out RetInfo);
        }

        public static bool DeleteAlarm(int id)
        {
            return new AlarmDao().Delete(id);
        }

        public static bool DeleteCustomDistributorStatistic(string id)
        {
            return new DistributorsDao().DeleteCustomDistributorStatistic(id);
        }

        public static bool DeleteFeedBack(int id)
        {
            return new FeedBackDao().Delete(id);
        }

        public static bool DeleteFuwuMenu(int menuId)
        {
            return new MenuDao().DeleteFuwuMenu(menuId);
        }

        public static bool DeleteMenu(int menuId)
        {
            return new MenuDao().DeleteMenu(menuId);
        }

        public static bool DelTplCfg(int id)
        {
            return new BannerDao().DelTplCfg(id);
        }

        public static string GenerateOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        public static IList<ActivitiesInfo> GetActivitiesInfo(string ActivitiesId)
        {
            return new ActivitiesDao().GetActivitiesInfo(ActivitiesId);
        }

        public static DbQueryResult GetActivitiesList(ActivitiesQuery query)
        {
            return new ActivitiesDao().GetActivitiesList(query);
        }

        public static ActivityInfo GetActivity(int activityId)
        {
            return new ActivityDao().GetActivity(activityId);
        }

        public static IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
        {
            return new ActivitySignUpDao().GetActivitySignUpById(activityId);
        }

        public static DataTable GetAdminUserMsgDetail(bool IsDistributor)
        {
            return new MessageTemplateHelperDao().GetAdminUserMsgDetail(IsDistributor);
        }

        public static DataTable GetAdminUserMsgList()
        {
            return new MessageTemplateHelperDao().GetAdminUserMsgList(0);
        }

        public static DataTable GetAdminUserMsgList(int userType)
        {
            return new MessageTemplateHelperDao().GetAdminUserMsgList(userType);
        }

        public static DbQueryResult GetAlarms(int pageIndex, int pageSize)
        {
            return new AlarmDao().List(pageIndex, pageSize);
        }

        public static IList<MessageTemplate> GetAliFuWuMessageTemplates()
        {
            return new MessageTemplateHelperDao().GetAliFuWuMessageTemplates();
        }

        public static IList<ActivityInfo> GetAllActivity()
        {
            return new ActivityDao().GetAllActivity();
        }

        public static IList<BannerInfo> GetAllBanners()
        {
            return new BannerDao().GetAllBanners();
        }

        public static IList<NavigateInfo> GetAllNavigate()
        {
            return new BannerDao().GetAllNavigate();
        }

        public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            return new DistributorsDao().GetBalanceDrawRequest(query, null);
        }

        public static string GetCommissionPayStatus(string ischeck)
        {
            string str = "未定义";
            string str2 = ischeck;
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "0"))
            {
                if (str2 != "1")
                {
                    if (str2 != "2")
                    {
                        return str;
                    }
                    return "已支付";
                }
            }
            else
            {
                return "未审核";
            }
            return "已审核";
        }

        public static string GetCommissionPayType(string payType)
        {
            string str = "未定义";
            string str2 = payType;
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "0"))
            {
                if (str2 != "1")
                {
                    if (str2 == "2")
                    {
                        return "线下转帐";
                    }
                    if (str2 == "3")
                    {
                        return "微信红包";
                    }
                    if (str2 != "4")
                    {
                        return str;
                    }
                    return "转入余额";
                }
            }
            else
            {
                return "微信钱包";
            }
            return "支付宝";
        }

        public static DbQueryResult GetCommissions(CommissionsQuery query)
        {
            return new DistributorsDao().GetCommissions(query);
        }

        public static DbQueryResult GetCommissionsWithStoreName(CommissionsQuery query, string subLevel = "")
        {
            return new DistributorsDao().GetCommissionsWithStoreName(query, subLevel);
        }

        public static int GetCountBanner()
        {
            return new BannerDao().GetCountBanner();
        }

        public static DataTable GetCustomDistributorStatistic(int id)
        {
            return new DistributorsDao().GetCustomDistributorStatistic(id);
        }

        public static DataTable GetCustomDistributorStatistic(string storeName)
        {
            return new DistributorsDao().GetCustomDistributorStatistic(storeName);
        }

        public static DbQueryResult GetCustomDistributorStatisticList()
        {
            return new DistributorsDao().GetCustomDistributorStatisticList();
        }

        public static IList<DistributorGradeInfo> GetDistributorGradeInfos()
        {
            return new DistributorGradeDao().GetDistributorGradeInfos();
        }

        public static DbQueryResult GetDistributors(DistributorsQuery query, string topUserId = null, string level = null)
        {
            return new DistributorsDao().GetDistributors(query, topUserId, level);
        }

        public static DataTable GetDistributorSaleinfo(string startTime, string endTime, int[] UserIds)
        {
            return new DistributorsDao().GetDistributorSaleinfo(startTime, endTime, UserIds);
        }

        public static DataTable GetDistributorsNum()
        {
            return new DistributorsDao().GetDistributorsNum();
        }

        public static DbQueryResult GetDistributorsRankings(string startTime, string endTime, int pgSize, int CurrPage)
        {
            return new DistributorsDao().GetDistributorsRankings(startTime, endTime, pgSize, CurrPage);
        }

        public static DataTable GetDistributorsSubStoreNum(int topUserId)
        {
            return new DistributorsDao().GetDistributorsSubStoreNum(topUserId);
        }

        public static int GetDistributorsSubStoreNumN(int topUserId, int grade, string startTime, string endTime)
        {
            return new DistributorsDao().GetDistributorsSubStoreNumN(topUserId, grade, startTime, endTime);
        }

        public static int GetDownDistributorNum(string userid)
        {
            return new DistributorsDao().GetDownDistributorNum(userid);
        }

        public static int GetDownDistributorNumReferralOrders(string userid)
        {
            return new DistributorsDao().GetDownDistributorNumReferralOrders(userid);
        }

        public static FeedBackInfo GetFeedBack(int id)
        {
            return new FeedBackDao().Get(id);
        }

        public static FeedBackInfo GetFeedBack(string feedBackID)
        {
            return new FeedBackDao().Get(feedBackID);
        }

        public static DbQueryResult GetFeedBacks(int pageIndex, int pageSize, string msgType)
        {
            return new FeedBackDao().List(pageIndex, pageSize, msgType);
        }

        public static MenuInfo GetFuwuMenu(int menuId)
        {
            return new MenuDao().GetFuwuMenu(menuId);
        }

        public static IList<MenuInfo> GetFuwuMenusByParentId(int parentId)
        {
            return new MenuDao().GetFuwuMenusByParentId(parentId);
        }

        public static DataTable GetHomeProducts()
        {
            return new HomeProductDao().GetHomeProducts();
        }

        public static IList<MenuInfo> GetInitFuwuMenus()
        {
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topFuwuMenus = dao.GetTopFuwuMenus();
            foreach (MenuInfo info in topFuwuMenus)
            {
                info.Chilren = dao.GetFuwuMenusByParentId(info.MenuId);
                if (info.Chilren == null)
                {
                    info.Chilren = new List<MenuInfo>();
                }
            }
            return topFuwuMenus;
        }

        public static IList<MenuInfo> GetInitMenus()
        {
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topMenus = dao.GetTopMenus();
            foreach (MenuInfo info in topMenus)
            {
                info.Chilren = dao.GetMenusByParentId(info.MenuId);
                if (info.Chilren == null)
                {
                    info.Chilren = new List<MenuInfo>();
                }
            }
            return topMenus;
        }

        public static MenuInfo GetMenu(int menuId)
        {
            return new MenuDao().GetMenu(menuId);
        }

        public static IList<MenuInfo> GetMenus()
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topMenus = dao.GetTopMenus();
            if (topMenus != null)
            {
                foreach (MenuInfo info in topMenus)
                {
                    list.Add(info);
                    IList<MenuInfo> menusByParentId = dao.GetMenusByParentId(info.MenuId);
                    if (menusByParentId != null)
                    {
                        foreach (MenuInfo info2 in menusByParentId)
                        {
                            list.Add(info2);
                        }
                    }
                }
            }
            return list;
        }

        public static IList<MenuInfo> GetMenusByParentId(int parentId)
        {
            return new MenuDao().GetMenusByParentId(parentId);
        }

        public static MessageTemplate GetMessageTemplate(string messageType)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                return null;
            }
            return new MessageTemplateHelperDao().GetMessageTemplate(messageType);
        }

        public static IList<MessageTemplate> GetMessageTemplates()
        {
            return new MessageTemplateHelperDao().GetMessageTemplates();
        }

        public static DbQueryResult GetSubDistributorsContribute(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
        {
            return new DistributorsDao().GetSubDistributorsContribute(startTime, endTime, pgSize, CurrPage, belongUserId, grade);
        }

        public static DbQueryResult GetSubDistributorsRankingsN(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
        {
            return new DistributorsDao().GetSubDistributorsRankingsN(startTime, endTime, pgSize, CurrPage, belongUserId, grade);
        }

        public static IList<MenuInfo> GetTopFuwuMenus()
        {
            return new MenuDao().GetTopFuwuMenus();
        }

        public static IList<MenuInfo> GetTopMenus()
        {
            return new MenuDao().GetTopMenus();
        }

        public static TplCfgInfo GetTplCfgById(int id)
        {
            return new BannerDao().GetTplCfgById(id);
        }

        public static DataTable GetType(int Types)
        {
            return new ActivitiesDao().GetType(Types);
        }

        public static DistributorsInfo GetUserIdDistributors(int userid)
        {
            return new DistributorsDao().GetDistributorInfo(userid);
        }

        public static bool InsertCustomDistributorStatistic(CustomDistributorStatistic custom)
        {
            return new DistributorsDao().InsertCustomDistributorStatistic(custom);
        }

        public static int IsExistUsers(string userIds)
        {
            return new DistributorsDao().IsExistUsers(userIds);
        }

        public static bool RemoveAllHomeProduct()
        {
            return new HomeProductDao().RemoveAllHomeProduct();
        }

        public static bool RemoveHomeProduct(int productId)
        {
            return new HomeProductDao().RemoveHomeProduct(productId);
        }

        public static bool SaveActivity1(ActivityInfo activity)
        {
            int num = new ActivityDao().SaveActivity(activity);
            ReplyInfo reply = new TextReplyInfo {
                Keys = activity.Keys,
                MatchType = MatchType.Equal,
                MessageType = MessageType.Text,
                ReplyType = ReplyType.SignUp,
                ActivityId = num
            };
            return new ReplyDao().SaveReply(reply);
        }

        public static bool SaveAdminUserMsgList(bool IsInsert, MsgList myList, string OldUserOpenIdIfUpdate, out string RetInfo)
        {
            return new MessageTemplateHelperDao().SaveAdminUserMsgList(IsInsert, myList, OldUserOpenIdIfUpdate, out RetInfo);
        }

        public static bool SaveAlarm(AlarmInfo info)
        {
            return new AlarmDao().Save(info);
        }

        public static bool SaveFeedBack(FeedBackInfo info)
        {
            return new FeedBackDao().Save(info);
        }

        public static bool SaveFuwuMenu(MenuInfo menu)
        {
            return new MenuDao().SaveFuwuMenu(menu);
        }

        public static bool SaveMenu(MenuInfo menu)
        {
            return new MenuDao().SaveMenu(menu);
        }

        public static bool SaveTplCfg(TplCfgInfo info)
        {
            return new BannerDao().SaveTplCfg(info);
        }

        public static void SwapMenuSequence(int menuId, bool isUp)
        {
            new MenuDao().SwapMenuSequence(menuId, isUp);
        }

        public static void SwapTplCfgSequence(int bannerId, int replaceBannerId)
        {
            BannerDao dao = new BannerDao();
            TplCfgInfo tplCfgById = dao.GetTplCfgById(bannerId);
            TplCfgInfo info = dao.GetTplCfgById(replaceBannerId);
            if ((tplCfgById != null) && (info != null))
            {
                int displaySequence = tplCfgById.DisplaySequence;
                tplCfgById.DisplaySequence = info.DisplaySequence;
                info.DisplaySequence = displaySequence;
                dao.UpdateTplCfg(tplCfgById);
                dao.UpdateTplCfg(info);
            }
        }

        public static bool UpdateActivities(ActivitiesInfo activity)
        {
            return new ActivitiesDao().UpdateActivities(activity);
        }

        public static void UpdateAliFuWuSettings(IList<MessageTemplate> templates)
        {
            if ((templates != null) && (templates.Count != 0))
            {
                new MessageTemplateHelperDao().UpdateAliFuWuSettings(templates);
                foreach (MessageTemplate template in templates)
                {
                    HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
                }
            }
        }

        public static bool UpdateBalanceDistributors(int UserId, decimal ReferralRequestBalance)
        {
            return new DistributorsDao().UpdateBalanceDistributors(UserId, ReferralRequestBalance);
        }

        public static bool UpdateBalanceDrawRequest(int id, string Remark)
        {
            return new DistributorsDao().UpdateBalanceDrawRequest(id, Remark, null);
        }

        public static bool UpdateCommission(int UserId, decimal Commission, string CommRemark)
        {
            DistributorGradeCommissionInfo info = new DistributorGradeCommissionInfo {
                UserId = UserId,
                Commission = Commission,
                PubTime = DateTime.Now,
                OperAdmin = "system",
                Memo = CommRemark,
                OldCommissionTotal = 0M,
                ReferralUserId = UserId,
                OrderID = "A" + GenerateOrderId(),
                CommType = 5
            };
            return new DistributorsDao().AddCommission(info);
        }

        public static bool UpdateCustomDistributorStatistic(CustomDistributorStatistic custom)
        {
            return new DistributorsDao().UpdateCustomDistributorStatistic(custom);
        }

        public static bool UpdateFeedBackMsgType(string feedBackId, string msgType)
        {
            return new FeedBackDao().UpdateMsgType(feedBackId, msgType);
        }

        public static bool UpdateFuwuMenu(MenuInfo menu)
        {
            return new MenuDao().UpdateFuwuMenu(menu);
        }

        public static bool UpdateHomeProductSequence(int ProductId, int displaysequence)
        {
            return new HomeProductDao().UpdateHomeProductSequence(ProductId, displaysequence);
        }

        public static bool UpdateMenu(MenuInfo menu)
        {
            return new MenuDao().UpdateMenu(menu);
        }

        public static void UpdateSettings(IList<MessageTemplate> templates)
        {
            if ((templates != null) && (templates.Count != 0))
            {
                new MessageTemplateHelperDao().UpdateSettings(templates);
                foreach (MessageTemplate template in templates)
                {
                    HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
                }
            }
        }

        public static void UpdateTemplate(MessageTemplate template)
        {
            if (template != null)
            {
                new MessageTemplateHelperDao().UpdateTemplate(template);
                HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
            }
        }

        public static bool UpdateTplCfg(TplCfgInfo info)
        {
            return new BannerDao().UpdateTplCfg(info);
        }

        public static void UpdateWeiXinMsgDetail(bool IsDistributor, IList<MsgDetail> templates)
        {
            new MessageTemplateHelperDao().UpdateWeiXinMsgDetail(IsDistributor, templates);
        }
    }
}

