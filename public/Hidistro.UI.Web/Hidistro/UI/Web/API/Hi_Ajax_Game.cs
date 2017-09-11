namespace Hidistro.UI.Web.API
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_Game : IHttpHandler
    {
        private static object objLock = new object();

        private void CheckCanVote(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            int voteId = 0;
            try
            {
                voteId = int.Parse(context.Request["voteId"]);
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            VoteInfo vote = VoteHelper.GetVote((long) voteId);
            if (vote == null)
            {
                builder.Append("\"status\":\"2\",\"Desciption\":\"不存在该投票!\"}");
                context.Response.Write(builder.ToString());
            }
            else if (MemberProcessor.CheckCurrentMemberIsInRange(vote.MemberGrades, vote.DefualtGroup, vote.CustomGroup))
            {
                if (!VoteHelper.IsVote(voteId))
                {
                    builder.Append("\"status\":\"1\",\"Desciption\":\"可以投票!\"}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    builder.Append("\"status\":\"2\",\"Desciption\":\"已投过票!\"}");
                    context.Response.Write(builder.ToString());
                }
            }
            else
            {
                builder.Append("\"status\":\"2\",\"Desciption\":\"该投票不适应您的会员，谢谢!\"}");
                context.Response.Write(builder.ToString());
            }
        }

        private void CheckUserCanPlay(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                GameInfo modelByGameId = GameHelper.GetModelByGameId(str);
                if (modelByGameId == null)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"游戏不存在!\"}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    int userid = 0;
                    try
                    {
                        userid = MemberProcessor.GetCurrentMember().UserId;
                    }
                    catch (Exception)
                    {
                        userid = 1;
                    }
                    try
                    {
                        if (MemberProcessor.CheckCurrentMemberIsInRange(modelByGameId.ApplyMembers, modelByGameId.DefualtGroup, modelByGameId.CustomGroup))
                        {
                            if (!GameHelper.IsCanPrize(modelByGameId.GameId, userid))
                            {
                                throw new Exception("不能再玩！");
                            }
                            builder.Append("\"status\":\"1\",\"Desciption\":\"可以正常玩!\"}");
                            context.Response.Write(builder.ToString());
                        }
                        else
                        {
                            builder.Append("\"status\":\"0\",\"Desciption\":\"当前会员不在活动的适用会员范围内\"}");
                            context.Response.Write(builder.ToString());
                        }
                    }
                    catch (Exception exception)
                    {
                        builder.Append("\"status\":\"0\",\"Desciption\":\"" + exception.Message + "!\"}");
                        context.Response.Write(builder.ToString());
                    }
                }
            }
        }

        private void GetCouponToMember(HttpContext context)
        {
            StringBuilder builder = new StringBuilder("{");
            int couponId = 0;
            try
            {
                couponId = int.Parse(context.Request["couponId"]);
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            int userId = 0;
            try
            {
                userId = MemberProcessor.GetCurrentMember().UserId;
            }
            catch (Exception)
            {
                userId = 1;
            }
            try
            {
                SendCouponResult result = CouponHelper.SendCouponToMember(couponId, userId);
                switch (result)
                {
                    case SendCouponResult.正常领取:
                        builder.Append("\"status\":\"1\",\"Desciption\":\"领取成功!\"}");
                        context.Response.Write(builder.ToString());
                        return;

                    case SendCouponResult.其它错误:
                        throw new Exception();
                }
                builder.Append("\"status\":\"2\",\"Desciption\":\"" + result.ToString() + "!\"}");
                context.Response.Write(builder.ToString());
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"3\",\"Desciption\":\"领取失败!\"}");
                context.Response.Write(builder.ToString());
            }
        }

        private void GetOpportunity(HttpContext context)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                GameInfo modelByGameId = GameHelper.GetModelByGameId(str);
                if (DateTime.Now < modelByGameId.BeginTime)
                {
                    builder.Append("\"status\":\"ok\",\"opportunitynumber\":\"0\"}");
                    context.Response.Write(builder.ToString());
                }
                else if ((DateTime.Now > modelByGameId.EndTime) || (modelByGameId.Status == GameStatus.结束))
                {
                    builder.Append("\"status\":\"ok\",\"opportunitynumber\":\"0\"}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    try
                    {
                        if (MemberProcessor.CheckCurrentMemberIsInRange(modelByGameId.ApplyMembers, modelByGameId.DefualtGroup, modelByGameId.CustomGroup))
                        {
                            GameHelper.IsCanPrize(modelByGameId.GameId, currentMember.UserId);
                            int num = Globals.RequestFormNum("LimitEveryDay");
                            int num2 = Globals.RequestFormNum("MaximumDailyLimit");
                            if ((num == 0) && (num2 == 0))
                            {
                                builder.Append("\"status\":\"ok\",\"opportunitynumber\":\"-1\"}");
                            }
                            else
                            {
                                int num3 = 0;
                                int oppNumberByToday = GameHelper.GetOppNumberByToday(currentMember.UserId, modelByGameId.GameId);
                                int oppNumber = GameHelper.GetOppNumber(currentMember.UserId, modelByGameId.GameId);
                                int num6 = num - oppNumberByToday;
                                int num7 = num2 - oppNumber;
                                if (num2 == 0)
                                {
                                    num3 = num6;
                                }
                                else
                                {
                                    num3 = (num6 >= num7) ? num7 : num6;
                                }
                                if (num == 0)
                                {
                                    num3 = num7;
                                }
                                if (num3 < 0)
                                {
                                    num3 = 0;
                                }
                                builder.Append("\"status\":\"ok\",\"opportunitynumber\":\"" + num3 + "\"}");
                            }
                            context.Response.Write(builder.ToString());
                        }
                        else
                        {
                            builder.Append("\"status\":\"ok\",\"opportunitynumber\":\"0\"}");
                            context.Response.Write(builder.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        builder.Append("\"status\":\"ok\",\"opportunitynumber\":\"0\"}");
                        context.Response.Write(builder.ToString());
                    }
                }
            }
        }

        private void GetPrize(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                GameInfo modelByGameId = GameHelper.GetModelByGameId(str);
                if (modelByGameId == null)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                    context.Response.Write(builder.ToString());
                }
                else if (DateTime.Now < modelByGameId.BeginTime)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"活动还没开始!\"}");
                    context.Response.Write(builder.ToString());
                }
                else if ((DateTime.Now > modelByGameId.EndTime) || (modelByGameId.Status == GameStatus.结束))
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"活动已结束!\"}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    int userid = 0;
                    try
                    {
                        userid = MemberProcessor.GetCurrentMember().UserId;
                    }
                    catch (Exception)
                    {
                        builder.Append("\"status\":\"0\",\"Desciption\":\"请先登录!\"}");
                        context.Response.Write(builder.ToString());
                        return;
                    }
                    try
                    {
                        if (MemberProcessor.CheckCurrentMemberIsInRange(modelByGameId.ApplyMembers, modelByGameId.DefualtGroup, modelByGameId.CustomGroup))
                        {
                            GameHelper.IsCanPrize(modelByGameId.GameId, userid);
                        }
                        else
                        {
                            builder.Append("\"status\":\"0\",\"Desciption\":\"当前会员不在活动的适用会员范围内\"}");
                            context.Response.Write(builder.ToString());
                            return;
                        }
                    }
                    catch (Exception exception)
                    {
                        builder.Append("\"status\":\"0\",\"Desciption\":\"" + exception.Message + "!\"}");
                        context.Response.Write(builder.ToString());
                        return;
                    }
                    lock (objLock)
                    {
                        try
                        {
                            List<GameWinningPool> winningPoolList = GameHelper.GetWinningPoolList(Globals.ToNum(modelByGameId.GameId));
                            int maxValue = winningPoolList.Count<GameWinningPool>();
                            string prizeName = "";
                            if (maxValue > 0)
                            {
                                int num3 = new Random().Next(0, maxValue);
                                GameWinningPool pool = winningPoolList[num3];
                                if (pool != null)
                                {
                                    int prizeId = 0;
                                    if (pool.GamePrizeId > 0)
                                    {
                                        GamePrizeInfo gamePrizeInfoById = GameHelper.GetGamePrizeInfoById(pool.GamePrizeId);
                                        if (gamePrizeInfoById != null)
                                        {
                                            prizeId = gamePrizeInfoById.PrizeId;
                                            prizeName = gamePrizeInfoById.PrizeName;
                                        }
                                        builder.Append(string.Concat(new object[] { "\"status\":\"1\",\"Desciption\":\"\",\"prizeName\":\"", prizeName, "\",\"prizeState\":\"ok\",\"prizeId\":\"", pool.GamePrizeId, "\",\"prize\":\"", gamePrizeInfoById.Prize, "\",\"prizeGrade\":\"", this.GetPrizeName(gamePrizeInfoById.PrizeGrade), "\"}" }));
                                    }
                                    else
                                    {
                                        prizeName = modelByGameId.NotPrzeDescription;
                                        builder.Append("\"status\":\"1\",\"Desciption\":\"\",\"prizeName\":\"" + prizeName + "\",\"prizeState\":\"fail\",\"prizeId\":\"0\",\"prizeGrade\":\"0\"}");
                                    }
                                    PrizeResultInfo model = new PrizeResultInfo {
                                        GameId = modelByGameId.GameId,
                                        PrizeId = prizeId,
                                        UserId = userid
                                    };
                                    GameHelper.AddPrizeLog(model);
                                    GameHelper.UpdateWinningPoolIsReceive(pool.WinningPoolId);
                                }
                            }
                            else
                            {
                                builder.Append("\"status\":\"1\",\"Desciption\":\"\",\"prizeName\":\"" + (string.IsNullOrEmpty(modelByGameId.NotPrzeDescription) ? "谢谢参与！" : modelByGameId.NotPrzeDescription) + "\",\"prizeState\":\"fail\",\"prizeId\":\"0\",\"prizeGrade\":\"0\"}");
                                PrizeResultInfo info5 = new PrizeResultInfo {
                                    GameId = modelByGameId.GameId,
                                    PrizeId = 0,
                                    UserId = userid
                                };
                                GameHelper.AddPrizeLog(info5);
                                context.Response.Write(builder.ToString());
                                return;
                            }
                            context.Response.Write(builder.ToString());
                        }
                        catch (Exception exception2)
                        {
                            Globals.Debuglog(exception2.ToString(), "_GameDebuglog.txt");
                        }
                    }
                }
            }
        }

        private int GetPrizeIndex(PrizeGrade prizeGrade)
        {
            int num = (int) prizeGrade;
            if (prizeGrade != PrizeGrade.未中奖)
            {
                Random random = new Random();
                if ((random.Next(1, 10) % 2) == 0)
                {
                    return (num + 5);
                }
            }
            return num;
        }

        private void GetPrizeLists(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                GameInfo modelByGameId = GameHelper.GetModelByGameId(str);
                if (modelByGameId == null)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"游戏不存在!\"}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    GameData data = new GameData {
                        status = 1,
                        Description = modelByGameId.Description,
                        BeginDate = modelByGameId.BeginTime,
                        EndDate = modelByGameId.EndTime,
                        LimitEveryDay = modelByGameId.LimitEveryDay,
                        MaximumDailyLimit = modelByGameId.MaximumDailyLimit,
                        MemberCheck = modelByGameId.MemberCheck,
                        HasPhone = string.IsNullOrEmpty(currentMember.CellPhone) ? 0 : 1
                    };
                    IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(modelByGameId.GameId);
                    List<PrizeData> list2 = new List<PrizeData>();
                    string fullName = "";
                    GamePrizeInfo info3 = new GamePrizeInfo();
                    GamePrizeInfo info4 = new GamePrizeInfo();
                    GamePrizeInfo info5 = new GamePrizeInfo();
                    GamePrizeInfo info6 = new GamePrizeInfo();
                    switch (gamePrizeListsByGameId.Count<GamePrizeInfo>())
                    {
                        case 1:
                        {
                            info3 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖);
                            PrizeResultViewInfo item = new PrizeResultViewInfo {
                                PrizeType = info3.PrizeType,
                                GivePoint = info3.GivePoint,
                                GiveCouponId = info3.GiveCouponId,
                                GiveShopBookId = info3.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(item);
                            PrizeData data2 = new PrizeData {
                                prizeId = info3.PrizeId,
                                prize = info3.Prize,
                                prizeType = info3.PrizeGrade.ToString(),
                                prizeCount = info3.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info3.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data2);
                            break;
                        }
                        case 2:
                        {
                            info3 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖);
                            PrizeResultViewInfo info8 = new PrizeResultViewInfo {
                                PrizeType = info3.PrizeType,
                                GivePoint = info3.GivePoint,
                                GiveCouponId = info3.GiveCouponId,
                                GiveShopBookId = info3.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info8);
                            PrizeData data3 = new PrizeData {
                                prizeId = info3.PrizeId,
                                prize = info3.Prize,
                                prizeType = info3.PrizeGrade.ToString(),
                                prizeCount = info3.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info3.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data3);
                            info4 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖);
                            PrizeResultViewInfo info9 = new PrizeResultViewInfo {
                                PrizeType = info4.PrizeType,
                                GivePoint = info4.GivePoint,
                                GiveCouponId = info4.GiveCouponId,
                                GiveShopBookId = info4.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info9);
                            PrizeData data4 = new PrizeData {
                                prizeId = info4.PrizeId,
                                prize = info4.Prize,
                                prizeType = info4.PrizeGrade.ToString(),
                                prizeCount = info4.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info4.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data4);
                            break;
                        }
                        case 3:
                        {
                            info3 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖);
                            PrizeResultViewInfo info10 = new PrizeResultViewInfo {
                                PrizeType = info3.PrizeType,
                                GivePoint = info3.GivePoint,
                                GiveCouponId = info3.GiveCouponId,
                                GiveShopBookId = info3.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info10);
                            PrizeData data5 = new PrizeData {
                                prizeId = info3.PrizeId,
                                prize = info3.Prize,
                                prizeType = info3.PrizeGrade.ToString(),
                                prizeCount = info3.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info3.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data5);
                            info4 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖);
                            PrizeResultViewInfo info11 = new PrizeResultViewInfo {
                                PrizeType = info4.PrizeType,
                                GivePoint = info4.GivePoint,
                                GiveCouponId = info4.GiveCouponId,
                                GiveShopBookId = info4.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info11);
                            PrizeData data6 = new PrizeData {
                                prizeId = info4.PrizeId,
                                prize = info4.Prize,
                                prizeType = info4.PrizeGrade.ToString(),
                                prizeCount = info4.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info4.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data6);
                            info5 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.三等奖);
                            PrizeResultViewInfo info12 = new PrizeResultViewInfo {
                                PrizeType = info5.PrizeType,
                                GivePoint = info5.GivePoint,
                                GiveCouponId = info5.GiveCouponId,
                                GiveShopBookId = info5.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info12);
                            PrizeData data7 = new PrizeData {
                                prizeId = info5.PrizeId,
                                prize = info5.Prize,
                                prizeType = info5.PrizeGrade.ToString(),
                                prizeCount = info5.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info5.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data7);
                            break;
                        }
                        case 4:
                        {
                            info3 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖);
                            PrizeResultViewInfo info13 = new PrizeResultViewInfo {
                                PrizeType = info3.PrizeType,
                                GivePoint = info3.GivePoint,
                                GiveCouponId = info3.GiveCouponId,
                                GiveShopBookId = info3.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info13);
                            PrizeData data8 = new PrizeData {
                                prizeId = info3.PrizeId,
                                prize = info3.Prize,
                                prizeType = info3.PrizeGrade.ToString(),
                                prizeCount = info3.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info3.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data8);
                            info4 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖);
                            PrizeResultViewInfo info14 = new PrizeResultViewInfo {
                                PrizeType = info4.PrizeType,
                                GivePoint = info4.GivePoint,
                                GiveCouponId = info4.GiveCouponId,
                                GiveShopBookId = info4.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info14);
                            PrizeData data9 = new PrizeData {
                                prizeId = info4.PrizeId,
                                prize = info4.Prize,
                                prizeType = info4.PrizeGrade.ToString(),
                                prizeCount = info4.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info4.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data9);
                            info5 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.三等奖);
                            PrizeResultViewInfo info15 = new PrizeResultViewInfo {
                                PrizeType = info5.PrizeType,
                                GivePoint = info5.GivePoint,
                                GiveCouponId = info5.GiveCouponId,
                                GiveShopBookId = info5.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info15);
                            PrizeData data10 = new PrizeData {
                                prizeId = info5.PrizeId,
                                prize = info5.Prize,
                                prizeType = info5.PrizeGrade.ToString(),
                                prizeCount = info5.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info5.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data10);
                            info6 = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.四等奖);
                            PrizeResultViewInfo info16 = new PrizeResultViewInfo {
                                PrizeType = info6.PrizeType,
                                GivePoint = info6.GivePoint,
                                GiveCouponId = info6.GiveCouponId,
                                GiveShopBookId = info6.GiveShopBookId
                            };
                            fullName = GameHelper.GetPrizeFullName(info16);
                            PrizeData data11 = new PrizeData {
                                prizeId = info6.PrizeId,
                                prize = info6.Prize,
                                prizeType = info6.PrizeGrade.ToString(),
                                prizeCount = info6.PrizeRate,
                                prizeName = GameHelper.GetPrizeName(info6.PrizeType, fullName),
                                PrizeFullName = fullName
                            };
                            list2.Add(data11);
                            break;
                        }
                    }
                    data.prizeLists = list2;
                    IsoDateTimeConverter converter = new IsoDateTimeConverter {
                        DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                    };
                    context.Response.Write(JsonConvert.SerializeObject(data, Formatting.Indented, new JsonConverter[] { converter }));
                }
            }
        }

        public string GetPrizeName(PrizeGrade prizeGrade)
        {
            if (prizeGrade == PrizeGrade.未中奖)
            {
                return "谢谢参与";
            }
            return prizeGrade.ToString();
        }

        private string GetUserName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }
            int length = name.Length;
            string str = name.Substring(0, 1) + "**";
            if (length <= 3)
            {
                return str;
            }
            return (str + name.Substring(length - 1));
        }

        private void GetUserPrizeLists(HttpContext context)
        {
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["gameid"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                int pageIndex = 1;
                int pageSize = 7;
                try
                {
                    pageIndex = int.Parse(context.Request["pageIndex"]);
                }
                catch (Exception)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                try
                {
                    pageSize = int.Parse(context.Request["pageSize"]);
                }
                catch (Exception)
                {
                    builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                    context.Response.Write(builder.ToString());
                    return;
                }
                IList<PrizeResultViewInfo> list = GameHelper.GetPrizeLogLists(GameHelper.GetModelByGameId(str).GameId, pageIndex, pageSize);
                builder.Append("\"lists\":[");
                int count = list.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        PrizeResultViewInfo item = list[i];
                        builder.Append("{\"PrizeGrade\":\"" + item.PrizeGrade.ToString() + "\",\"UserName\":\"" + this.GetUserName(item.UserName) + "\",\"PrizeName\":\"" + GameHelper.GetPrizeName(item.PrizeType, GameHelper.GetPrizeFullName(item)) + "\",\"Prize\":\"" + item.PrizeName + "\",\"DateTime\":\"" + item.PlayTime.ToString("yyyy-MM-dd") + "\"}");
                        if (i != (count - 1))
                        {
                            builder.Append(",");
                        }
                    }
                }
                builder.Append("]}");
                context.Response.Write(builder);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string str = context.Request["action"];
            if (!string.IsNullOrEmpty(str))
            {
                str = str.ToLower();
            }
            switch (str)
            {
                case "getprizelists":
                    this.GetPrizeLists(context);
                    return;

                case "checkusercanplay":
                    this.CheckUserCanPlay(context);
                    return;

                case "getprizeinfo":
                    this.GetPrize(context);
                    return;

                case "getuserprizelists":
                    this.GetUserPrizeLists(context);
                    return;

                case "checkcanvote":
                    this.CheckCanVote(context);
                    return;

                case "uservote":
                    this.UserVote(context);
                    return;

                case "getcoupon":
                    this.GetCouponToMember(context);
                    return;

                case "getopportunity":
                    this.GetOpportunity(context);
                    return;

                case "updatecellphone":
                    this.UpdateCellPhone(context);
                    return;
            }
        }

        private void UpdateCellPhone(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            string str = context.Request["CellPhone"];
            if (string.IsNullOrEmpty(str))
            {
                builder.Append("\"status\":\"fail\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                currentMember.CellPhone = str;
                if (MemberProcessor.UpdateMember(currentMember))
                {
                    builder.Append("\"status\":\"ok\",\"message\":\"修改成功\"}");
                }
                context.Response.Write(builder.ToString());
            }
        }

        private void UserVote(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            int voteId = 0;
            try
            {
                voteId = int.Parse(context.Request["voteId"]);
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            string str = context.Request["voteItem"];
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    if (!VoteHelper.Vote(voteId, str))
                    {
                        throw new Exception("投票失败！");
                    }
                    builder.Append("\"status\":\"1\",\"Desciption\":\"成功!\"}");
                    context.Response.Write(builder.ToString());
                }
                catch (Exception exception)
                {
                    builder.Append("\"status\":\"2\",\"Desciption\":\"" + exception.Message + "\"}");
                    context.Response.Write(builder.ToString());
                }
            }
            else
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
                context.Response.Write(builder.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class GameData
        {
            public DateTime BeginDate { get; set; }

            public string Description { get; set; }

            public DateTime EndDate { get; set; }

            public int HasPhone { get; set; }

            public int LimitEveryDay { get; set; }

            public int MaximumDailyLimit { get; set; }

            public int MemberCheck { get; set; }

            public IList<Hi_Ajax_Game.PrizeData> prizeLists { get; set; }

            public int status { get; set; }
        }

        public class PrizeData
        {
            public string prize { get; set; }

            public int prizeCount { get; set; }

            public string PrizeFullName { get; set; }

            public int prizeId { get; set; }

            public string prizeName { get; set; }

            public string prizeType { get; set; }
        }
    }
}

