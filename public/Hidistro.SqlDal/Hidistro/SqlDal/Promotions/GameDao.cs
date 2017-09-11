namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public class GameDao
    {
        private Database _database = DatabaseFactory.CreateDatabase();

        public bool Create(GameInfo model, out int gameId)
        {
            gameId = -1;
            StringBuilder builder = new StringBuilder();
            builder.Append("if not exists(select top 1 gameid from Hishop_PromotionGame where KeyWork=@KeyWork) INSERT INTO [Hishop_PromotionGame]\r\n                       ([GameType]\r\n                       ,[GameTitle]\r\n                       ,[Description]\r\n                       ,[BeginTime]\r\n                       ,[EndTime]\r\n                       ,[ApplyMembers]\r\n                       ,[DefualtGroup]\r\n                       ,[CustomGroup]\r\n                       ,[NeedPoint]\r\n                       ,[GivePoint]\r\n                       ,[OnlyGiveNotPrizeMember]\r\n                       ,[PlayType]\r\n                       ,[NotPrzeDescription]\r\n                       ,[GameUrl]\r\n                       ,[GameQRCodeAddress]\r\n                       ,[Status],[KeyWork]\r\n                       ,[LimitEveryDay]\r\n                       ,[MaximumDailyLimit]\r\n                       ,[PrizeRate]\r\n                       ,[MemberCheck])\r\n                        VALUES ");
            builder.Append("(@GameType\r\n                       ,@GameTitle\r\n                       ,@Description\r\n                       ,@BeginTime\r\n                       ,@EndTime\r\n                       ,@ApplyMembers\r\n                       ,@DefualtGroup\r\n                       ,@CustomGroup\r\n                       ,@NeedPoint\r\n                       ,@GivePoint\r\n                       ,@OnlyGiveNotPrizeMember\r\n                       ,@PlayType\r\n                       ,@NotPrzeDescription\r\n                       ,@GameUrl\r\n                       ,@GameQRCodeAddress\r\n                       ,@Status,@KeyWork\r\n                       ,@LimitEveryDay\r\n                       ,@MaximumDailyLimit\r\n                       ,@PrizeRate\r\n                       ,@MemberCheck);");
            builder.Append("select @@identity;");
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
            this._database.AddInParameter(sqlStringCommand, "@GameType", DbType.Int32, (int) model.GameType);
            this._database.AddInParameter(sqlStringCommand, "@GameTitle", DbType.String, model.GameTitle);
            this._database.AddInParameter(sqlStringCommand, "@Description", DbType.String, model.Description);
            this._database.AddInParameter(sqlStringCommand, "@BeginTime", DbType.DateTime, model.BeginTime);
            this._database.AddInParameter(sqlStringCommand, "@EndTime", DbType.DateTime, model.EndTime);
            this._database.AddInParameter(sqlStringCommand, "@ApplyMembers", DbType.String, model.ApplyMembers);
            this._database.AddInParameter(sqlStringCommand, "@DefualtGroup", DbType.String, model.DefualtGroup);
            this._database.AddInParameter(sqlStringCommand, "@CustomGroup", DbType.String, model.CustomGroup);
            this._database.AddInParameter(sqlStringCommand, "@NeedPoint", DbType.Int32, model.NeedPoint);
            this._database.AddInParameter(sqlStringCommand, "@GivePoint", DbType.Int32, model.GivePoint);
            this._database.AddInParameter(sqlStringCommand, "@OnlyGiveNotPrizeMember", DbType.Boolean, model.OnlyGiveNotPrizeMember);
            this._database.AddInParameter(sqlStringCommand, "@PlayType", DbType.Int32, (int) model.PlayType);
            this._database.AddInParameter(sqlStringCommand, "@NotPrzeDescription", DbType.String, model.NotPrzeDescription);
            this._database.AddInParameter(sqlStringCommand, "@GameUrl", DbType.String, model.GameUrl);
            this._database.AddInParameter(sqlStringCommand, "@GameQRCodeAddress", DbType.String, model.GameQRCodeAddress);
            this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int32, 0);
            this._database.AddInParameter(sqlStringCommand, "@KeyWork", DbType.String, model.KeyWork);
            this._database.AddInParameter(sqlStringCommand, "@LimitEveryDay", DbType.String, model.LimitEveryDay);
            this._database.AddInParameter(sqlStringCommand, "@MaximumDailyLimit", DbType.String, model.MaximumDailyLimit);
            this._database.AddInParameter(sqlStringCommand, "@PrizeRate", DbType.String, model.PrizeRate);
            this._database.AddInParameter(sqlStringCommand, "@MemberCheck", DbType.String, model.MemberCheck);
            try
            {
                object obj2 = this._database.ExecuteScalar(sqlStringCommand);
                if ((obj2 != null) && !int.TryParse(obj2.ToString(), out gameId))
                {
                    gameId = -1;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return (gameId > 0);
        }

        public bool Delete(params int[] gameIds)
        {
            string str = string.Join<int>(",", gameIds);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Delete From Hishop_PromotionGameResultMembersLog where GameId in({0});", str);
            builder.AppendFormat("Delete From Hishop_PromotionGamePrizes where GameId in({0});", str);
            builder.AppendFormat("Delete From Hishop_PromotionGame where GameId in({0});", str);
            builder.AppendFormat("Delete From Hishop_PromotionWinningPool where GameId in({0});", str);
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeletePrizesDelivery(int[] ids)
        {
            string query = "delete from Hishop_PromotionGameResultMembersLog where LogId in(" + string.Join<int>(",", ids) + ");delete from Hishop_PrizesDeliveryRecord where LogId in(" + string.Join<int>(",", ids) + ")";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DbQueryResult GetAllPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (query.Status >= 0)
            {
                builder.Append(" AND ");
                builder.AppendFormat("status = {0}", query.Status);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" ProductName like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.Pid))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" Pid = '{0}'", query.Pid);
            }
            if (!string.IsNullOrEmpty(query.ActivityTitle))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" Title like '%{0}%'", DataHelper.CleanSearchString(query.ActivityTitle));
            }
            if (!string.IsNullOrEmpty(query.Receiver))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" Receiver = '{0}'", DataHelper.CleanSearchString(query.Receiver));
            }
            if (!string.IsNullOrEmpty(query.ReggionId))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" ',' + ReggionPath + ',' like '%,{0},%'", DataHelper.CleanSearchString(query.ReggionId));
            }
            DateTime now = DateTime.Now;
            if (DateTime.TryParse(query.StartDate, out now))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" WinTime>='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
            }
            if (DateTime.TryParse(query.EndDate, out now))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" WinTime<='{0}'", now.ToString("yyyy-MM-dd 23:59:59"));
            }
            if (ExtendLimits != "")
            {
                builder.Append(ExtendLimits);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Vshop_ProductPrizeLIst_WithDelievelyInfo", "WinTime", (builder.Length > 0) ? builder.ToString() : null, selectFields);
        }

        public GameInfo GetGameInfoById(int gameId)
        {
            string query = "Select * From [Hishop_PromotionGame] where GameId=@GameId";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
            this._database.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GameInfo>(reader);
            }
        }

        public DbQueryResult GetGameList(GameSearch search)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (search.GameType.HasValue)
            {
                builder.AppendFormat(" and [GameType]={0}", search.GameType);
            }
            if (!string.IsNullOrEmpty(search.Status))
            {
                builder.AppendFormat(" and [Status]={0}", search.Status);
            }
            if (search.BeginTime.HasValue)
            {
                builder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime);
            }
            if (search.EndTime.HasValue)
            {
                builder.AppendFormat(" and [EndTime]>'{0}'", search.EndTime);
            }
            string selectFields = "GameId, GameType, GameTitle, Description, BeginTime, EndTime, ApplyMembers,DefualtGroup,CustomGroup, NeedPoint, GivePoint, OnlyGiveNotPrizeMember, PlayType, NotPrzeDescription, GameUrl, GameQRCodeAddress, Status";
            return DataHelper.PagingByTopnotin(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "Hishop_PromotionGame", "GameId", builder.ToString(), selectFields);
        }

        public DbQueryResult GetGameListByView(GameSearch search)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (search.GameType.HasValue)
            {
                builder.AppendFormat(" and [GameType]={0}", search.GameType);
            }
            if (!string.IsNullOrEmpty(search.Status))
            {
                if (search.Status == "1")
                {
                    builder.AppendFormat(" and ([Status]={0} or EndTime<getdate())", search.Status);
                }
                else
                {
                    builder.AppendFormat(" and ([Status]={0} and EndTime>getdate())", search.Status);
                }
            }
            if (search.BeginTime.HasValue)
            {
                builder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime.Value.ToString("yyyy-MM-dd"));
            }
            if (search.EndTime.HasValue)
            {
                builder.AppendFormat(" and [EndTime]<'{0}'", search.EndTime.Value.AddDays(1.0).ToString("yyyy-MM-dd"));
            }
            string selectFields = " GameID, GameType,GameTitle, BeginTime ,EndTime,PlayType,GameUrl,GameQRCodeAddress ,Status,TotalCount,PrizeCount,LimitEveryDay,MaximumDailyLimit ";
            return DataHelper.PagingByTopnotin(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "vw_Hishop_PromotionGame", "GameId", builder.ToString(), selectFields);
        }

        public IEnumerable<GameInfo> GetLists(GameSearch search)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM [vw_Hishop_PromotionGame] where 1=1 ");
            if (!string.IsNullOrEmpty(search.Status))
            {
                builder.AppendFormat(" and [Status]={0}", search.Status);
            }
            if (search.BeginTime.HasValue)
            {
                builder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime);
            }
            if (search.EndTime.HasValue)
            {
                builder.AppendFormat(" and [EndTime]<'{0}'", search.EndTime);
            }
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<GameInfo>(reader);
            }
        }

        public GameInfo GetModelByGameId(int gameId)
        {
            string query = "SELECT * FROM [Hishop_PromotionGame] where GameId=@GameId";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
            this._database.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GameInfo>(reader);
            }
        }

        public GameInfo GetModelByGameId(string keyWord)
        {
            string query = "SELECT * FROM [Hishop_PromotionGame] where KeyWork=@KeyWork";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
            this._database.AddInParameter(sqlStringCommand, "@KeyWork", DbType.String, keyWord);
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GameInfo>(reader);
            }
        }

        public DataSet GetOrdersAndLines(string orderIds, string pIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            if (!string.IsNullOrEmpty(pIds) && !pIds.Contains("'"))
            {
                pIds = "'" + pIds.Replace(",", "','") + "'";
            }
            orderIds = !string.IsNullOrEmpty(orderIds) ? orderIds : "''";
            pIds = !string.IsNullOrEmpty(pIds) ? pIds : "''";
            Database database = DatabaseFactory.CreateDatabase();
            database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            if (orderIds == "''")
            {
                builder.AppendFormat("SELECT * FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo WHERE (LogId=0 AND Pid IN ({1})) order by  CourierNumber asc,DeliveryTime desc ", orderIds, pIds);
            }
            else
            {
                builder.AppendFormat("SELECT * FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo WHERE  LogId IN ({0}) OR (LogId=0 AND Pid IN ({1})) order by  CourierNumber asc,DeliveryTime desc ", orderIds, pIds);
            }
            Globals.Debuglog("发货单查询语句：" + builder.ToString(), "_SqlDebuglog.txt");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return database.ExecuteDataSet(sqlStringCommand);
        }

        public DataSet GetPrizeListByLogIDList(string orderIds, string pidlist)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            if (!string.IsNullOrEmpty(pidlist) && !pidlist.Contains("'"))
            {
                pidlist = "'" + pidlist.Replace(",", "','") + "'";
            }
            orderIds = !string.IsNullOrEmpty(orderIds) ? orderIds : "''";
            pidlist = !string.IsNullOrEmpty(pidlist) ? pidlist : "''";
            Database database = DatabaseFactory.CreateDatabase();
            string query = string.Empty;
            string str2 = "*";
            if (orderIds == "''")
            {
                query = string.Format("with v as (SELECT " + str2 + ", row_number() over (partition by Receiver+ReggionPath+ExpressName+[Address]+Tel order by  ReggionPath desc) as rownumber from vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where  ( LogId=0 AND Pid in ({1}) )) select " + str2 + ",rownumber from v", orderIds, pidlist);
            }
            else
            {
                query = string.Format("with v as (SELECT " + str2 + ", row_number() over (partition by Receiver+ReggionPath+ExpressName+[Address]+Tel order by  ReggionPath desc) as rownumber from vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where   LogId in ({0}) OR ( LogId=0 AND Pid in ({1}) )) select " + str2 + ",rownumber from v", orderIds, pidlist);
            }
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return database.ExecuteDataSet(sqlStringCommand);
        }

        public bool GetPrizesDeliveInfo(PrizesDeliveQuery query, out string GameTitle, out int UserId, out string prizeName)
        {
            GameTitle = "";
            UserId = 0;
            prizeName = "";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand("select top 1 GameTitle, UserId,ProductName from [vw_Hishop_PrizesDeliveryRecord]   where LogId=" + query.LogId.ToString() + " and  ID=" + query.Id.ToString());
            DataTable table = this._database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if ((table != null) && (table.Rows.Count > 0))
            {
                GameTitle = Convert.ToString(table.Rows[0]["GameTitle"]);
                UserId = Convert.ToInt32(table.Rows[0]["UserId"]);
                prizeName = Convert.ToString(table.Rows[0]["ProductName"]);
                return true;
            }
            return false;
        }

        public DbQueryResult GetPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (query.PrizeType > -1)
            {
                builder.Append(" AND ");
                builder.AppendFormat(" PrizeType = {0}", query.PrizeType);
            }
            if (query.Id > 0)
            {
                builder.Append(" AND ");
                builder.AppendFormat(" Id = {0}", query.Id);
            }
            if (query.UserId > 0)
            {
                builder.Append(" AND ");
                builder.AppendFormat(" UserId = {0}", query.UserId);
            }
            if (!string.IsNullOrEmpty(query.LogId))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" LogId = {0}", query.LogId);
            }
            if (query.Status >= 0)
            {
                builder.Append(" AND ");
                builder.AppendFormat("status = {0}", query.Status);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" ProductName like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.Receiver))
            {
                builder.Append(" AND ");
                builder.AppendFormat("( Receiver = '{0}'", DataHelper.CleanSearchString(query.Receiver));
                builder.AppendFormat(" OR RealName = '{0}' )", DataHelper.CleanSearchString(query.Receiver));
            }
            if (!string.IsNullOrEmpty(query.ReggionId))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" ',' + ReggionPath + ',' like '%,{0},%'", DataHelper.CleanSearchString(query.ReggionId));
            }
            DateTime now = DateTime.Now;
            if (DateTime.TryParse(query.StartDate, out now))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" PlayTime>='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
            }
            if (DateTime.TryParse(query.EndDate, out now))
            {
                builder.Append(" AND ");
                builder.AppendFormat(" PlayTime<='{0}'", now.ToString("yyyy-MM-dd 23:59:59"));
            }
            if (ExtendLimits != "")
            {
                builder.Append(ExtendLimits);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PrizesDeliveryRecord", "LogId", (builder.Length > 0) ? builder.ToString() : null, selectFields);
        }

        public DataTable GetPrizesDeliveryNum()
        {
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand("Select (SELECT count(LogId) FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where status=0 ) st0,(SELECT count(LogId) FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where status=1 ) st1,(SELECT count(LogId) FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where status=2 ) st2,(SELECT count(LogId) FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where status=3 ) st3");
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public List<GameWinningPool> GetWinningPoolList(int gameId)
        {
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(("SELECT * FROM Hishop_PromotionWinningPool where IsReceive=0 and GameId=" + gameId).ToString());
            using (IDataReader reader = this._database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<GameWinningPool>(reader).ToList<GameWinningPool>();
            }
        }

        public bool SetPrintOrderExpress(string orderId, string pId, string expressCompanyName, string expressCompanyAbb, string shipOrderNumber)
        {
            string query = string.Empty;
            if (string.IsNullOrEmpty(shipOrderNumber))
            {
                if (orderId != "0")
                {
                    query = "UPDATE Hishop_PrizesDeliveryRecord SET ExpressName=@ExpressName WHERE  LogID=@LogID";
                }
                else if (pId != "0")
                {
                    query = "UPDATE Hishop_PrizesDeliveryRecord SET ExpressName=@ExpressName WHERE  LogID=0 AND Pid=@Pid";
                }
            }
            else if (orderId != "0")
            {
                query = "UPDATE Hishop_PrizesDeliveryRecord SET IsPrinted=1,CourierNumber=@CourierNumber,ExpressName=@ExpressName WHERE  LogID=@LogID";
            }
            else if (pId != "0")
            {
                query = "UPDATE Hishop_PrizesDeliveryRecord SET IsPrinted=1,CourierNumber=@CourierNumber,ExpressName=@ExpressName WHERE  LogID=0   AND Pid=@Pid";
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            database.AddInParameter(sqlStringCommand, "LogID", DbType.String, orderId);
            database.AddInParameter(sqlStringCommand, "Pid", DbType.String, pId);
            database.AddInParameter(sqlStringCommand, "CourierNumber", DbType.String, shipOrderNumber);
            database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressCompanyName);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool Update(GameInfo model, bool isNotStart = true)
        {
            bool flag;
            StringBuilder builder = new StringBuilder();
            builder.Append("Update [Hishop_PromotionGame] set [GameTitle]=@GameTitle\r\n                       ,[Description]=@Description\r\n                       ,[BeginTime]=@BeginTime\r\n                       ,[EndTime]=@EndTime\r\n                       ,[ApplyMembers]=@ApplyMembers\r\n                       ,[DefualtGroup]=@DefualtGroup\r\n                       ,[CustomGroup]=@CustomGroup\r\n                       ,[NeedPoint]=@NeedPoint\r\n                       ,[GivePoint]=@GivePoint\r\n                       ,[OnlyGiveNotPrizeMember]=@OnlyGiveNotPrizeMember\r\n                       ,[PlayType]=@PlayType\r\n                       ,[NotPrzeDescription]=@NotPrzeDescription\r\n                       ,[GameUrl]=@GameUrl\r\n                       ,[GameQRCodeAddress]=@GameQRCodeAddress\r\n                       ,[LimitEveryDay]=@LimitEveryDay\r\n                       ,[MaximumDailyLimit]=@MaximumDailyLimit\r\n                       ,[PrizeRate]=@PrizeRate\r\n                       ,[MemberCheck]=@MemberCheck\r\n                       Where GameId=@GameId\r\n                        ");
            if (isNotStart)
            {
                builder.Append(" and getdate()< BeginTime");
            }
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
            this._database.AddInParameter(sqlStringCommand, "@GameTitle", DbType.String, model.GameTitle);
            this._database.AddInParameter(sqlStringCommand, "@Description", DbType.String, model.Description);
            this._database.AddInParameter(sqlStringCommand, "@BeginTime", DbType.DateTime, model.BeginTime);
            this._database.AddInParameter(sqlStringCommand, "@EndTime", DbType.DateTime, model.EndTime);
            this._database.AddInParameter(sqlStringCommand, "@ApplyMembers", DbType.String, model.ApplyMembers);
            this._database.AddInParameter(sqlStringCommand, "@DefualtGroup", DbType.String, model.DefualtGroup);
            this._database.AddInParameter(sqlStringCommand, "@CustomGroup", DbType.String, model.CustomGroup);
            this._database.AddInParameter(sqlStringCommand, "@NeedPoint", DbType.Int32, model.NeedPoint);
            this._database.AddInParameter(sqlStringCommand, "@GivePoint", DbType.Int32, model.GivePoint);
            this._database.AddInParameter(sqlStringCommand, "@OnlyGiveNotPrizeMember", DbType.Boolean, model.OnlyGiveNotPrizeMember);
            this._database.AddInParameter(sqlStringCommand, "@PlayType", DbType.Int32, (int) model.PlayType);
            this._database.AddInParameter(sqlStringCommand, "@NotPrzeDescription", DbType.String, Globals.SubStr(model.NotPrzeDescription, 100, ""));
            this._database.AddInParameter(sqlStringCommand, "@GameUrl", DbType.String, model.GameUrl);
            this._database.AddInParameter(sqlStringCommand, "@GameQRCodeAddress", DbType.String, model.GameQRCodeAddress);
            this._database.AddInParameter(sqlStringCommand, "@LimitEveryDay", DbType.Int32, model.LimitEveryDay);
            this._database.AddInParameter(sqlStringCommand, "@MaximumDailyLimit", DbType.Int32, model.MaximumDailyLimit);
            this._database.AddInParameter(sqlStringCommand, "@PrizeRate", DbType.Double, model.PrizeRate);
            this._database.AddInParameter(sqlStringCommand, "@MemberCheck", DbType.Int32, model.MemberCheck);
            this._database.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, model.GameId);
            try
            {
                flag = this._database.ExecuteNonQuery(sqlStringCommand) > 0;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public bool UpdateOutOfDateStatus()
        {
            string query = "Update Hishop_PromotionGame set [Status]=1 where EndTime<getdate()";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdatePrizesDelivery(PrizesDeliveQuery query)
        {
            DbCommand sqlStringCommand;
            StringBuilder builder = new StringBuilder();
            if (query.Id == 0)
            {
                builder.Append("insert into Hishop_PrizesDeliveryRecord");
                builder.Append("(Receiver,Tel,LogId,ReggionPath,Address,status,RecordType,Pid)VALUES");
                builder.Append("(@Receiver,@Tel,@LogId,@ReggionPath,@Address,@Status,@RecordType,@Pid)");
                builder.Append(";update Hishop_PromotionGameResultMembersLog set IsUsed=1 where LogId=" + query.LogId);
                sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
                this._database.AddInParameter(sqlStringCommand, "@Receiver", DbType.String, query.Receiver);
                this._database.AddInParameter(sqlStringCommand, "@Tel", DbType.String, query.Tel);
                this._database.AddInParameter(sqlStringCommand, "@LogId", DbType.Int32, query.LogId);
                this._database.AddInParameter(sqlStringCommand, "@ReggionPath", DbType.String, query.ReggionPath);
                this._database.AddInParameter(sqlStringCommand, "@Address", DbType.String, query.Address);
                this._database.AddInParameter(sqlStringCommand, "@RecordType", DbType.String, query.RecordType);
                if (string.IsNullOrEmpty(query.Pid))
                {
                    query.Pid = "0";
                }
                this._database.AddInParameter(sqlStringCommand, "@Pid", DbType.String, query.Pid);
                if (((query.Address.Length > 5) && (query.ReggionPath.Length > 0)) && ((query.Receiver.Length > 1) && (query.Tel.Length > 7)))
                {
                    this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int16, 1);
                }
                else
                {
                    this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int16, 0);
                }
            }
            else
            {
                builder.Append("Update Hishop_PrizesDeliveryRecord ");
                if (query.Status == 0)
                {
                    builder.Append("set status=status");
                }
                else
                {
                    builder.Append("set Status=@Status");
                }
                if (!string.IsNullOrEmpty(query.Receiver))
                {
                    builder.Append(",Receiver=@Receiver");
                }
                if (!string.IsNullOrEmpty(query.Tel))
                {
                    builder.Append(",Tel=@Tel");
                }
                if (!string.IsNullOrEmpty(query.ReggionPath))
                {
                    builder.Append(",ReggionPath=@ReggionPath");
                }
                if (!string.IsNullOrEmpty(query.ExpressName))
                {
                    builder.Append(",ExpressName=@ExpressName");
                }
                if (!string.IsNullOrEmpty(query.CourierNumber))
                {
                    builder.Append(",CourierNumber=@CourierNumber");
                }
                if (!string.IsNullOrEmpty(query.Address))
                {
                    builder.Append(",Address=@Address");
                }
                DateTime now = DateTime.Now;
                if (DateTime.TryParse(query.DeliveryTime, out now))
                {
                    builder.AppendFormat(",DeliveryTime='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
                }
                if (DateTime.TryParse(query.ReceiveTime, out now))
                {
                    builder.AppendFormat(",ReceiveTime='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
                }
                builder.Append(" where Id=@Id");
                if ((query.Status == 3) && (query.LogId != "0"))
                {
                    builder.Append(";update Hishop_PromotionGameResultMembersLog set IsUsed=1 where LogId=" + query.LogId);
                }
                sqlStringCommand = this._database.GetSqlStringCommand(builder.ToString());
                this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int16, query.Status);
                this._database.AddInParameter(sqlStringCommand, "@Address", DbType.String, query.Address);
                this._database.AddInParameter(sqlStringCommand, "@ExpressName", DbType.String, query.ExpressName);
                this._database.AddInParameter(sqlStringCommand, "@CourierNumber", DbType.String, query.CourierNumber);
                this._database.AddInParameter(sqlStringCommand, "@ReggionPath", DbType.String, query.ReggionPath);
                this._database.AddInParameter(sqlStringCommand, "@Receiver", DbType.String, query.Receiver);
                this._database.AddInParameter(sqlStringCommand, "@Tel", DbType.String, query.Tel);
                this._database.AddInParameter(sqlStringCommand, "@Id", DbType.Int32, query.Id);
            }
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateStatus(int gameId, GameStatus status)
        {
            string query = "Update Hishop_PromotionGame set [Status]=@Status where GameId=@GameId";
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
            this._database.AddInParameter(sqlStringCommand, "@Status", DbType.Int32, (int) status);
            this._database.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateWinningPoolIsReceive(int winningPoolId)
        {
            DbCommand sqlStringCommand = this._database.GetSqlStringCommand(("update Hishop_PromotionWinningPool set IsReceive=1 where WinningPoolId=" + winningPoolId).ToString());
            return (this._database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

