namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.FenXiao;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class DistributorsDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        private static object RedLock = new object();

        public bool AddBalanceDrawRequest(BalanceDrawRequestInfo bdrinfo)
        {
            string query = "INSERT INTO Hishop_BalanceDrawRequest(UserId,bankName,RequestType,UserName,Amount,AccountName,CellPhone,MerchantCode,Remark,RequestTime,IsCheck) VALUES(@UserId,@bankName,@RequestType,@UserName,@Amount,@AccountName,@CellPhone,@MerchantCode,@Remark,getdate(),0)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, bdrinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "RequestType", DbType.Int32, bdrinfo.RequestType);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, bdrinfo.UserName);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, bdrinfo.Amount);
            this.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, bdrinfo.AccountName);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, bdrinfo.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, bdrinfo.MerchantCode);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, bdrinfo.Remark);
            this.database.AddInParameter(sqlStringCommand, "bankName", DbType.String, bdrinfo.BankName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool AddCommission(DistributorGradeCommissionInfo info)
        {
            string query = "UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+@Commission WHERE UserId=@UserID;";
            query = query + "select @@identity;INSERT INTO Hishop_Commissions(UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,State,CommRemark)values(@UserId,@ReferralUserId,@OrderID,@PubTime,0,@Commission,@CommType,1,@Memo);";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, info.UserId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, info.ReferralUserId);
            this.database.AddInParameter(sqlStringCommand, "Commission", DbType.Decimal, info.Commission);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, info.PubTime);
            this.database.AddInParameter(sqlStringCommand, "OperAdmin", DbType.String, info.OperAdmin);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, info.Memo);
            this.database.AddInParameter(sqlStringCommand, "OrderID", DbType.String, info.OrderID);
            this.database.AddInParameter(sqlStringCommand, "OldCommissionTotal", DbType.Decimal, info.OldCommissionTotal);
            this.database.AddInParameter(sqlStringCommand, "CommType", DbType.Int32, info.CommType);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void AddDistributorProducts(int productId, int distributorId)
        {
            string query = "INSERT INTO Hishop_DistributorProducts VALUES(@ProductId,@UserId)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public decimal CommionsRequestSumMoney(int userId)
        {
            string query = "SELECT isnull(sum(amount),0) FROM dbo.Hishop_BalanceDrawRequest WHERE IsCheck in(0,1,3) AND UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return decimal.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
        }

        public bool CreateDistributor(DistributorsInfo distributor)
        {
            if (distributor.UserId <= 0)
            {
                return false;
            }
            string query = "INSERT INTO aspnet_Distributors(UserId,StoreName,Logo,BackImage,RequestAccount,GradeId,ReferralUserId,ReferralPath, ReferralOrders,ReferralBlance, ReferralRequestBalance,ReferralStatus,StoreDescription,DistributorGradeId) VALUES(@UserId,@StoreName,@Logo,@BackImage,@RequestAccount,@GradeId,@ReferralUserId,@ReferralPath,@ReferralOrders,@ReferralBlance, @ReferralRequestBalance, @ReferralStatus,@StoreDescription,@DistributorGradeId)";
            if (!string.IsNullOrEmpty(distributor.CellPhone))
            {
                query = "UPDATE aspnet_Members set CellPhone=@CellPhone where UserId=@UserId;" + query;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, distributor.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "BackImage", DbType.String, distributor.BackImage);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, distributor.RequestAccount);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int64, (int) distributor.DistributorGradeId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int64, distributor.ParentUserId.Value);
            this.database.AddInParameter(sqlStringCommand, "ReferralPath", DbType.String, distributor.ReferralPath);
            this.database.AddInParameter(sqlStringCommand, "ReferralOrders", DbType.Int64, distributor.ReferralOrders);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, distributor.ReferralBlance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, distributor.ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.Int64, distributor.ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            this.database.AddInParameter(sqlStringCommand, "DistributorGradeId", DbType.Int64, distributor.DistriGradeId);
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
                            flag = dao.AddSendRedpackRecord(sendredpackinfo, dbTran);
                            return this.UpdateSendRedpackRecord(serialid, 1, dbTran);
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
                            int num6 = num4 + ((num3 > 0) ? 1 : 0);
                            flag = this.UpdateSendRedpackRecord(serialid, num6, dbTran);
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

        public bool DeleteCustomDistributorStatistic(string id)
        {
            string query = "DELETE FROM Hishop_CustomDistributor WHERE id IN (" + id + ");";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int EditCommisionsGrade(string userids, string Grade)
        {
            string query = "UPDATE aspnet_Distributors set DistributorGradeId=@DistributorGradeId WHERE UserId in(" + userids + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "DistributorGradeId", DbType.String, Grade);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool EditDisbutosInfos(string userid, string QQNum, string CellPhone, string RealName, string Password)
        {
            string query = "UPDATE aspnet_Members set QQ=@QQ,Password=@Password,CellPhone=@CellPhone,RealName=@RealName WHERE UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userid);
            this.database.AddInParameter(sqlStringCommand, "QQ", DbType.String, QQNum);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, CellPhone);
            this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, RealName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, Password);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool FrozenCommision(int userid, string ReferralStatus)
        {
            string query = "UPDATE aspnet_Distributors set ReferralStatus=@ReferralStatus WHERE UserId=@UserId ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.String, ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int FrozenCommisionChecks(string userids, string ReferralStatus)
        {
            string query = "UPDATE aspnet_Distributors set ReferralStatus=@ReferralStatus WHERE UserId in(" + userids + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.String, ReferralStatus);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DataTable GetAllDistributorsName(string keywords)
        {
            DataTable table = new DataTable();
            string[] strArray = Regex.Split(DataHelper.CleanSearchString(keywords), @"\s+");
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(DataHelper.CleanSearchString(strArray[0])));
            for (int i = 1; (i < strArray.Length) && (i <= 5); i++)
            {
                builder.AppendFormat(" OR StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 10 StoreName,UserName from vw_Hishop_DistributorsMembers WHERE " + builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query, string[] extendCheckStatus = null)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
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
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            if (extendCheckStatus != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.Append(" IsCheck in (" + string.Join(",", extendCheckStatus) + ") ");
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
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BalanceDrawRequesDistributors ", "SerialID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public BalanceDrawRequestInfo GetBalanceDrawRequestById(string SerialID)
        {
            if (string.IsNullOrEmpty(SerialID))
            {
                return null;
            }
            BalanceDrawRequestInfo info = new BalanceDrawRequestInfo();
            string query = "select a.*, b.StoreName, c.OpenId as UserOpenId  from Hishop_BalanceDrawRequest  a  \r\n                     left join aspnet_Distributors b on a.UserId=b.UserId \r\n                     left join aspnet_Members c on a.UserId= c.UserId\r\n                  WHERE a.SerialID=@SerialID";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, SerialID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<BalanceDrawRequestInfo>(reader);
            }
            if (((info != null) && ((info.RequestType == 3) || (info.RequestType == 0))) && string.IsNullOrEmpty(info.RedpackId))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                string str2 = this.CreatRedpackId(masterSettings.WeixinPartnerID);
                query = "update Hishop_BalanceDrawRequest set RedpackId=@redId where SerialID=@SerialID and RedpackId  is null ";
                sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, SerialID);
                this.database.AddInParameter(sqlStringCommand, "redId", DbType.String, str2);
                if (this.database.ExecuteNonQuery(sqlStringCommand) > 0)
                {
                    info.RedpackId = str2;
                }
            }
            return info;
        }

        public bool GetBalanceDrawRequestIsCheck(int serialid)
        {
            string query = "select IsCheck from Hishop_BalanceDrawRequest where SerialID=" + serialid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)).ToString() == "2");
        }

        public int GetBalanceDrawRequestIsCheckStatus(int serialid)
        {
            string query = "select IsCheck from Hishop_BalanceDrawRequest where SerialID=" + serialid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return int.Parse(Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)).ToString());
        }

        public DbQueryResult GetCommissions(CommissionsQuery query)
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
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.OrderNum))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" OrderId = '{0}'", query.OrderNum);
            }
            if (!string.IsNullOrEmpty(query.StartTime))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',TradeTime)>=0", query.StartTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("  datediff(dd,'{0}',TradeTime)<=0", query.EndTime);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CommissionDistributors", "CommId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult GetCommissionsWithStoreName(CommissionsQuery query, string level = "")
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
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("fromStoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(level))
            {
                if (level == "4")
                {
                    builder.Append(" and UserID=ReferralUserId AND CommType in (3,4,5)");
                }
                else if (level == "5")
                {
                    if (!string.IsNullOrEmpty(query.StoreName))
                    {
                        builder.Append(" and CommType not in (3,4,5) ");
                    }
                    else
                    {
                        builder.Append(" and UserID=ReferralUserId and CommType not in (3,4,5) ");
                    }
                }
                else if (level == "0")
                {
                    if (string.IsNullOrEmpty(query.StoreName))
                    {
                        builder.Append(" and UserID=ReferralUserId  and CommType<>3  and CommType<>5 ");
                    }
                }
                else if (level == "1")
                {
                    builder.AppendFormat(" and (ReferralPath='{0}' or ReferralPath like '%|{0}') ", query.UserId);
                }
                else if (level == "2")
                {
                    builder.AppendFormat(" and ReferralPath like '{0}|%'", query.UserId);
                }
                else if (level == "3")
                {
                    builder.AppendFormat(" and (UserID<>ReferralUserId or CommType=3)", query.UserId);
                }
            }
            if (query.ReferralUserId > 0)
            {
                builder.AppendFormat(" and ReferralUserId = {0}", query.ReferralUserId);
            }
            if (!string.IsNullOrEmpty(query.OrderNum))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" OrderId = '{0}'", query.OrderNum);
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
            string table = "vw_Hishop_CommissionDistributorsAddFromStore";
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                table = "vw_Hishop_CommissionDistributorsOnlyForStoreName";
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "CommId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable GetCurrentDistributorsCommosion(int userId)
        {
            string query = string.Format("SELECT sum(OrderTotal) AS OrderTotal,sum(CommTotal) AS CommTotal from dbo.Hishop_Commissions where UserId={0} AND OrderId in (select OrderId from dbo.Hishop_Orders where ReferralUserId={0})", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set != null) && (set.Tables.Count > 0))
            {
                return set.Tables[0];
            }
            return null;
        }

        public DataTable GetCustomDistributorStatistic(int id)
        {
            string query = "select * from dbo.Hishop_CustomDistributor where  id = " + id;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetCustomDistributorStatistic(string storeName)
        {
            string query = ("select id,storename from dbo.Hishop_CustomDistributor where  storename = '" + storeName + "'") + "union all select UserId as id,storename from dbo.aspnet_Distributors where  storename = '" + storeName + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetCustomDistributorStatisticList()
        {
            DbQueryResult result = new DbQueryResult();
            string query = "select  * from dbo.Hishop_CustomDistributor order by commtotalsum desc ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            result.Data = table;
            return result;
        }

        public IList<DistributorGradeInfo> GetDistributorGrades()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrade");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<DistributorGradeInfo>(reader);
            }
        }

        public DistributorsInfo GetDistributorInfo(int distributorId)
        {
            if (distributorId <= 0)
            {
                return null;
            }
            DistributorsInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Distributors where UserId={0} ", distributorId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateDistributorInfo(reader);
                }
            }
            return info;
        }

        public DistributorsInfo GetDistributorInfoByStoreName(string storeName)
        {
            if (string.IsNullOrEmpty(storeName))
            {
                return null;
            }
            DistributorsInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Distributors where storename='{0}'", storeName));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateDistributorInfo(reader);
                }
            }
            return info;
        }

        public int GetDistributorNum(DistributorGrade grade)
        {
            int num = 0;
            string query = string.Format("SELECT COUNT(*) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", Globals.GetCurrentMemberUserId(false));
            if (grade != DistributorGrade.All)
            {
                query = query + " AND GradeId=" + ((int) grade);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = (int) reader[0];
                    reader.Close();
                }
            }
            return num;
        }

        public DbQueryResult GetDistributors(DistributorsQuery query, string TopUserId = null, string level = null)
        {
            StringBuilder builder = new StringBuilder();
            if (query.GradeId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("DistributorGradeId = {0}", query.GradeId);
            }
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            if (query.ReferralStatus > -1)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("ReferralStatus = '{0}'", query.ReferralStatus);
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CellPhone='{0}'", DataHelper.CleanSearchString(query.CellPhone));
            }
            if (!string.IsNullOrEmpty(query.MicroSignal))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserBindName LIKE '%{0}%'", DataHelper.CleanSearchString(query.MicroSignal));
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            int userId = 0;
            if (!string.IsNullOrEmpty(query.StoreName1))
            {
                DistributorsInfo distributorInfoByStoreName = new DistributorsDao().GetDistributorInfoByStoreName(query.StoreName1);
                if (distributorInfoByStoreName != null)
                {
                    userId = distributorInfoByStoreName.UserId;
                    if (builder.Length > 0)
                    {
                        builder.AppendFormat(" AND ", new object[0]);
                    }
                    builder.AppendFormat("(ReferralPath='{0}' OR ReferralPath LIKE '%|{0}' )", DataHelper.CleanSearchString(userId.ToString()));
                }
            }
            else if (!string.IsNullOrEmpty(query.StoreName) && string.IsNullOrEmpty(query.StoreName1))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.ReferralPath))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("(ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
            }
            if (!string.IsNullOrEmpty(TopUserId) && !string.IsNullOrEmpty(level))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (level == "1")
                {
                    builder.AppendFormat("(ReferralPath='{0}' OR ReferralPath LIKE '%|{0}' )", DataHelper.CleanSearchString(TopUserId));
                }
                else if (level == "2")
                {
                    builder.AppendFormat(" ReferralPath like '{0}|%' ", DataHelper.CleanSearchString(TopUserId));
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_DistributorsMembers", "UserId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable GetDistributorSaleinfo(string startTime, string endTime, int[] UserIds)
        {
            DateTime time;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("select UserId,StoreName,Logo,COUNT(DISTINCT case  userid when ReferralUserId then OrderId else null end) as Ordernums,COUNT(DISTINCT case  userid when ReferralUserId then BuyUserId else null end) as BuyUserIds,");
            builder.AppendLine(" SUM(case userid when ReferralUserId then OrderTotal else 0 end ) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
            builder.AppendLine(" from vw_Hishop_CommissionWithBuyUserId  where 1=1 ");
            builder.AppendLine(" and userid in(" + string.Join<int>(",", UserIds) + ") ");
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and TradeTime>='{0}' ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  TradeTime<='{0}' ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" group by UserId,StoreName,Logo ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetDistributorsCommission(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            string str = "";
            if (query.GradeId > 0)
            {
                builder.AppendFormat("AND GradeId = {0}", query.GradeId);
            }
            if (!string.IsNullOrEmpty(query.ReferralPath))
            {
                builder.AppendFormat(" AND (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
            }
            if (query.UserId > 0)
            {
                str = " UserId=" + query.UserId + " AND ";
            }
            string str2 = string.Concat(new object[] { "select TOP ", query.PageSize, " UserId,StoreName,GradeId,CreateTime,isnull((select SUM(OrderTotal) from Hishop_Commissions where ", str, " ReferralUserId=aspnet_Distributors.UserId),0) as OrderTotal,isnull((select SUM(CommTotal) from Hishop_Commissions where ", str, " ReferralUserId=aspnet_Distributors.UserId),0) as  CommTotal from aspnet_Distributors WHERE ", builder.ToString(), " order by CreateTime  desc" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetDistributorsCommosion(int userId)
        {
            string query = string.Format("SELECT  GradeId,COUNT(*),SUM(OrdersTotal) AS OrdersTotal,SUM(ReferralOrders) AS ReferralOrders,SUM(ReferralBlance) AS ReferralBlance,SUM(ReferralRequestBalance) AS ReferralRequestBalance FROM aspnet_Distributors WHERE ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}' GROUP BY GradeId", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set != null) && (set.Tables.Count > 0))
            {
                return set.Tables[0];
            }
            return null;
        }

        public DataTable GetDistributorsCommosion(int userId, DistributorGrade grade)
        {
            string query = string.Format("SELECT sum(OrderTotal) AS OrderTotal,sum(CommTotal) AS CommTotal from dbo.Hishop_Commissions where UserId={0} AND ReferralUserId in (select UserId from aspnet_Distributors  WHERE (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}') and GradeId={1})", userId, (int) grade);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set != null) && (set.Tables.Count > 0))
            {
                return set.Tables[0];
            }
            return null;
        }

        public DataTable GetDistributorsNum()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Select (SELECT count(UserId) FROM vw_Hishop_DistributorsMembers where ReferralStatus=0) active,(SELECT count(UserId) FROM vw_Hishop_DistributorsMembers where ReferralStatus=1) frozen");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetDistributorsRankings(string startTime, string endTime, int pgSize, int CurrPage)
        {
            DateTime time;
            DataTable table;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("with  cr as  ( ");
            builder.AppendLine("select top " + (pgSize * (CurrPage - 1)) + " UserId from vw_Hishop_CommissionWithBuyUserId  where 1=1 ");
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and TradeTime>='{0}' ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  TradeTime<='{0}' ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" group by UserId  order by SUM(case  userid when ReferralUserId then OrderTotal else 0 end) desc )");
            builder.AppendLine(" select a.*,d.StoreName ,d.Logo,d.BackImage ,d.RequestAccount ,d.AccountTime ,d.GradeId,d.ReferralUserId,d.ReferralPath,d.OrdersTotal,d.ReferralOrders,d.ReferralBlance,d.ReferralRequestBalance ,d.CreateTime,d.ReferralStatus ,d.StoreDescription ,d.DistributorGradeId,m.UserName,m.CreateDate,m.RealName,m.CellPhone,m.QQ,m.OpenId,m.MicroSignal,m.UserHead from");
            builder.AppendLine(" (select top " + pgSize + " UserId,COUNT(DISTINCT case  when userid=ReferralUserId and CommType=1  then OrderId else null end) as Ordernums,COUNT(DISTINCT case  userid when ReferralUserId then BuyUserId else null end) as BuyUserIds,");
            builder.AppendLine(" SUM(case  userid when ReferralUserId then OrderTotal else 0 end) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
            builder.AppendLine(" from vw_Hishop_CommissionWithBuyUserId  where 1=1 ");
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and TradeTime>='{0}' ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  TradeTime<='{0}' ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" and UserId not in(select UserId from cr)");
            builder.AppendLine(" group by UserId  order by OrderTotalSum desc)  a");
            builder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
            builder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            string query = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ";
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                query = query + string.Format(" and TradeTime>='{0}' ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                query = query + string.Format("  and  TradeTime<='{0}' ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            DbCommand command = this.database.GetSqlStringCommand(query);
            result.TotalRecords = int.Parse(this.database.ExecuteScalar(command).ToString());
            result.Data = table;
            return result;
        }

        public DbQueryResult GetDistributorsRankingsY(string startTime, string endTime, int pgSize, int CurrPage)
        {
            DateTime time;
            DataTable table;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("with  cr as  ( ");
            builder.AppendLine("select top " + (pgSize * (CurrPage - 1)) + " UserId from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ");
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and TradeTime>='{0}' ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  TradeTime<='{0}' ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" group by UserId  order by SUM(OrderTotal) desc )");
            builder.AppendLine(" select a.*,d.StoreName ,d.Logo,d.BackImage ,d.RequestAccount ,d.AccountTime ,d.GradeId,d.ReferralUserId,d.ReferralPath,d.OrdersTotal,d.ReferralOrders,d.ReferralBlance,d.ReferralRequestBalance ,d.CreateTime,d.ReferralStatus ,d.StoreDescription ,d.DistributorGradeId,m.UserName,m.CreateDate,m.RealName,m.CellPhone,m.QQ,m.OpenId,m.MicroSignal,m.UserHead from");
            builder.AppendLine(" (select top " + pgSize + " UserId,COUNT(OrderId) as Ordernums,COUNT(DISTINCT BuyUserId) as BuyUserIds,");
            builder.AppendLine(" SUM(OrderTotal) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
            builder.AppendLine(" from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ");
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and TradeTime>='{0}' ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  TradeTime<='{0}' ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" and UserId not in(select UserId from cr)");
            builder.AppendLine(" group by UserId  order by SUM(OrderTotal) desc)  a");
            builder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
            builder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            string query = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ";
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                query = query + string.Format(" and TradeTime>='{0}' ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                query = query + string.Format("  and  TradeTime<='{0}' ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            DbCommand command = this.database.GetSqlStringCommand(query);
            result.TotalRecords = int.Parse(this.database.ExecuteScalar(command).ToString());
            result.Data = table;
            return result;
        }

        public DataTable GetDistributorsSubStoreNum(int topUserId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Select (SELECT count(UserId) FROM vw_Hishop_DistributorsMembers where  (ReferralPath='{0}' or ReferralPath like '%|{0}')) firstV,(SELECT count(UserId) FROM  vw_Hishop_DistributorsMembers where  ReferralPath like '{0}|%') secondV ,( select count(*) from aspnet_Members  s left join aspnet_Distributors d on s.userid=d.userid  where  s.Status in(1,9) and s.ReferralUserId={0} and d.StoreName is null) memberCount", topUserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int GetDistributorsSubStoreNumN(int topUserId, int grade, string startTime, string endTime)
        {
            DateTime time;
            string query = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionWithBuyUserId  where userid=ReferralUserId ";
            if (grade == 1)
            {
                query = query + string.Format(" and ReferralPath='{0}' or ReferralPath like '%|{0}'", topUserId);
            }
            else
            {
                query = query + string.Format("  and ReferralPath like '{0}|%'", topUserId);
            }
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                query = query + string.Format(" and TradeTime>='{0}' ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                query = query + string.Format("  and  TradeTime<='{0}' ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
        }

        public int GetDistributorSuperiorId(int userid)
        {
            string query = "select top 1 ReferralUserId from aspnet_Distributors where UserId=" + userid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public int GetDownDistributorNum(string userid)
        {
            int num = 0;
            string query = string.Format("SELECT COUNT(*) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = (int) reader[0];
                    reader.Close();
                }
            }
            return num;
        }

        public int GetDownDistributorNumReferralOrders(string userid)
        {
            int num = 0;
            string query = string.Format("SELECT isnull(sum(ReferralOrders),0) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if (set.Tables[0].Rows.Count > 0)
            {
                num = int.Parse(set.Tables[0].Rows[0][0].ToString());
            }
            return num;
        }

        public DataTable GetDownDistributors(DistributorsQuery query, out int total, string sort, string order)
        {
            StringBuilder builder = new StringBuilder(" 1=1 ");
            string str = "";
            if (query.GradeId > 0)
            {
                if (query.GradeId == 2)
                {
                    builder.AppendFormat(" AND ( ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
                }
                if (query.GradeId == 3)
                {
                    builder.AppendFormat(" AND ReferralPath LIKE '{0}|%' ", DataHelper.CleanSearchString(query.ReferralPath));
                }
            }
            int num = (query.PageIndex * query.PageSize) + 1;
            int num2 = (num + query.PageSize) - 1;
            if (query.UserId > 0)
            {
                str = " UserId=" + query.UserId + " AND ";
            }
            total = 0;
            string str2 = " SELECT count(*) FROM aspnet_Distributors where " + builder;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                total = (int) obj2;
            }
            string str3 = string.Concat(new object[] { "select * from ( select ROW_NUMBER()  OVER (ORDER BY ", sort, " ", order, ") as [RowIndex],* From ( select * from (select  UserId,StoreName,GradeId,CreateTime,ReferralPath,Logo, (select StoreName from aspnet_Distributors where   UserId= s.ReferralUserId ) as LStoreName, (select count(*) from aspnet_Distributors where  ReferralUserId = s.UserId ) as disTotal, (select count(*) from aspnet_Members where ReferralUserId=s.UserId) as MemberTotal, (SELECT Name FROM aspnet_DistributorGrade WHERE GradeId = s.DistributorGradeId) AS GradeName, (select UserName from aspnet_Members where   UserId=s.UserId) as UserName, isnull((select SUM(OrderTotal) from Hishop_Commissions where ", str, " ReferralUserId=s.UserId),0) as OrderTotal, isnull((select SUM(CommTotal) from Hishop_Commissions where ", str, " ReferralUserId=s.UserId),0) as  CommTotal from aspnet_Distributors as s WHERE ", builder.ToString(), ") as w ) AS M ) AS K  WHERE  RowIndex between ", num, " and ", num2 });
            DbCommand command = this.database.GetSqlStringCommand(str3);
            return this.database.ExecuteDataSet(command).Tables[0];
        }

        public DataTable GetDrawRequestNum(int[] CheckValues)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select IsCheck,COUNT(SerialID) as num from Hishop_BalanceDrawRequest where IsCheck in(" + string.Join<int>(",", CheckValues) + ")  group by IsCheck order by IsCheck");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public Dictionary<int, bool> GetExistDistributorList(string userIds)
        {
            string query = "SELECT Distinct(UserId) as UserId FROM aspnet_Distributors WHERE UserId IN (" + userIds + ")  ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set != null) && (set.Tables.Count > 0))
            {
                DataTable table = set.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    dictionary.Add((int) row[0], true);
                }
            }
            return dictionary;
        }

        public DistributorGradeInfo GetIsDefaultDistributorGradeInfo()
        {
            DistributorGradeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_DistributorGrade where IsDefault=1 order by CommissionsLimit asc", new object[0]));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateDistributorGradeInfo(reader);
                }
            }
            return info;
        }

        public Dictionary<int, int> GetMulBalanceDrawRequestIsCheckStatus(int[] serialids)
        {
            string query = "select IsCheck,SerialID from Hishop_BalanceDrawRequest where SerialID in(" + string.Join<int>(",", serialids) + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    dictionary.Add((int) row["SerialID"], (int) row["IsCheck"]);
                }
            }
            return dictionary;
        }

        public DbQueryResult GetSubDistributorsContribute(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
        {
            DateTime time;
            DataTable table;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("with  cr as  ( ");
            builder.AppendFormat("select top " + (pgSize * (CurrPage - 1)) + " UserId from vw_Hishop_CommissionWithReferralPath  where (CUserId={0} or CUserId is null) and ", belongUserId);
            if (grade == 1)
            {
                builder.AppendFormat(" ( ReferralPath='{0}' or ReferralPath like '%|{0}' )", belongUserId);
            }
            else
            {
                builder.AppendFormat("  ReferralPath like '{0}|%'", belongUserId);
            }
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and (TradeTime>='{0}' or TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  (TradeTime<='{0}' or  TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" group by UserId  order by SUM(OrderTotal) desc,UserId asc )");
            builder.AppendLine(" select a.*,d.StoreName ,d.Logo,d.BackImage ,d.RequestAccount ,d.AccountTime ,d.GradeId,d.ReferralUserId,d.ReferralPath,d.OrdersTotal,d.ReferralOrders,d.ReferralBlance,d.ReferralRequestBalance ,d.CreateTime,d.ReferralStatus ,d.StoreDescription ,d.DistributorGradeId,m.UserName,m.CreateDate,m.RealName,m.CellPhone,m.QQ,m.OpenId,m.MicroSignal,m.UserHead from");
            builder.AppendLine(" (select top " + pgSize + " UserId,COUNT(OrderId) as Ordernums,COUNT(DISTINCT BuyUserId) as BuyUserIds,");
            builder.AppendLine(" SUM(OrderTotal) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
            builder.AppendFormat(" from vw_Hishop_CommissionWithReferralPath  where (CUserId={0} or CUserId is null) and ", belongUserId);
            if (grade == 1)
            {
                builder.AppendFormat("  ( ReferralPath='{0}' or ReferralPath like '%|{0}') ", belongUserId);
            }
            else
            {
                builder.AppendFormat("  ReferralPath like '{0}|%'", belongUserId);
            }
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and (TradeTime>='{0}' or  TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  (TradeTime<='{0}' or  TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" and UserId not in(select UserId from cr)");
            builder.AppendLine(" group by UserId  order by SUM(OrderTotal) desc,UserId asc)  a");
            builder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
            builder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            string query = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionWithReferralPath  where   ";
            query = query + string.Format("  (CUserId={0} or CUserId is null) and ", belongUserId);
            if (grade == 1)
            {
                query = query + string.Format(" (ReferralPath='{0}' or ReferralPath like '%|{0}')", belongUserId);
            }
            else
            {
                query = query + string.Format("   ReferralPath like '{0}|%'", belongUserId);
            }
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                query = query + string.Format(" and (TradeTime>='{0}'  or TradeTime is null)", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                query = query + string.Format("  and  (TradeTime<='{0}' or  TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            DbCommand command = this.database.GetSqlStringCommand(query);
            result.TotalRecords = int.Parse(this.database.ExecuteScalar(command).ToString());
            result.Data = table;
            return result;
        }

        public DbQueryResult GetSubDistributorsRankingsN(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
        {
            DateTime time;
            DataTable table;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("with  cr as  ( ");
            builder.AppendLine("select top " + (pgSize * (CurrPage - 1)) + "  UserId from vw_Hishop_CommissionRanking  where 1=1 and ");
            if (grade == 1)
            {
                builder.AppendFormat("( ReferralPath='{0}' or ReferralPath like '%|{0}' )", belongUserId);
            }
            else
            {
                builder.AppendFormat(" ReferralPath like '{0}|%'", belongUserId);
            }
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and (TradeTime>='{0}' or TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  (TradeTime<='{0}' or  TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" group by UserId  order by SUM(case  userid when ReferralUserId then OrderTotal else 0 end) desc,UserId asc )");
            builder.AppendLine(" select a.*,d.StoreName ,d.Logo,d.BackImage ,d.RequestAccount ,d.AccountTime ,d.GradeId,d.ReferralUserId,d.ReferralPath,d.OrdersTotal,d.ReferralOrders,d.ReferralBlance,d.ReferralRequestBalance ,d.CreateTime,d.ReferralStatus ,d.StoreDescription ,d.DistributorGradeId,m.UserName,m.CreateDate,m.RealName,m.CellPhone,m.QQ,m.OpenId,m.MicroSignal,m.UserHead from");
            builder.AppendLine(" (select top " + pgSize + " UserId,COUNT(DISTINCT case  userid when ReferralUserId then OrderId else null end) as Ordernums,COUNT(DISTINCT case  userid when ReferralUserId then BuyUserId else null end) as BuyUserIds,");
            builder.AppendLine(" SUM(case  userid when ReferralUserId then OrderTotal else 0 end) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
            builder.AppendLine(" from vw_Hishop_CommissionRanking  where 1=1 ");
            if (grade == 1)
            {
                builder.AppendFormat(" and ( ReferralPath='{0}' or ReferralPath like '%|{0}') ", belongUserId);
            }
            else
            {
                builder.AppendFormat(" and ReferralPath like '{0}|%'", belongUserId);
            }
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                builder.AppendFormat(" and (TradeTime>='{0}' or  TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                builder.AppendFormat("  and  (TradeTime<='{0}' or  TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            builder.AppendLine(" and UserId not in(select UserId from cr)");
            builder.AppendLine(" group by UserId  order by OrderTotalSum desc,UserId asc)  a");
            builder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
            builder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            string query = "select  count(DISTINCT (UserId)) from vw_Hishop_CommissionRanking  where userid=ReferralUserId ";
            if (grade == 1)
            {
                query = query + string.Format("  and  (ReferralPath='{0}' or ReferralPath like '%|{0}')", belongUserId);
            }
            else
            {
                query = query + string.Format("   and  ReferralPath like '{0}|%'", belongUserId);
            }
            if ((startTime != null) && DateTime.TryParse(startTime, out time))
            {
                query = query + string.Format(" and (TradeTime>='{0}'  or TradeTime is null)", time.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            if ((endTime != null) && DateTime.TryParse(endTime, out time))
            {
                query = query + string.Format("  and  (TradeTime<='{0}' or  TradeTime is null) ", time.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            DbCommand command = this.database.GetSqlStringCommand(query);
            result.TotalRecords = int.Parse(this.database.ExecuteScalar(command).ToString());
            result.Data = table;
            return result;
        }

        public int GetSystemDistributorsCount()
        {
            string query = "select count(*) as num from aspnet_Distributors";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !string.IsNullOrEmpty(obj2.ToString()))
            {
                return int.Parse(obj2.ToString());
            }
            return 0;
        }

        public DataTable GetThreeDistributors(DistributorsQuery query, out int total)
        {
            StringBuilder builder = new StringBuilder("1=1");
            if (query.GradeId > 0)
            {
                builder.AppendFormat(" AND ReferralUserId={0} ", query.UserId);
            }
            int num = (query.PageIndex * query.PageSize) + 1;
            int num2 = (num + query.PageSize) - 1;
            total = 0;
            string str = " SELECT count(*) FROM aspnet_Distributors where " + builder;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                total = (int) obj2;
            }
            string str2 = string.Concat(new object[] { "select * from (select ROW_NUMBER()  OVER (ORDER BY UserId) as [RowIndex] , UserId,StoreName,GradeId,CreateTime,ReferralPath,Logo, (select StoreName from aspnet_Distributors where   UserId= s.ReferralUserId ) as LStoreName, (select count(*) from aspnet_Distributors where  ReferralUserId = s.UserId ) as disTotal, (select count(*) from aspnet_Members where ReferralUserId=s.UserId) as MemberTotal, (SELECT Name FROM aspnet_DistributorGrade WHERE GradeId = s.DistributorGradeId) AS GradeName, (select UserName from aspnet_Members where   UserId=s.UserId) as UserName, isnull((select SUM(OrderTotal) from Hishop_Commissions where  ReferralUserId=s.UserId),0) as OrderTotal, isnull((select SUM(CommTotal) from Hishop_Commissions where  ReferralUserId=s.UserId),0) as  CommTotal from aspnet_Distributors as s WHERE ", builder.ToString(), ") as w where w.RowIndex BETWEEN ", num, " AND ", num2, " order by CommTotal  desc" });
            DbCommand command = this.database.GetSqlStringCommand(str2);
            return this.database.ExecuteDataSet(command).Tables[0];
        }

        public decimal GetUserCommissions(int userid, DateTime fromdatetime, string enddatetime = null, string storeName = null, string OrderNum = null, string level = "")
        {
            DateTime time;
            StringBuilder builder = new StringBuilder();
            builder.Append(" State=1 ");
            if (userid > 0)
            {
                builder.Append(" and UserID=" + userid);
            }
            builder.Append(" and TradeTime>='" + fromdatetime.ToString("yyyy-MM-dd") + " 00:00:00'");
            if (!string.IsNullOrEmpty(level))
            {
                if (level == "0")
                {
                    builder.Append(" and UserID=ReferralUserId AND CommType not in (3,4,5)");
                }
                else if (level == "1")
                {
                    builder.AppendFormat(" and (ReferralPath='{0}' or ReferralPath like '%|{0}') ", userid);
                }
                else if (level == "2")
                {
                    builder.AppendFormat(" and ReferralPath like '{0}|%'", userid);
                }
            }
            if (!string.IsNullOrEmpty(storeName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("fromStoreName LIKE '%{0}%'", DataHelper.CleanSearchString(storeName));
            }
            if (!string.IsNullOrEmpty(OrderNum))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" OrderId = '{0}'", OrderNum);
            }
            if ((enddatetime != null) && DateTime.TryParse(enddatetime, out time))
            {
                builder.Append(" and TradeTime < '" + time.AddDays(1.0).ToString("yyyy-MM-dd") + " 00:00:00'");
            }
            string query = " select isnull(sum(CommTotal),0) from vw_Hishop_CommissionDistributorsAddFromStore where " + builder.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return decimal.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
        }

        public DataSet GetUserRanking(int userid)
        {
            string query = string.Format("select top 1 w.UserId,w.Logo,w. Blance,w.StoreName,  ( select (( select count(0) from aspnet_Distributors a where (a.ReferralBlance+a.ReferralRequestBalance)>w.Blance or (a.ReferralBlance+a.ReferralRequestBalance=w.Blance ))+(select count(0) from Hishop_CustomDistributor where commtotalsum>w.Blance or commtotalsum=w.Blance)))as ccount  from (select d.UserId,d.Logo,d.ReferralBlance+d.ReferralRequestBalance as Blance,d.StoreName from aspnet_Distributors d union all select id as UserId,Logo,commtotalsum as Blance,StoreName from  Hishop_CustomDistributor ) as w  where UserID={0} select top 10 * from (select UserId,Logo,ReferralBlance+ReferralRequestBalance as Blance,StoreName  from aspnet_Distributors where ReferralStatus!=9  union all select id+10086 as UserId,Logo,commtotalsum as Blance,StoreName from  Hishop_CustomDistributor ) as w  order by Blance desc,userid asc select top 10 UserId,Logo,ReferralBlance+ReferralRequestBalance as Blance,StoreName  from aspnet_Distributors where (ReferralPath like '{0}|%' or ReferralPath like '%|{0}' or ReferralPath = '{0}') and ReferralStatus!=9  order by Blance desc", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public bool InsertCustomDistributorStatistic(CustomDistributorStatistic custom)
        {
            string query = "insert into Hishop_CustomDistributor(storename,ordernums,commtotalsum,logo) values(@storename,@ordernums,@commtotalsum,@logo)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "storename", DbType.String, custom.StoreName);
            this.database.AddInParameter(sqlStringCommand, "ordernums", DbType.Int32, custom.OrderNums);
            this.database.AddInParameter(sqlStringCommand, "commtotalsum", DbType.Decimal, custom.CommTotalSum);
            this.database.AddInParameter(sqlStringCommand, "logo", DbType.String, custom.Logo);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int IsExistUsers(string userIds)
        {
            string query = "SELECT count(*) FROM aspnet_Distributors WHERE UserId IN (" + userIds + ") AND ReferralStatus=0";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public int IsExiteDistributorsByStoreName(string storname)
        {
            string query = "SELECT UserId FROM aspnet_Distributors WHERE StoreName=@StoreName";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, DataHelper.CleanSearchString(storname));
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 == null)
            {
                return 0;
            }
            return (int) obj2;
        }

        public bool IsExitsCommionsRequest(int userId)
        {
            bool flag = false;
            string query = "SELECT * FROM dbo.Hishop_BalanceDrawRequest WHERE IsCheck in(0,1,3) AND UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    flag = true;
                }
            }
            return flag;
        }

        public DataTable OrderIDGetCommosion(string orderid)
        {
            string query = string.Format("SELECT CommId,Userid,OrderTotal,CommTotal from Hishop_Commissions where OrderId='" + orderid + "' ", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set != null) && (set.Tables.Count > 0))
            {
                return set.Tables[0];
            }
            return null;
        }

        public void RemoveDistributorProducts(List<int> productIds, int distributorId)
        {
            string str = string.Join<int>(",", productIds);
            string query = "DELETE FROM Hishop_DistributorProducts WHERE UserId=@UserId AND ProductId IN (" + str + ");";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DataTable SelectDistributors(DistributorsQuery query, string TopUserId = null, string level = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.MicroSignal))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" OR ");
                }
                builder.AppendFormat("UserBindName LIKE '%{0}%'", DataHelper.CleanSearchString(query.MicroSignal));
            }
            string str = "Select * from vw_Hishop_DistributorsMembers Where " + builder.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public bool SelectDistributorsAliOpenId(ref List<string> SendToUserList)
        {
            string query = "Select  b.AlipayOpenid\r\n                from aspnet_Distributors a left join aspnet_Members  b  on a.UserId= b.UserId\r\n                Where a.ReferralStatus<=1 and (b.AlipayOpenid is not null and b.AlipayOpenid<>'' )";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                SendToUserList.Add(row["AlipayOpenid"].ToString());
            }
            return true;
        }

        public bool SelectDistributorsOpenId(ref List<string> SendToUserList)
        {
            string query = "Select  b.OpenId\r\n                from aspnet_Distributors a left join aspnet_Members  b  on a.UserId= b.UserId\r\n                Where a.ReferralStatus<=1 and (b.OpenId is not null and b.OpenId<>'' )";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                SendToUserList.Add(row["OpenId"].ToString());
            }
            return true;
        }

        public string SendRedPackToBalanceDrawRequest(int serialid)
        {
            if (!SettingsManager.GetMasterSettings(false).EnableWeiXinRequest)
            {
                return "管理员后台未开启微信付款！";
            }
            string query = "select a.SerialID,a.userid,a.Amount,b.OpenID,isnull(b.OpenId,'') as OpenId from Hishop_BalanceDrawRequest a inner join aspnet_Members b on a.userid=b.userid where SerialID=@SerialID and IsCheck=1";
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
                query = "select top 1 ID from vshop_SendRedpackRecord where BalanceDrawRequestID=" + table.Rows[0]["SerialID"].ToString();
                sqlStringCommand = this.database.GetSqlStringCommand(query);
                if (this.database.ExecuteDataSet(sqlStringCommand).Tables[0].Rows.Count > 0)
                {
                    return "-1";
                }
                return (this.CreateSendRedpackRecord(serialid, userid, str3, amount, "您的提现申请已成功", "恭喜您提现成功!") ? "1" : "提现操作失败");
            }
            return "该用户没有提现申请,或者提现申请未审核";
        }

        public bool SetBalanceDrawRequestIsCheckStatus(int[] serialid, int checkValue, string Remark = null, string Amount = null)
        {
            string query = "UPDATE Hishop_BalanceDrawRequest set IsCheck=@IsCheck ";
            if (Remark != null)
            {
                query = query + ",Remark=@Remark ";
            }
            if (Amount != null)
            {
                query = query + ",Amount=@Amount ";
            }
            query = query + " where IsCheck not in(-1,2) and  SerialID in(" + string.Join<int>(",", serialid) + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "IsCheck", DbType.Int16, checkValue);
            if (Remark != null)
            {
                this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, Remark);
            }
            if (Amount != null)
            {
                this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, Amount);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateBalanceDistributors(int UserId, decimal ReferralRequestBalance)
        {
            string query = "UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance-@ReferralBlance,ReferralRequestBalance=ReferralRequestBalance+@ReferralRequestBalance  WHERE UserId=@UserId ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateBalanceDrawRequest(int Id, string Remark, string CheckTime = null)
        {
            if (CheckTime == null)
            {
                CheckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                DateTime now = DateTime.Now;
                if (DateTime.TryParse(CheckTime, out now))
                {
                    CheckTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return false;
                }
            }
            string query = "UPDATE Hishop_BalanceDrawRequest set Remark=@Remark,IsCheck=2,CheckTime=@CheckTime WHERE SerialID=@SerialID and IsCheck<>2 ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, Remark);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, Id);
            this.database.AddInParameter(sqlStringCommand, "CheckTime", DbType.String, CheckTime);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateCalculationCommission(string UserId, string ReferralUserId, string OrderId, decimal OrderTotal, decimal resultCommTatal)
        {
            string query = "";
            object obj2 = query + "begin try  " + "  begin tran TranUpdate";
            object obj3 = string.Concat(new object[] { obj2, " INSERT INTO   [Hishop_Commissions](UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,CommRemark,State)values(", UserId, ",", ReferralUserId, ",'", OrderId, "','", DateTime.Now.ToString(), "',", OrderTotal, ",", resultCommTatal, ",1,'',1) ;" });
            object obj4 = string.Concat(new object[] { obj3, "  UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+", resultCommTatal, "  WHERE UserId=", UserId, "; " });
            query = string.Concat(new object[] { obj4, "  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+", OrderTotal, ",ReferralOrders=ReferralOrders+1  WHERE UserId=", ReferralUserId, "; " }) + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateCustomDistributorStatistic(CustomDistributorStatistic custom)
        {
            string query = "update Hishop_CustomDistributor set storename=@storename,ordernums=@ordernums,commtotalsum=@commtotalsum,logo=@logo where id=@id";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "storename", DbType.String, custom.StoreName);
            this.database.AddInParameter(sqlStringCommand, "ordernums", DbType.Int32, custom.OrderNums);
            this.database.AddInParameter(sqlStringCommand, "commtotalsum", DbType.Decimal, custom.CommTotalSum);
            this.database.AddInParameter(sqlStringCommand, "logo", DbType.String, custom.Logo);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, custom.id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributor(DistributorsInfo distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET StoreName=@StoreName,Logo=@Logo,BackImage=@BackImage,RequestAccount=@RequestAccount,ReferralOrders=@ReferralOrders,ReferralBlance=@ReferralBlance, ReferralRequestBalance=@ReferralRequestBalance,StoreDescription=@StoreDescription,ReferralStatus=@ReferralStatus WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "BackImage", DbType.String, distributor.BackImage);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, distributor.RequestAccount);
            this.database.AddInParameter(sqlStringCommand, "ReferralOrders", DbType.Int64, distributor.ReferralOrders);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.Int64, distributor.ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, distributor.ReferralBlance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, distributor.ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributorById(string requestAccount, int distributorId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET RequestAccount=@RequestAccount WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, requestAccount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributorMessage(DistributorsInfo distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members set CellPhone=@CellPhone where UserId=@UserId;UPDATE aspnet_Distributors SET StoreName=@StoreName,Logo=@Logo,StoreDescription=@StoreDescription,RequestAccount=@RequestAccount WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, distributor.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, distributor.RequestAccount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public string UpdateDistributorSuperior(int userid, int tosuperuserid)
        {
            string str = string.Empty;
            StringBuilder builder = new StringBuilder();
            DistributorsInfo distributorInfo = this.GetDistributorInfo(userid);
            if (distributorInfo != null)
            {
                builder.Append(string.Concat(new object[] { "会员（ID=", userid.ToString(), "）原来的上级ID是：", distributorInfo.UserId, ";分销商名称是：", distributorInfo.StoreName, ";" }));
            }
            DistributorsInfo info2 = this.GetDistributorInfo(tosuperuserid);
            if (info2 != null)
            {
                string str2 = string.Empty;
                int referralUserId = info2.ReferralUserId;
                if (referralUserId == tosuperuserid)
                {
                    str2 = tosuperuserid.ToString();
                }
                else
                {
                    str2 = referralUserId.ToString() + "|" + tosuperuserid.ToString();
                }
                Database database = DatabaseFactory.CreateDatabase();
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    new StringBuilder();
                    try
                    {
                        try
                        {
                            string query = string.Concat(new object[] { "update aspnet_Members set ReferralUserId=", tosuperuserid, " where userid=", userid });
                            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
                            database.ExecuteNonQuery(sqlStringCommand, transaction);
                            builder.Append(query + ";");
                            string str4 = tosuperuserid + "|" + userid;
                            query = string.Concat(new object[] { "update aspnet_Distributors set ReferralPath='", str4, "' where ReferralUserId=", userid });
                            sqlStringCommand = database.GetSqlStringCommand(query);
                            database.ExecuteNonQuery(sqlStringCommand, transaction);
                            builder.Append(query + ";");
                            query = string.Concat(new object[] { "update aspnet_Distributors set ReferralPath='", str2, "',ReferralUserId=", tosuperuserid.ToString(), " where UserID=", userid });
                            sqlStringCommand = database.GetSqlStringCommand(query);
                            database.ExecuteNonQuery(sqlStringCommand, transaction);
                            builder.Append(query + ";");
                            transaction.Commit();
                            Globals.Debuglog(builder.ToString(), "_DebuglogModifyDistributorRefer.txt");
                            str = "1";
                        }
                        catch
                        {
                            str = "执行出错";
                            transaction.Rollback();
                        }
                        return str;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return str;
        }

        public bool UpdateGradeId(ArrayList GradeIdList, ArrayList ReferralUserIdList)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < ReferralUserIdList.Count; i++)
            {
                if (!GradeIdList[i].Equals(0))
                {
                    object obj2 = query;
                    query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors SET DistributorGradeId=", GradeIdList[i], " where UserId=", ReferralUserIdList[i], "; " });
                }
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateNotSetCalculationCommission(ArrayList UserIdList, ArrayList OrdersTotal)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set OrdersTotal=OrdersTotal+", OrdersTotal[i], " WHERE UserId=", UserIdList[i], "; " });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateRedPackStatus(int Id, string Remark, string CheckTime = null)
        {
            if (CheckTime == null)
            {
                CheckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                DateTime now = DateTime.Now;
                if (DateTime.TryParse(CheckTime, out now))
                {
                    CheckTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return false;
                }
            }
            string query = "UPDATE Hishop_BalanceDrawRequest set Remark=@Remark,IsCheck=3,CheckTime=@CheckTime WHERE SerialID=@SerialID and IsCheck=2 ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, Remark);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, Id);
            this.database.AddInParameter(sqlStringCommand, "CheckTime", DbType.String, CheckTime);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateSendRedpackRecord(int serialid, int num, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BalanceDrawRequest set RedpackRecordNum=@RedpackRecordNum where SerialID=@SerialID");
            this.database.AddInParameter(sqlStringCommand, "RedpackRecordNum", DbType.Int32, num);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, serialid);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void UpdateSetCardCreatTime()
        {
            string query = "update aspnet_Distributors set CardCreatTime='2012-08-08' where CardCreatTime is not null ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateStoreCard(int userId, string imgUrl)
        {
            string query = "  UPDATE aspnet_Distributors SET StoreCard='" + imgUrl + "',CardCreatTime='" + DateTime.Now.ToString("G") + "' where UserId=" + userId.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateTwoCalculationCommission(ArrayList UserIdList, string ReferralUserId, string OrderId, ArrayList OrderTotalList, ArrayList CommTatalList)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = query;
                object obj3 = string.Concat(new object[] { obj2, " INSERT INTO   [Hishop_Commissions](UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,CommRemark,State)values(", UserIdList[i], ",", ReferralUserId, ",'", OrderId, "','", DateTime.Now.ToString(), "',", OrderTotalList[i], ",", CommTatalList[i], ",1,'',1) ;" });
                object obj4 = string.Concat(new object[] { obj3, "  UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+", CommTatalList[i], "  WHERE UserId=", UserIdList[i], "; " });
                query = string.Concat(new object[] { obj4, "  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+", OrderTotalList[i], ",ReferralOrders=ReferralOrders+1  WHERE UserId=", UserIdList[i], "; " });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateTwoDistributorsOrderNum(ArrayList useridList, ArrayList OrdersTotalList)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < useridList.Count; i++)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+", useridList[i], ",ReferralOrders=ReferralOrders+1  WHERE UserId=", OrdersTotalList[i], "; " });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

