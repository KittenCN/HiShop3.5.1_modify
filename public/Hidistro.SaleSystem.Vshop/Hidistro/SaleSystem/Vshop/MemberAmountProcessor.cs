namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.VShop;
    using Hidistro.Messages;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class MemberAmountProcessor
    {
        public static bool CommissionToAmount(MemberAmountDetailedInfo amountinfo, int userid, decimal amount)
        {
            AmountDao dao = new AmountDao();
            return dao.CommissionToAmount(amountinfo, userid, amount);
        }

        public static bool CreatAmount(MemberAmountDetailedInfo AmountInfo)
        {
            AmountDao dao = new AmountDao();
            return dao.CreatAmount(AmountInfo, null);
        }

        public static bool CreatAmountApplyRequest(MemberAmountRequestInfo applyInfo)
        {
            AmountDao dao = new AmountDao();
            bool flag = dao.CreatAmountApplyRequest(applyInfo);
            if (flag)
            {
                MemberInfo member = new MemberDao().GetMember(applyInfo.UserId);
                MemberAmountDetailedInfo model = new MemberAmountDetailedInfo {
                    UserId = applyInfo.UserId,
                    TradeAmount = -applyInfo.Amount,
                    PayId = Globals.GetGenerateId(),
                    UserName = applyInfo.UserName,
                    TradeType = TradeType.Withdrawals,
                    TradeTime = DateTime.Now,
                    State = 1,
                    TradeWays = GetWaysByRequestType(applyInfo.RequestType),
                    AvailableAmount = member.AvailableAmount - applyInfo.Amount,
                    Remark = "余额提现。收款账号：" + applyInfo.AccountCode
                };
                flag = dao.UpdateMember(model, null) && CreatAmount(model);
            }
            return flag;
        }

        public static MemberAmountDetailedInfo GetAmountDetail(int Id)
        {
            AmountDao dao = new AmountDao();
            return dao.GetAmountDetail(Id);
        }

        public static MemberAmountDetailedInfo GetAmountDetailByPayId(string PayId)
        {
            AmountDao dao = new AmountDao();
            return dao.GetAmountDetailByPayId(PayId);
        }

        public static Dictionary<string, decimal> GetAmountDic(MemberAmountQuery query)
        {
            return new AmountDao().GetAmountDic(query);
        }

        public static DbQueryResult GetAmountListRequest(int type, int page, int pagesize, int userId)
        {
            AmountDao dao = new AmountDao();
            return dao.GetAmountListRequest(type, page, pagesize, userId);
        }

        public static DbQueryResult GetAmountListRequestByTime(int type, int page, int pagesize, int userId, string startTime = "", string endTime = "")
        {
            AmountDao dao = new AmountDao();
            return dao.GetAmountListRequestByTime(type, page, pagesize, userId, startTime, endTime);
        }

        public static MemberAmountRequestInfo GetAmountRequestDetail(int serialid)
        {
            return new AmountDao().GetAmountRequestDetail(serialid);
        }

        public static int GetAmountRequestStatus(int serialid)
        {
            return new AmountDao().GetAmountRequestStatus(serialid);
        }

        public static DbQueryResult GetAmountWithUserName(MemberAmountQuery query)
        {
            return new AmountDao().GetAmountWithUserName(query);
        }

        public static DbQueryResult GetBalanceWithdrawListRequest(int type, int page, int pagesize, int userId)
        {
            return new AmountDao().GetBalanceWithdrawListRequest(type, page, pagesize, userId);
        }

        public static Dictionary<string, decimal> GetDataByUserId(int userid)
        {
            return new AmountDao().GetDataByUserId(userid);
        }

        public static DbQueryResult GetMemberAmountRequest(BalanceDrawRequestQuery query, string[] extendChecks = null)
        {
            return new AmountDao().GetMemberAmountRequest(query, extendChecks);
        }

        public static Dictionary<int, int> GetMulAmountRequestStatus(int[] serialids)
        {
            return new AmountDao().GetMulAmountRequestStatus(serialids);
        }

        public static PaymentModeInfo GetPaymentMode(TradeWays ways)
        {
            return new AmountDao().GetPaymentMode(ways);
        }

        public static SendRedpackRecordInfo GetSendRedpackRecordByID(string id = null, string sid = null)
        {
            return new SendRedpackRecordDao().GetSendRedpackRecordByID(id, sid);
        }

        public static decimal GetUserMaxAmountDetailed(int userid)
        {
            return new AmountDao().GetUserMaxAmountDetailed(userid);
        }

        public static TradeWays GetWaysByRequestType(RequesType type)
        {
            TradeWays balance = TradeWays.Balance;
            switch (type)
            {
                case RequesType.微信钱包:
                    return TradeWays.WeChatWallet;

                case RequesType.支付宝:
                    return TradeWays.Alipay;

                case RequesType.线下支付:
                    return TradeWays.LineTransfer;

                case RequesType.微信红包:
                    return TradeWays.WeChatWallet;
            }
            return balance;
        }

        public static string SendRedPackToBalanceDrawRequest(int serialid)
        {
            return new AmountDao().SendRedPackToAmountRequest(serialid);
        }

        public static bool SetAmountByShopAdjustment(MemberAmountDetailedInfo model)
        {
            AmountDao dao = new AmountDao();
            bool flag = dao.CreatAmount(model, null);
            if (flag)
            {
                flag = dao.UpdateMember(model, null);
            }
            return flag;
        }

        public static bool SetAmountRequestStatus(int[] serialids, int checkValue, string Remark = "", string Amount = "", string Operator = "")
        {
            bool flag = new AmountDao().SetAmountRequestStatus(serialids, checkValue, Remark, Amount, Operator);
            if ((checkValue == -1) && flag)
            {
                foreach (int num in serialids)
                {
                    MemberAmountRequestInfo amountRequestDetail = GetAmountRequestDetail(num);
                    MemberInfo member = new MemberDao().GetMember(amountRequestDetail.UserId);
                    MemberAmountDetailedInfo model = new MemberAmountDetailedInfo {
                        UserId = amountRequestDetail.UserId,
                        TradeAmount = amountRequestDetail.Amount,
                        PayId = Globals.GetGenerateId(),
                        UserName = amountRequestDetail.UserName,
                        TradeType = TradeType.WithdrawalsRefuse,
                        TradeTime = DateTime.Now,
                        State = 1,
                        TradeWays = GetWaysByRequestType(amountRequestDetail.RequestType),
                        AvailableAmount = member.AvailableAmount + amountRequestDetail.Amount,
                        Remark = "余额提现驳回"
                    };
                    flag = new AmountDao().UpdateMember(model, null) && CreatAmount(model);
                    MemberAmountRequestInfo balance = GetAmountRequestDetail(num);
                    if (balance != null)
                    {
                        string url = Globals.FullPath("/Vshop/MemberAmountRequestDetail.aspx?Id=" + balance.Id);
                        try
                        {
                            Messenger.SendWeiXinMsg_MemberAmountDrawCashRefuse(balance, url);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return flag;
        }

        public static bool SetRedpackRecordIsUsed(int id, bool issend)
        {
            return new AmountDao().SetRedpackRecordIsUsed(id, issend);
        }

        public static bool UserPayOrder(MemberAmountDetailedInfo model)
        {
            AmountDao dao = new AmountDao();
            model.State = 1;
            bool flag = dao.UpdateAmount(model);
            if (flag)
            {
                flag = dao.UpdateMember(model, null);
                SettingsManager.GetMasterSettings(true);
                Globals.Debuglog("触发自动成为分销商的条件", "_DebuglogMemberAutoToDistributor.txt");
                MemberInfo member = MemberProcessor.GetMember(model.UserId, true);
                if (VshopBrowser.IsPassAutoToDistributor(member, true))
                {
                    DistributorsBrower.MemberAutoToDistributor(member);
                }
                return flag;
            }
            Globals.Debuglog("充值操作重复提交了！！！" + model.UserId, "_DebuglogMemberAutoToDistributor.txt");
            return flag;
        }
    }
}

