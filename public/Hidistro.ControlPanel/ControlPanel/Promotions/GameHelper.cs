namespace ControlPanel.Promotions
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Messages;
    using Hidistro.SqlDal.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.InteropServices;

    public static class GameHelper
    {
        private static Random Rnd = new Random();

        public static bool AddPrizeLog(PrizeResultInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("参数model不能不null");
            }
            return new PrizeResultDao().AddPrizeLog(model);
        }

        private static GamePrizeInfo ChouJiang(IList<GamePrizeInfo> prizeLists)
        {
            return (from x in Enumerable.Range(0, 0x2710)
                let prizeInfo0 = prizeLists[Rnd.Next(prizeLists.Count<GamePrizeInfo>())]
                let rnd = Rnd.Next(0, 100)
                where rnd < prizeInfo0.PrizeRate
                select prizeInfo0).First<GamePrizeInfo>();
        }

        public static bool Create(GameInfo model, out int gameId)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            Globals.EntityCoding(model, true);
            return new GameDao().Create(model, out gameId);
        }

        public static bool CreatePrize(GamePrizeInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            return new GamePrizeDao().Create(model);
        }

        public static bool CreateWinningPool(GameWinningPoolInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            return new GamePrizeDao().CreateWinningPool(model);
        }

        public static void CreateWinningPools(float PrizeRate, int prizeCount, int gameId)
        {
            List<int> pool = GetPool(GetGamePrizeListsByGameId(gameId).ToList<GamePrizeInfo>());
            int num = (int) (((float) prizeCount) / (PrizeRate / 100f));
            float num2 = ((float) prizeCount) % (PrizeRate / 100f);
            for (int i = 1; i < (num + 1); i++)
            {
                GameWinningPoolInfo model = new GameWinningPoolInfo {
                    GameId = gameId,
                    Number = i,
                    GamePrizeId = (i < (prizeCount + 1)) ? pool[i - 1] : 0,
                    IsReceive = 0
                };
                CreateWinningPool(model);
            }
            if (num2 > 0f)
            {
                GameWinningPoolInfo info2 = new GameWinningPoolInfo {
                    GameId = gameId,
                    Number = num + 1,
                    GamePrizeId = 0,
                    IsReceive = 0
                };
                CreateWinningPool(info2);
            }
        }

        public static bool Delete(params int[] gameIds)
        {
            if ((gameIds == null) || (gameIds.Count<int>() <= 0))
            {
                throw new ArgumentNullException("参数gameIds不能为空！");
            }
            return new GameDao().Delete(gameIds);
        }

        public static bool DeletePrizesDelivery(int[] ids)
        {
            return new GameDao().DeletePrizesDelivery(ids);
        }

        public static bool DeletePromotionGamePrize(GamePrizeInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            return new GamePrizeDao().Delete(model);
        }

        public static bool DeleteWinningPools(int gameId)
        {
            return new GamePrizeDao().DeleteWinningPools(gameId);
        }

        public static DbQueryResult GetAllPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
        {
            return new GameDao().GetAllPrizesDeliveryList(query, ExtendLimits, selectFields);
        }

        public static GameInfo GetGameInfoById(int gameId)
        {
            return new GameDao().GetGameInfoById(gameId);
        }

        public static DbQueryResult GetGameList(GameSearch search)
        {
            return new GameDao().GetGameList(search);
        }

        public static DbQueryResult GetGameListByView(GameSearch search)
        {
            return new GameDao().GetGameListByView(search);
        }

        public static GamePrizeInfo GetGamePrizeInfoById(int prizeId)
        {
            if (prizeId < 0)
            {
                throw new ArgumentNullException("参数错误");
            }
            return new GamePrizeDao().GetGamePrizeInfoById(prizeId);
        }

        public static IList<GamePrizeInfo> GetGamePrizeListsByGameId(int gameId)
        {
            if (gameId <= 0)
            {
                return null;
            }
            return new GamePrizeDao().GetGamePrizeListsByGameId(gameId);
        }

        public static string GetGameTypeName(string GameType)
        {
            return Enum.GetName(typeof(GameType), int.Parse(GameType));
        }

        public static IEnumerable<GameInfo> GetLists(GameSearch search)
        {
            return new GameDao().GetLists(search);
        }

        public static GameInfo GetModelByGameId(int gameId)
        {
            GameInfo modelByGameId = new GameDao().GetModelByGameId(gameId);
            Globals.EntityCoding(modelByGameId, false);
            return modelByGameId;
        }

        public static GameInfo GetModelByGameId(string keyWord)
        {
            GameInfo modelByGameId = new GameDao().GetModelByGameId(keyWord);
            Globals.EntityCoding(modelByGameId, false);
            return modelByGameId;
        }

        public static GamePrizeInfo GetModelByPrizeGradeAndGameId(PrizeGrade grade, int gameId)
        {
            return new GamePrizeDao().GetModelByPrizeGradeAndGameId(grade, gameId);
        }

        public static int GetOppNumber(int userId, int gameId)
        {
            return new GamePrizeDao().GetOppNumber(userId, gameId);
        }

        public static int GetOppNumberByToday(int userId, int gameId)
        {
            return new GamePrizeDao().GetOppNumberByToday(userId, gameId);
        }

        public static DataSet GetOrdersAndLines(string orderIds, string pIds)
        {
            return new GameDao().GetOrdersAndLines(orderIds, pIds);
        }

        private static List<int> GetPool(List<GamePrizeInfo> list)
        {
            List<int> list2 = new List<int>();
            if ((list != null) && (list.Count > 0))
            {
                foreach (GamePrizeInfo info in list)
                {
                    if ((info != null) && (info.PrizeCount > 0))
                    {
                        for (int i = 0; i < info.PrizeCount; i++)
                        {
                            list2.Add(info.PrizeId);
                        }
                    }
                }
            }
            return list2;
        }

        public static string GetPrizeFullName(PrizeResultViewInfo item)
        {
            switch (item.PrizeType)
            {
                case PrizeType.赠送积分:
                    return string.Format("{0} 积分", item.GivePoint);

                case PrizeType.赠送优惠券:
                {
                    CouponInfo coupon = CouponHelper.GetCoupon(int.Parse(item.GiveCouponId));
                    if (coupon == null)
                    {
                        return ("优惠券" + item.GiveCouponId + "[已删除]");
                    }
                    return coupon.CouponName;
                }
                case PrizeType.赠送商品:
                {
                    ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(int.Parse(item.GiveShopBookId));
                    if (productBaseInfo == null)
                    {
                        return "赠送商品[已删除]";
                    }
                    return productBaseInfo.ProductName;
                }
            }
            return "";
        }

        public static string GetPrizeGradeName(string PrizeGrade)
        {
            return Enum.GetName(typeof(PrizeGrade), int.Parse(PrizeGrade));
        }

        public static DataSet GetPrizeListByLogIDList(string orderlist, string pidlist)
        {
            return new GameDao().GetPrizeListByLogIDList(orderlist, pidlist);
        }

        public static DbQueryResult GetPrizeLogLists(PrizesDeliveQuery query)
        {
            return new PrizeResultDao().GetPrizeLogLists(query);
        }

        public static IList<PrizeResultViewInfo> GetPrizeLogLists(int gameId, int pageIndex, int pageSize)
        {
            return new PrizeResultDao().GetPrizeLogLists(gameId, pageIndex, pageSize);
        }

        public static string GetPrizeName(PrizeType PrizeType, string FullName)
        {
            switch (PrizeType)
            {
                case PrizeType.赠送优惠券:
                case PrizeType.赠送商品:
                    return Globals.SubStr(FullName, 12, "..");
            }
            return FullName;
        }

        public static DbQueryResult GetPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
        {
            return new GameDao().GetPrizesDeliveryList(query, ExtendLimits, selectFields);
        }

        public static DataTable GetPrizesDeliveryNum()
        {
            return new GameDao().GetPrizesDeliveryNum();
        }

        public static string GetPrizesDeliveStatus(string status, string isLogistics, string type, string gametype)
        {
            string str = "未定义";
            string str2 = status;
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "0"))
            {
                if (str2 == "1")
                {
                    return "待发货";
                }
                if (str2 == "2")
                {
                    return "已发货";
                }
                if (str2 == "3")
                {
                    return "已收货";
                }
                if (str2 != "4")
                {
                    return str;
                }
            }
            else
            {
                if (((type != "2") && (gametype != "5")) && (isLogistics == "0"))
                {
                    return "已收货";
                }
                return "待填写收货地址";
            }
            return "已收货";
        }

        public static List<GameWinningPool> GetWinningPoolList(int gameId)
        {
            return new GameDao().GetWinningPoolList(gameId);
        }

        public static bool IsCanPrize(int gameId, int userid)
        {
            return new PrizeResultDao().IsCanPrize(gameId, userid);
        }

        public static bool SetPrintOrderExpress(string orderId, string pid, string expressCompanyName, string expressCompanyAbb, string shipOrderNumber)
        {
            return new GameDao().SetPrintOrderExpress(orderId, pid, expressCompanyName, expressCompanyAbb, shipOrderNumber);
        }

        public static bool Update(GameInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            Globals.EntityCoding(model, true);
            return new GameDao().Update(model, true);
        }

        public static bool UpdateOneyuanDelivery(PrizesDeliveQuery query)
        {
            return new GameDao().UpdatePrizesDelivery(query);
        }

        public static bool UpdateOutOfDateStatus()
        {
            return new GameDao().UpdateOutOfDateStatus();
        }

        public static bool UpdatePrize(GamePrizeInfo model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model参数不能为null");
            }
            return new GamePrizeDao().Update(model);
        }

        public static bool UpdatePrizeLogStatus(int logId)
        {
            return new PrizeResultDao().UpdatePrizeLogStatus(logId);
        }

        public static bool UpdatePrizesDelivery(PrizesDeliveQuery query)
        {
            bool flag = new GameDao().UpdatePrizesDelivery(query);
            if (flag)
            {
                string gameTitle = "";
                int userId = 0;
                string prizeName = "";
                new GameDao().GetPrizesDeliveInfo(query, out gameTitle, out userId, out prizeName);
                try
                {
                    MemberInfo member = MemberHelper.GetMember(userId);
                    if (member != null)
                    {
                        Messenger.SendWeiXinMsg_PrizeRelease(member, gameTitle, prizeName);
                    }
                }
                catch (Exception)
                {
                }
            }
            return flag;
        }

        public static bool UpdateStatus(int gameId, GameStatus status)
        {
            return new GameDao().UpdateStatus(gameId, status);
        }

        public static bool UpdateWinningPoolIsReceive(int winningPoolId)
        {
            return new GameDao().UpdateWinningPoolIsReceive(winningPoolId);
        }

        public static GamePrizeInfo UserPrize(int gameId, int useId)
        {
            IList<GamePrizeInfo> gamePrizeListsByGameId = GetGamePrizeListsByGameId(gameId);
            int num = gamePrizeListsByGameId.Max<GamePrizeInfo>((Func<GamePrizeInfo, int>) (p => p.PrizeRate));
            GamePrizeInfo item = new GamePrizeInfo {
                PrizeId = 0,
                PrizeRate = (num >= 100) ? 0 : 100,
                PrizeGrade = PrizeGrade.未中奖
            };
            gamePrizeListsByGameId.Add(item);
            GamePrizeInfo info2 = ChouJiang(gamePrizeListsByGameId);
            if ((info2.PrizeId != 0) && (info2.PrizeCount <= 0))
            {
                info2 = item;
            }
            if (((info2.PrizeId != 0) && (info2.PrizeType == PrizeType.赠送优惠券)) && (CouponHelper.IsCanSendCouponToMember(int.Parse(info2.GiveCouponId), useId) != SendCouponResult.正常领取))
            {
                info2 = item;
            }
            PrizeResultInfo model = new PrizeResultInfo {
                GameId = gameId,
                PrizeId = info2.PrizeId,
                UserId = useId
            };
            new PrizeResultDao().AddPrizeLog(model);
            return info2;
        }
    }
}

