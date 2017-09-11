namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.VShop;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class OneyuanTaoHelp
    {
        private static object Calculate = new object();
        private static object Orderidlock = new object();
        private static string provOrderid = "";

        public static bool AddInitParticipantInfo(int num = 50)
        {
            return new OneyuanTaoDao().AddInitParticipantInfo(num);
        }

        public static bool AddLuckInfo(LuckInfo info)
        {
            return new OneyuanTaoDao().AddLuckInfo(info);
        }

        public static bool AddLuckInfo(List<LuckInfo> infoList)
        {
            return new OneyuanTaoDao().AddLuckInfo(infoList);
        }

        public static bool AddOneyuanTao(OneyuanTaoInfo info)
        {
            info.ActivityId = GetOrderNumber(true);
            return new OneyuanTaoDao().AddOneyuanTao(info);
        }

        public static bool AddParticipant(OneyuanTaoParticipantInfo info)
        {
            if (string.IsNullOrEmpty(info.Pid))
            {
                info.Pid = GetOrderNumber(false);
            }
            return new OneyuanTaoDao().AddParticipant(info);
        }

        public static int BatchDeleteOneyuanTao(string[] ActivityIds)
        {
            return new OneyuanTaoDao().BatchDeleteOneyuanTao(ActivityIds);
        }

        public static int BatchSetOneyuanTaoIsOn(string[] ActivityIds, bool IsOn)
        {
            return new OneyuanTaoDao().BatchSetOneyuanTaoIsOn(ActivityIds, IsOn);
        }

        public static string CalculateWinner(string ActivityId = "")
        {
            string str = "0";
            if (Monitor.TryEnter(Calculate))
            {
                try
                {
                    try
                    {
                        IList<OneyuanTaoInfo> oneyuanTaoInfoNotCalculate;
                        if (string.IsNullOrEmpty(ActivityId))
                        {
                            oneyuanTaoInfoNotCalculate = new OneyuanTaoDao().GetOneyuanTaoInfoNotCalculate();
                        }
                        else
                        {
                            oneyuanTaoInfoNotCalculate = new List<OneyuanTaoInfo> {
                                GetOneyuanTaoInfoById(ActivityId)
                            };
                        }
                        foreach (OneyuanTaoInfo info in oneyuanTaoInfoNotCalculate)
                        {
                            str = "1";
                            if (info.FinishedNum == 0)
                            {
                                SetOneyuanTaoHasCalculate(info.ActivityId);
                            }
                            else if (info.HasCalculate)
                            {
                                str = "success";
                            }
                            else if (((info.ReachType == 1) && (info.FinishedNum >= info.ReachNum)) || ((info.ReachType == 2) || ((info.ReachType == 3) && (info.FinishedNum >= info.ReachNum))))
                            {
                                str = DoOneTaoDrawLottery(info);
                            }
                            else
                            {
                                str = "未满足开奖条件，开奖失败";
                                DoOneTaoRefund(info);
                            }
                        }
                        return str;
                    }
                    catch (Exception)
                    {
                        str = "0";
                    }
                    return str;
                }
                finally
                {
                    Monitor.Exit(Calculate);
                }
            }
            return "计算工作已启动，请等待计算开奖结果！";
        }

        public static bool CheckIsAll(string Aid)
        {
            return new OneyuanTaoDao().CheckIsAll(Aid);
        }

        public static OneyuanTaoInfo DataRowToOneyuanTaoInfo(DataRow dr)
        {
            return ReaderConvert.DataRowToModel<OneyuanTaoInfo>(dr);
        }

        public static bool DeleteOneyuanTao(string ActivityId)
        {
            return new OneyuanTaoDao().DeleteOneyuanTao(ActivityId);
        }

        public static bool DelParticipantMember(string ActivityId, bool DelAll = true)
        {
            return new OneyuanTaoDao().DelParticipantMember(ActivityId, DelAll);
        }

        private static string DoOneTaoDrawLottery(OneyuanTaoInfo WItem)
        {
            long num2;
            string str = "开奖失败";
            DateTime now = DateTime.Now;
            if (new OneyuanTaoDao().GetParticipantCount() < (50 + WItem.PrizeNumber))
            {
                AddInitParticipantInfo(50);
            }
            Dictionary<long, bool> dictionary = new Dictionary<long, bool>();
            dictionary.Clear();
            IList<Top50ParticipantInfo> source = new OneyuanTaoDao().GetTop50ParticipantList(now, 50);
        Label_0046:
            num2 = 0L;
            foreach (Top50ParticipantInfo info in source)
            {
                num2 += long.Parse(info.BuyTime.ToString("Hmmssfff"));
            }
            long num3 = num2 % long.Parse(WItem.FinishedNum.ToString());
            long key = 0x989681L + num3;
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, true);
                source.Last<Top50ParticipantInfo>().PrizeLuckInfo = string.Format("{0},{1},{2},{3}", new object[] { key.ToString(), num2, WItem.FinishedNum, num3 });
            }
            else
            {
                source.Last<Top50ParticipantInfo>().PrizeLuckInfo = string.Format("{0}重复,{1},{2},{3},", new object[] { key.ToString(), num2, WItem.FinishedNum, num3 });
            }
            if (dictionary.Count < WItem.PrizeNumber)
            {
                Top50ParticipantInfo nextParticipant = new OneyuanTaoDao().GetNextParticipant(now, source.Last<Top50ParticipantInfo>().Pid);
                if (nextParticipant != null)
                {
                    source.Add(nextParticipant);
                    goto Label_0046;
                }
            }
            IsoDateTimeConverter converter = new IsoDateTimeConverter {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff"
            };
            if (dictionary.Count != WItem.PrizeNumber)
            {
                str = "订单数据不足，无法开出指定数量的中奖号！";
                new OneyuanTaoDao().SetErrPrizeCountInfo(WItem.ActivityId, JsonConvert.SerializeObject(source, new JsonConverter[] { converter }));
                return str;
            }
            str = "success";
            if (SetOneyuanTaoPrizeTime(WItem.ActivityId, now, JsonConvert.SerializeObject(source, new JsonConverter[] { converter })))
            {
                foreach (KeyValuePair<long, bool> pair in dictionary)
                {
                    setWin(pair.Key.ToString(), WItem.ActivityId);
                }
            }
            return str;
        }

        private static void DoOneTaoRefund(OneyuanTaoInfo WItem)
        {
            SetOneyuanTaoHasCalculate(WItem.ActivityId);
        }

        public static OneyuanTaoParticipantInfo GetAddParticipant(int UserId, string Pid = "", string payNum = "")
        {
            return new OneyuanTaoDao().GetAddParticipant(UserId, Pid, payNum);
        }

        public static IList<LuckInfo> getLuckInfoList(bool IsWin, string ActivityId)
        {
            return new OneyuanTaoDao().getLuckInfoList(IsWin, ActivityId);
        }

        public static IList<LuckInfo> getLuckInfoListByAId(string ActivityId, int UserId)
        {
            return new OneyuanTaoDao().getLuckInfoListByAId(ActivityId, UserId);
        }

        public static OneTaoState getOneTaoState(OneyuanTaoInfo info)
        {
            try
            {
                OneTaoState state;
                bool isOn = info.IsOn;
                bool isEnd = info.IsEnd;
                DateTime startTime = info.StartTime;
                DateTime endTime = info.EndTime;
                DateTime now = DateTime.Now;
                if (info.IsSuccess)
                {
                    state = OneTaoState.已开奖;
                }
                else if (info.IsAllRefund)
                {
                    state = OneTaoState.退款完成;
                }
                else if (info.HasCalculate && (info.FinishedNum > 0))
                {
                    state = OneTaoState.开奖失败;
                }
                else if ((isOn && !isEnd) && ((startTime < now) && (endTime > now)))
                {
                    state = OneTaoState.进行中;
                }
                else if ((isOn && !isEnd) && (startTime > now))
                {
                    state = OneTaoState.未开始;
                }
                else
                {
                    state = OneTaoState.已结束;
                }
                return state;
            }
            catch (Exception)
            {
                return OneTaoState.NONE;
            }
        }

        public static DbQueryResult GetOneyuanPartInDataTable(OneyuanTaoPartInQuery query)
        {
            return new OneyuanTaoDao().GetOneyuanPartInDataTable(query);
        }

        public static DbQueryResult GetOneyuanTao(OneyuanTaoQuery query)
        {
            return new OneyuanTaoDao().GetOneyuanTao(query);
        }

        public static OneyuanTaoInfo GetOneyuanTaoInfoById(string ActivityId)
        {
            return new OneyuanTaoDao().GetOneyuanTaoInfoById(ActivityId);
        }

        public static IList<OneyuanTaoInfo> GetOneyuanTaoInfoByIdList(string[] ActivityIds)
        {
            return new OneyuanTaoDao().GetOneyuanTaoInfoByIdList(ActivityIds);
        }

        public static IList<OneyuanTaoInfo> GetOneyuanTaoInfoNotCalculate()
        {
            return new OneyuanTaoDao().GetOneyuanTaoInfoNotCalculate();
        }

        public static int GetOneyuanTaoTotalNum(out int hasStart, out int waitStart, out int hasEnd)
        {
            return new OneyuanTaoDao().GetOneyuanTaoTotalNum(out hasStart, out waitStart, out hasEnd);
        }

        public static string GetOrderNumber(bool isActivity = true)
        {
            lock (Orderidlock)
            {
                string str = DateTime.Now.ToString("yyMMddHHmmssfff");
                if (str == provOrderid)
                {
                    Thread.Sleep(1);
                    str = DateTime.Now.ToString("yyMMddHHmmssfff");
                }
                provOrderid = str;
                if (isActivity)
                {
                    str = "A" + str;
                }
                else
                {
                    str = "B" + str;
                }
                return str;
            }
        }

        public static IList<OneyuanTaoParticipantInfo> GetParticipantList(string ActivityId, int[] UserIds = null, string[] Ids = null)
        {
            return new OneyuanTaoDao().GetParticipantList(ActivityId, UserIds, Ids);
        }

        public static List<string> GetParticipantPids(string ActivityId, bool IsPay = true, bool IsRefund = false, string PayWay = "alipay")
        {
            return new OneyuanTaoDao().GetParticipantPids(ActivityId, IsPay, IsRefund, PayWay);
        }

        public static string getPrizeCountInfo(string ActivityId)
        {
            return new OneyuanTaoDao().getPrizeCountInfo(ActivityId);
        }

        public static string GetPrizesDeliveStatus(string status)
        {
            string str = "未定义";
            string str2 = status;
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
                        return "已发货";
                    }
                    if (str2 != "3")
                    {
                        return str;
                    }
                    return "已收货";
                }
            }
            else
            {
                return "待填写收货地址";
            }
            return "待发货";
        }

        public static OneTaoPrizeState getPrizeState(OneyuanTaoInfo info)
        {
            if (info.IsSuccess)
            {
                return OneTaoPrizeState.成功开奖;
            }
            if (!info.HasCalculate)
            {
                return OneTaoPrizeState.未开奖;
            }
            if (info.FinishedNum == 0)
            {
                return OneTaoPrizeState.已关闭;
            }
            if (info.IsAllRefund)
            {
                return OneTaoPrizeState.已退款;
            }
            if (info.IsRefund && info.IsAllRefund)
            {
                return OneTaoPrizeState.NONE;
            }
            return OneTaoPrizeState.待退款;
        }

        public static string getReachTypeStr(int ReachTyp)
        {
            string str = "";
            if (ReachTyp == 1)
            {
                return "满份开奖";
            }
            if (ReachTyp == 2)
            {
                return "到期开奖";
            }
            if (ReachTyp == 3)
            {
                str = "到期满份开奖";
            }
            return str;
        }

        public static IList<OneyuanTaoParticipantInfo> GetRefundParticipantList(string[] PIds)
        {
            return new OneyuanTaoDao().GetRefundParticipantList(PIds);
        }

        public static int GetRefundTotalNum(out int hasRefund)
        {
            return new OneyuanTaoDao().GetRefundTotalNum(out hasRefund);
        }

        public static string GetSkuStrBySkuId(string Skuid, bool ShowAttr = true)
        {
            return new OneyuanTaoDao().GetSkuStrBySkuId(Skuid, ShowAttr);
        }

        public static string getStateStr(OneyuanTaoInfo info)
        {
            return getOneTaoState(info).ToString();
        }

        public static int GetUserAlreadyBuyNum(int userId, string activityId)
        {
            return new OneyuanTaoDao().GetUserAlreadyBuyNum(userId, activityId);
        }

        public static IList<LuckInfo> getWinnerLuckInfoList(string ActivityId, string Pid = "")
        {
            return new OneyuanTaoDao().getWinnerLuckInfoList(ActivityId, Pid);
        }

        public static bool IsExistAlipayRefundNUm(string batch_no)
        {
            return new OneyuanTaoDao().IsExistAlipayRefundNUm(batch_no);
        }

        public static int MermberCanbuyNum(string Aid, int userid)
        {
            return new OneyuanTaoDao().MermberCanbuyNum(Aid, userid);
        }

        public static DataTable PrizesDeliveryRecord(string Pid)
        {
            return new OneyuanTaoDao().PrizesDeliveryRecord(Pid);
        }

        public static int SetIsAllRefund(List<string> ActivivyIds)
        {
            return new OneyuanTaoDao().SetIsAllRefund(ActivivyIds);
        }

        public static bool SetOneyuanTaoFinishedNum(string ActivityId, int Addnum = 0)
        {
            return new OneyuanTaoDao().SetOneyuanTaoFinishedNum(ActivityId, Addnum);
        }

        public static bool SetOneyuanTaoHasCalculate(string ActivityId)
        {
            return new OneyuanTaoDao().SetOneyuanTaoHasCalculate(ActivityId);
        }

        public static bool SetOneyuanTaoIsOn(string ActivityId, bool IsOn)
        {
            return new OneyuanTaoDao().SetOneyuanTaoIsOn(ActivityId, IsOn);
        }

        public static bool SetOneyuanTaoPrizeTime(string ActivityId, DateTime PrizeTime, string PrizeInfoJson)
        {
            return new OneyuanTaoDao().SetOneyuanTaoPrizeTime(ActivityId, PrizeTime, PrizeInfoJson);
        }

        public static bool Setout_refund_no(string pid, string Refundoutid)
        {
            return new OneyuanTaoDao().Setout_refund_no(pid, Refundoutid);
        }

        public static bool SetPayinfo(OneyuanTaoParticipantInfo info)
        {
            return new OneyuanTaoDao().SetPayinfo(info);
        }

        public static bool SetRefundinfo(OneyuanTaoParticipantInfo info)
        {
            return new OneyuanTaoDao().SetRefundinfo(info);
        }

        public static bool SetRefundinfoErr(OneyuanTaoParticipantInfo info)
        {
            return new OneyuanTaoDao().SetRefundinfoErr(info);
        }

        public static bool setWin(string PrizeNum, string ActivityId)
        {
            return new OneyuanTaoDao().setWin(PrizeNum, ActivityId);
        }

        public static bool UpdateOneyuanTao(OneyuanTaoInfo info)
        {
            return new OneyuanTaoDao().UpdateOneyuanTao(info);
        }
    }
}

