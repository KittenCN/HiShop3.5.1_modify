namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public class AmountDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        private static object RedLock = new object();

        public bool AddBalanceDrawRequest(BalanceDrawRequestInfo bdrinfo)
        {
            string query = "INSERT INTO Hishop_BalanceDrawRequest(UserId,RequestType,UserName,RequestTime,Amount,AccountName,BankName,CellPhone,MerchantCode,Remark,CheckTime,IsCheck) VALUES(@UserId,@RequestType,@UserName,@RequestTime,@Amount,@AccountName,@bankName,@CellPhone,@MerchantCode,@Remark,@CheckTime,@IsCheck)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, bdrinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "RequestType", DbType.Int32, bdrinfo.RequestType);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, bdrinfo.UserName);
            this.database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, bdrinfo.RequestTime);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, bdrinfo.Amount);
            this.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, bdrinfo.AccountName);
            this.database.AddInParameter(sqlStringCommand, "bankName", DbType.String, bdrinfo.BankName);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, bdrinfo.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, bdrinfo.MerchantCode);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, bdrinfo.Remark);
            this.database.AddInParameter(sqlStringCommand, "CheckTime", DbType.DateTime, bdrinfo.CheckTime);
            this.database.AddInParameter(sqlStringCommand, "IsCheck", DbType.String, bdrinfo.IsCheck);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CommissionToAmount(MemberAmountDetailedInfo amountinfo, int userid, decimal amount)
        {
            bool flag = this.CreatAmount(amountinfo, null);
            if (flag)
            {
                flag = this.UpdateMember(amountinfo, null);
                if (flag)
                {
                    flag = new DistributorsDao().UpdateBalanceDistributors(userid, amount);
                    if (flag)
                    {
                        BalanceDrawRequestInfo bdrinfo = new BalanceDrawRequestInfo();
                        MemberInfo member = new MemberDao().GetMember(userid);
                        bdrinfo.UserId = member.UserId;
                        bdrinfo.RequestType = 4;
                        bdrinfo.UserName = member.UserName;
                        bdrinfo.RequestTime = DateTime.Now;
                        bdrinfo.Amount = amount;
                        bdrinfo.AccountName = string.IsNullOrEmpty(member.RealName) ? member.UserName : member.RealName;
                        bdrinfo.BankName = "";
                        bdrinfo.MerchantCode = "佣金转余额";
                        bdrinfo.Remark = "";
                        bdrinfo.CheckTime = DateTime.Now;
                        bdrinfo.CellPhone = string.IsNullOrEmpty(member.CellPhone) ? "" : member.CellPhone;
                        bdrinfo.IsCheck = "2";
                        flag = this.AddBalanceDrawRequest(bdrinfo);
                    }
                }
            }
            return flag;
        }

        public bool CreatAmount(MemberAmountDetailedInfo mountInfo, DbTransaction dbTrans = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_MemberAmountDetailed(UserId,UserName,PayId,TradeAmount,AvailableAmount,TradeType,TradeWays,TradeTime, Remark,State) VALUES(@UserId,@UserName,@PayId,@TradeAmount,@AvailableAmount, @TradeType, @TradeWays,@TradeTime,@Remark,@State)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, mountInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, mountInfo.UserName);
            this.database.AddInParameter(sqlStringCommand, "PayId", DbType.String, mountInfo.PayId);
            this.database.AddInParameter(sqlStringCommand, "TradeAmount", DbType.Decimal, mountInfo.TradeAmount);
            this.database.AddInParameter(sqlStringCommand, "AvailableAmount", DbType.Decimal, mountInfo.AvailableAmount);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, mountInfo.TradeType);
            this.database.AddInParameter(sqlStringCommand, "TradeWays", DbType.Int32, mountInfo.TradeWays);
            this.database.AddInParameter(sqlStringCommand, "TradeTime", DbType.DateTime, mountInfo.TradeTime);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, mountInfo.Remark);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, mountInfo.State);
            if (dbTrans == null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTrans) > 0);
        }

        public bool CreatAmountApplyRequest(MemberAmountRequestInfo applyInfo)
        {
            string query = "INSERT INTO Hishop_MemberAmountRequest(UserId,UserName,RequestTime,Amount,RequestType,AccountCode,AccountName,BankName,Remark,State,CellPhone) VALUES(@UserId,@UserName,@RequestTime,@Amount,@RequestType,@AccountCode,@AccountName,@BankName,@Remark,@State,@CellPhone)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, applyInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, applyInfo.UserName);
            this.database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, applyInfo.RequestTime);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, applyInfo.Amount);
            this.database.AddInParameter(sqlStringCommand, "RequestType", DbType.Int32, applyInfo.RequestType);
            this.database.AddInParameter(sqlStringCommand, "AccountCode", DbType.String, applyInfo.AccountCode);
            this.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, applyInfo.AccountName);
            this.database.AddInParameter(sqlStringCommand, "BankName", DbType.String, applyInfo.BankName);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, applyInfo.Remark);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, applyInfo.State);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, applyInfo.CellPhone);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateSendRedpackRecord(int serialid, int userid, string openid, int amount, string act_name, string wishing)
        {
            bool flag = true;
            int num = 0x4e20;
            int num2 = amount;
            SendRedpackRecordInfo sendredpackinfo = new SendRedpackRecordInfo {
                BalanceDrawRequestID = serialid,
                UserID = userid,
                OpenID = openid,
                ActName = act_name,
                Wishing = wishing,
                ClientIP = Globals.IPAddress
            };
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                SendRedpackRecordDao dao = new SendRedpackRecordDao();
                try
                {
                    try
                    {
                        if (num2 <= num)
                        {
                            sendredpackinfo.Amount = amount;
                            return dao.AddSendRedpackRecord(sendredpackinfo, dbTran);
                        }
                        int num3 = amount % num;
                        int num4 = amount / num;
                        if (num3 > 0)
                        {
                            sendredpackinfo.Amount = num3;
                            flag = dao.AddSendRedpackRecord(sendredpackinfo, dbTran);
                        }
                        if (flag)
                        {
                            for (int i = 0; i < num4; i++)
                            {
                                sendredpackinfo.Amount = num;
                                flag = dao.AddSendRedpackRecord(sendredpackinfo, dbTran);
                                if (!flag)
                                {
                                    dbTran.Rollback();
                                }
                            }
                            if (!flag)
                            {
                                dbTran.Rollback();
                            }
                            return flag;
                        }
                        dbTran.Rollback();
                        return flag;
                    }
                    catch
                    {
                        if (dbTran.Connection != null)
                        {
                            dbTran.Rollback();
                        }
                        flag = false;
                    }
                    return flag;
                }
                finally
                {
                    if (flag)
                    {
                        dbTran.Commit();
                    }
                    connection.Close();
                }
            }
            return flag;
        }

        public string CreatRedpackId(string mch_id)
        {
            string str = "";
            lock (RedLock)
            {
                str = mch_id + DateTime.Now.ToString("yyyymmdd") + DateTime.Now.ToString("MMddHHmmss");
                Thread.Sleep(1);
            }
            return str;
        }

        public MemberAmountDetailedInfo GetAmountDetail(int Id)
        {
            MemberAmountDetailedInfo info = new MemberAmountDetailedInfo();
            string query = "select * from Hishop_MemberAmountDetailed where Id=" + Id;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                info.Id = Id;
                info.UserId = int.Parse(table.Rows[0]["UserId"].ToString());
                info.PayId = table.Rows[0]["PayId"].ToString();
                info.TradeAmount = Math.Round(decimal.Parse(table.Rows[0]["TradeAmount"].ToString()), 2);
                info.AvailableAmount = Math.Round(decimal.Parse(table.Rows[0]["AvailableAmount"].ToString()), 2);
                info.TradeType = (TradeType) table.Rows[0]["TradeType"];
                info.UserName = table.Rows[0]["UserName"].ToString();
                info.TradeWays = (TradeWays) table.Rows[0]["TradeWays"];
                info.TradeTime = DateTime.Parse(table.Rows[0]["TradeTime"].ToString());
                info.Remark = table.Rows[0]["Remark"].ToString();
                info.State = int.Parse(table.Rows[0]["State"].ToString());
                info.GatewayPayId = table.Rows[0]["GatewayPayId"].ToString();
                return info;
            }
            return null;
        }

        public MemberAmountDetailedInfo GetAmountDetailByPayId(string PayId)
        {
            MemberAmountDetailedInfo info = new MemberAmountDetailedInfo();
            string query = "select * from Hishop_MemberAmountDetailed where PayId='" + PayId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                info.Id = int.Parse(table.Rows[0]["Id"].ToString());
                info.UserId = int.Parse(table.Rows[0]["UserId"].ToString());
                info.PayId = table.Rows[0]["PayId"].ToString();
                info.TradeAmount = Math.Round(decimal.Parse(table.Rows[0]["TradeAmount"].ToString()), 2);
                info.AvailableAmount = Math.Round(decimal.Parse(table.Rows[0]["AvailableAmount"].ToString()), 2);
                info.TradeType = (TradeType) table.Rows[0]["TradeType"];
                info.UserName = table.Rows[0]["UserName"].ToString();
                info.TradeWays = (TradeWays) table.Rows[0]["TradeWays"];
                info.TradeTime = DateTime.Parse(table.Rows[0]["TradeTime"].ToString());
                info.Remark = table.Rows[0]["Remark"].ToString();
                info.State = int.Parse(table.Rows[0]["State"].ToString());
                info.GatewayPayId = table.Rows[0]["GatewayPayId"].ToString();
                return info;
            }
            return null;
        }

        public Dictionary<string, decimal> GetAmountDic(MemberAmountQuery query)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            StringBuilder builder = new StringBuilder();
            builder.Append(" State=1 ");
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.PayId))
            {
                builder.AppendFormat(" and PayId = '{0}'", query.PayId);
            }
            if (!string.IsNullOrEmpty(query.TradeType))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" TradeType = {0}", query.TradeType);
            }
            if (!string.IsNullOrEmpty(query.TradeWays))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" TradeWays = {0}", query.TradeWays);
            }
            if (!string.IsNullOrEmpty(query.StartTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',TradeTime)>=0", query.StartTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("  datediff(dd,'{0}',TradeTime)<=0", query.EndTime);
            }
            string str = "select isnull(sum(a.TotalAmount),0) AS CurrentTotal,isnull(sum(a.AvailableAmount),0) AS AvailableTotal from aspnet_Members a where exists (select userid  from Hishop_MemberAmountDetailed where userid=a.userid and " + builder + ") ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            dictionary.Add("CurrentTotal", decimal.Parse(table.Rows[0]["CurrentTotal"].ToString()));
            dictionary.Add("AvailableTotal", decimal.Parse(table.Rows[0]["AvailableTotal"].ToString()));
            string str2 = "SELECT isnull(sum(Amount),0) FROM Hishop_MemberAmountRequest a WHERE exists (select userid  from Hishop_MemberAmountDetailed where userid=a.userid and " + builder + ") and State in(0,1,3)";
            DbCommand command = this.database.GetSqlStringCommand(str2);
            decimal num = decimal.Parse(this.database.ExecuteScalar(command).ToString());
            dictionary.Add("UnliquidatedTotal", num);
            return dictionary;
        }

        public DbQueryResult GetAmountListRequest(int type, int page, int pagesize, int userId)
        {
            string table = "Hishop_MemberAmountDetailed";
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" State={0} AND UserId={1} ", 1, userId);
            if (type != 0)
            {
                if (type == 1)
                {
                    builder.AppendFormat(" AND TradeAmount < 0 ", new object[0]);
                }
                else if (type == 2)
                {
                    builder.AppendFormat(" AND TradeAmount > 0 ", new object[0]);
                }
            }
            return DataHelper.PagingByRownumber(page, pagesize, "TradeTime", SortAction.Desc, true, table, "ID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult GetAmountListRequestByTime(int type, int page, int pagesize, int userId, string startTime = "", string endTime = "")
        {
            string table = "Hishop_MemberAmountDetailed";
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" State={0} AND UserId={1} ", 1, userId);
            if (type != 0)
            {
                if (type == 1)
                {
                    builder.AppendFormat(" AND TradeAmount < 0 ", new object[0]);
                }
                else if (type == 2)
                {
                    builder.AppendFormat(" AND TradeAmount > 0 ", new object[0]);
                }
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',TradeTime)>=0", startTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("  datediff(dd,'{0}',TradeTime)<=0", endTime);
            }
            return DataHelper.PagingByRownumber(page, pagesize, "TradeTime", SortAction.Desc, true, table, "ID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public MemberAmountRequestInfo GetAmountRequestDetail(int serialid)
        {
            MemberAmountRequestInfo info = new MemberAmountRequestInfo();
            string query = "select * from Hishop_MemberAmountRequest where Id=" + serialid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            info.Id = serialid;
            info.UserId = int.Parse(table.Rows[0]["UserId"].ToString());
            info.UserName = table.Rows[0]["UserName"].ToString();
            info.RequestTime = DateTime.Parse(table.Rows[0]["RequestTime"].ToString());
            info.Amount = Math.Round(decimal.Parse(table.Rows[0]["Amount"].ToString()), 2);
            info.RequestType = (RequesType) table.Rows[0]["RequestType"];
            info.AccountCode = table.Rows[0]["AccountCode"].ToString();
            info.AccountName = table.Rows[0]["AccountName"].ToString();
            info.BankName = table.Rows[0]["BankName"].ToString();
            info.Remark = table.Rows[0]["Remark"].ToString();
            info.RedpackId = (table.Rows[0]["RedpackId"] == DBNull.Value) ? "" : table.Rows[0]["RedpackId"].ToString();
            info.State = (RequesState) table.Rows[0]["State"];
            if (table.Rows[0]["CheckTime"] != DBNull.Value)
            {
                info.CheckTime = new DateTime?(DateTime.Parse(table.Rows[0]["CheckTime"].ToString()));
            }
            else
            {
                info.CheckTime = null;
            }
            info.CellPhone = table.Rows[0]["CellPhone"].ToString();
            info.Operator = table.Rows[0]["Operator"].ToString();
            if (((info.RequestType == RequesType.微信红包) || (info.RequestType == RequesType.微信钱包)) && string.IsNullOrEmpty(info.RedpackId))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                string str2 = this.CreatRedpackId(masterSettings.WeixinPartnerID);
                query = "update Hishop_MemberAmountRequest set RedpackId=@redId where Id=@SerialID and RedpackId  is null ";
                sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, serialid);
                this.database.AddInParameter(sqlStringCommand, "redId", DbType.String, str2);
                if (this.database.ExecuteNonQuery(sqlStringCommand) > 0)
                {
                    info.RedpackId = str2;
                }
            }
            return info;
        }

        public int GetAmountRequestStatus(int serialid)
        {
            string query = "select State from Hishop_MemberAmountRequest where Id=" + serialid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return int.Parse(Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)).ToString());
        }

        public DbQueryResult GetAmountWithUserName(MemberAmountQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" State=1 ");
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.PayId))
            {
                builder.AppendFormat(" and PayId = '{0}'", query.PayId);
            }
            if (!string.IsNullOrEmpty(query.TradeType))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" TradeType = {0}", query.TradeType);
            }
            if (!string.IsNullOrEmpty(query.TradeWays))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" TradeWays = {0}", query.TradeWays);
            }
            if (!string.IsNullOrEmpty(query.StartTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',TradeTime)>=0", query.StartTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("  datediff(dd,'{0}',TradeTime)<=0", query.EndTime);
            }
            string table = "Hishop_MemberAmountDetailed";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "Id", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult GetBalanceWithdrawListRequest(int type, int page, int pagesize, int userId)
        {
            string table = "Hishop_MemberAmountRequest";
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" UserId={0} ", userId);
            if (type != 0)
            {
                builder.AppendFormat(" AND State not in(-1,2) ", new object[0]);
            }
            return DataHelper.PagingByRownumber(page, pagesize, "Id", SortAction.Desc, true, table, "ID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public Dictionary<string, decimal> GetDataByUserId(int userid)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            string query = "select COUNT(*) AS OrderCount,isnull(SUM(ValidOrderTotal),0) AS OrderTotal from  dbo.vw_VShop_FinishOrder_Main where UserId=" + userid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            dictionary.Add("OrderCount", decimal.Parse(table.Rows[0]["OrderCount"].ToString()));
            dictionary.Add("OrderTotal", decimal.Parse(table.Rows[0]["OrderTotal"].ToString()));
            string str2 = "SELECT isnull(sum(Amount),0) FROM Hishop_MemberAmountRequest a WHERE State in(0,1,3) and userid=" + userid;
            DbCommand command = this.database.GetSqlStringCommand(str2);
            decimal num = decimal.Parse(this.database.ExecuteScalar(command).ToString());
            dictionary.Add("RequestAmount", num);
            return dictionary;
        }

        public DbQueryResult GetMemberAmountRequest(BalanceDrawRequestQuery query, string[] extendCheckStatus = null)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.RequestTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" convert(varchar(10),RequestTime,120)='{0}'", query.RequestTime);
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" State={0}", query.IsCheck);
            }
            if (extendCheckStatus != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.Append(" State in (" + string.Join(",", extendCheckStatus) + ") ");
            }
            if (!string.IsNullOrEmpty(query.CheckTime.ToString()) && (query.CheckTime.ToString() != "CheckTime"))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" convert(varchar(10),CheckTime,120)='{0}'", query.CheckTime);
            }
            if (!string.IsNullOrEmpty(query.CheckTime.ToString()) && (query.CheckTime.ToString() == "CheckTime"))
            {
                if (!string.IsNullOrEmpty(query.RequestStartTime.ToString()))
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" AND ");
                    }
                    builder.AppendFormat(" datediff(dd,'{0}',CheckTime)>=0", query.RequestStartTime);
                }
                if (!string.IsNullOrEmpty(query.RequestEndTime.ToString()))
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" AND ");
                    }
                    builder.AppendFormat("  datediff(dd,'{0}',CheckTime)<=0", query.RequestEndTime);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(query.RequestStartTime.ToString()))
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" AND ");
                    }
                    builder.AppendFormat(" datediff(dd,'{0}',RequestTime)>=0", query.RequestStartTime);
                }
                if (!string.IsNullOrEmpty(query.RequestEndTime.ToString()))
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" AND ");
                    }
                    builder.AppendFormat("  datediff(dd,'{0}',RequestTime)<=0", query.RequestEndTime);
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_MemberAmountRequest ", "Id", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public Dictionary<int, int> GetMulAmountRequestStatus(int[] serialids)
        {
            string query = "select State,Id from Hishop_MemberAmountRequest where Id in(" + string.Join<int>(",", serialids) + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    dictionary.Add((int) row["Id"], (int) row["State"]);
                }
            }
            return dictionary;
        }

        public PaymentModeInfo GetPaymentMode(TradeWays ways)
        {
            string str = "";
            if (ways == TradeWays.Alipay)
            {
                str = "hishop.plugins.payment.ws_wappay.wswappayrequest";
            }
            else if (ways == TradeWays.ShengFutong)
            {
                str = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest";
            }
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public decimal GetUserMaxAmountDetailed(int userid)
        {
            string query = "select isnull(Max(TradeAmount),0) from Hishop_MemberAmountDetailed where UserID=" + userid + " and State=1";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return decimal.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
        }

        public bool MemberAmountAddByRefund(MemberInfo memberInfo, decimal amount, string orderid)
        {
            MemberAmountDetailedInfo info=new MemberAmountDetailedInfo();
            info = new MemberAmountDetailedInfo {
                UserId = memberInfo.UserId,
                UserName = memberInfo.UserName,
                PayId = Globals.GetGenerateId(),
                TradeAmount = decimal.Round(amount, 2),
                TradeType = TradeType.OrderClose,
                TradeTime = DateTime.Now,
                TradeWays = TradeWays.Balance,
                State = 1,
                AvailableAmount = memberInfo.AvailableAmount + info.TradeAmount,
                Remark = "订单号：" + orderid
            };
            return new AmountDao().UseBalance(info);
        }

        public string SendRedPackToAmountRequest(int serialid)
        {
            if (!SettingsManager.GetMasterSettings(false).EnableWeiXinRequest)
            {
                return "管理员后台未开启微信付款！";
            }
            string query = "select a.Id,a.userid,a.Amount,b.OpenID,isnull(b.OpenId,'') as OpenId from Hishop_MemberAmountRequest a inner join aspnet_Members b on a.userid=b.userid where Id=@SerialID and State=1";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, serialid);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            string str3 = string.Empty;
            int userid = 0;
            if (table.Rows.Count > 0)
            {
                str3 = table.Rows[0]["OpenId"].ToString();
                userid = int.Parse(table.Rows[0]["UserID"].ToString());
                decimal num2 = decimal.Parse(table.Rows[0]["Amount"].ToString()) * 100M;
                int amount = Convert.ToInt32(num2);
                if (string.IsNullOrEmpty(str3))
                {
                    return "用户未绑定微信号";
                }
                query = "select top 1 ID from vshop_SendRedpackRecord where BalanceDrawRequestID=-" + table.Rows[0]["Id"].ToString();
                sqlStringCommand = this.database.GetSqlStringCommand(query);
                if (this.database.ExecuteDataSet(sqlStringCommand).Tables[0].Rows.Count > 0)
                {
                    return "-1";
                }
                return (this.CreateSendRedpackRecord(-serialid, userid, str3, amount, "您的提现申请已成功", "恭喜您提现成功!") ? "1" : "提现操作失败");
            }
            return "该用户没有提现申请,或者提现申请未审核";
        }

        public bool SetAmountRequestStatus(int[] serialid, int checkValue, string Remark = "", string Amount = "", string Operator = "")
        {
            string query = "UPDATE Hishop_MemberAmountRequest set State=@State ";
            if (!string.IsNullOrEmpty(Remark))
            {
                query = query + ",Remark=@Remark ";
            }
            if (!string.IsNullOrEmpty(Amount))
            {
                query = query + ",Amount=@Amount ";
            }
            query = (query + ",CheckTime=@CheckTime,Operator=@Operator") + " where State not in(-1,2) and  Id in(" + string.Join<int>(",", serialid) + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int16, checkValue);
            if (!string.IsNullOrEmpty(Remark))
            {
                this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, Remark);
            }
            if (!string.IsNullOrEmpty(Amount))
            {
                this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, Amount);
            }
            this.database.AddInParameter(sqlStringCommand, "CheckTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetRedpackRecordIsUsed(int id, bool issend)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_SendRedpackRecord set IsSend=@IsSend,SendTime=getdate() where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
            this.database.AddInParameter(sqlStringCommand, "IsSend", DbType.Boolean, issend);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateAmount(MemberAmountDetailedInfo model)
        {
            string query = "Update Hishop_MemberAmountDetailed set State=1,GatewayPayId=@GatewayPayId where State<>1 and Id=" + model.Id;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, model.State);
            this.database.AddInParameter(sqlStringCommand, "GatewayPayId", DbType.String, model.GatewayPayId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateMember(MemberAmountDetailedInfo model, DbTransaction dbTrans = null)
        {
            string query = "Update aspnet_Members set TotalAmount=TotalAmount+@TotalAmount,AvailableAmount=AvailableAmount+@TradeAmount where UserID=" + model.UserId;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, model.UserId);
            this.database.AddInParameter(sqlStringCommand, "TradeAmount", DbType.Decimal, model.TradeAmount);
            this.database.AddInParameter(sqlStringCommand, "TotalAmount", DbType.Decimal, (model.TradeAmount > 0M) ? model.TradeAmount : 0M);
            if (dbTrans == null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTrans) > 0);
        }

        public bool UseBalance(MemberAmountDetailedInfo mountInfo)
        {
            if (this.CreatAmount(mountInfo, null))
            {
                string query = "Update aspnet_Members set AvailableAmount=AvailableAmount+@TradeAmount where UserID=" + mountInfo.UserId;
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, mountInfo.UserId);
                this.database.AddInParameter(sqlStringCommand, "TradeAmount", DbType.Decimal, mountInfo.TradeAmount);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return false;
        }
    }
}

