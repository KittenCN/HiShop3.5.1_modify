namespace Hidistro.SqlDal.CashBack
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.CashBack;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;

    public class CashBackDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddCashBack(CashBackInfo cashBackInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CashBack (UserId, RechargeAmount,CashBackAmount,Percentage,CashBackType,CreateDate,IsValid,IsFinished) VALUES(@UserId, @RechargeAmount,@CashBackAmount,@Percentage,@CashBackType,@CreateDate,@IsValid,@IsFinished)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, cashBackInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "RechargeAmount", DbType.Decimal, cashBackInfo.RechargeAmount);
            this.database.AddInParameter(sqlStringCommand, "CashBackAmount", DbType.Decimal, cashBackInfo.CashBackAmount);
            this.database.AddInParameter(sqlStringCommand, "Percentage", DbType.Decimal, cashBackInfo.Percentage);
            this.database.AddInParameter(sqlStringCommand, "CashBackType", DbType.Int32, (int) cashBackInfo.CashBackType);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, cashBackInfo.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "IsValid", DbType.Boolean, cashBackInfo.IsValid);
            this.database.AddInParameter(sqlStringCommand, "IsFinished", DbType.Boolean, cashBackInfo.IsFinished);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool AddCashBackDetails(CashBackDetailsInfo cashBackDetailsInfo, DbTransaction dbTrans = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CashBackDetails (CashBackId,CashBackAmount,CashBackType,CashBackDate) VALUES(@CashBackId,@CashBackAmount,@CashBackType,@CashBackDate)");
            this.database.AddInParameter(sqlStringCommand, "CashBackId", DbType.Int32, cashBackDetailsInfo.CashBackId);
            this.database.AddInParameter(sqlStringCommand, "CashBackAmount", DbType.Decimal, cashBackDetailsInfo.CashBackAmount);
            this.database.AddInParameter(sqlStringCommand, "CashBackType", DbType.Int32, (int) cashBackDetailsInfo.CashBackType);
            this.database.AddInParameter(sqlStringCommand, "CashBackDate", DbType.DateTime, cashBackDetailsInfo.CashBackDate);
            if (dbTrans == null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTrans) > 0);
        }

        public DbQueryResult GetCashBackByPager(CashBackQuery query)
        {
            string selectFields = "hcb.*,UserName,CellPhone";
            string table = "Hishop_CashBack hcb LEFT JOIN aspnet_Members am ON hcb.UserId=am.UserId";
            string pk = "CashBackId";
            string filter = " 1=1 ";
            if (query.UserId.HasValue)
            {
                filter = filter + string.Format(" AND hcb.UserId={0}", query.UserId.Value);
            }
            if (!string.IsNullOrWhiteSpace(query.UserName))
            {
                filter = filter + string.Format(" AND UserName LIKE '%{0}%'", query.UserName);
            }
            if (!string.IsNullOrWhiteSpace(query.Cellphone))
            {
                filter = filter + string.Format(" AND Cellphone LIKE '%{0}%'", query.Cellphone);
            }
            if (query.CashBackTypes.HasValue)
            {
                filter = filter + string.Format(" AND CashBackType = {0}", (int) query.CashBackTypes.Value);
            }
            if (query.IsValid.HasValue)
            {
                filter = filter + string.Format(" AND IsValid = {0}", query.IsValid.Value ? 1 : 0);
            }
            if (query.IsFinished.HasValue)
            {
                filter = filter + string.Format(" AND IsFinished = {0}", query.IsFinished.Value ? 1 : 0);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, pk, filter, selectFields);
        }

        public DbQueryResult GetCashBackDetailsByPager(CashBackDetailsQuery query)
        {
            string selectFields = "*";
            string table = "Hishop_CashBackDetails hcd";
            string pk = "CashBackDetailsId";
            string filter = " 1=1 ";
            filter = filter + string.Format(" AND CashBackId={0}", query.CashBackId);
            query.SortOrder = SortAction.Desc;
            query.SortBy = "CashBackDetailsId";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, pk, filter, selectFields);
        }

        public CashBackInfo GetCashBackInfo(int cashBackId)
        {
            CashBackInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CashBack WHERE CashBackId=@CashBackId");
            this.database.AddInParameter(sqlStringCommand, "CashBackId", DbType.Int32, cashBackId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if ((reader != null) && reader.Read())
                {
                    info = new CashBackInfo {
                        CashBackId = cashBackId,
                        UserId = (int) reader["UserId"],
                        CashBackAmount = (decimal) reader["CashBackAmount"],
                        RechargeAmount = (decimal) reader["RechargeAmount"],
                        Percentage = (decimal) reader["Percentage"],
                        CashBackType = (CashBackTypes) ((int) reader["CashBackType"]),
                        IsValid = (bool) reader["IsValid"],
                        IsFinished = (bool) reader["IsFinished"],
                        CreateDate = (DateTime) reader["CreateDate"]
                    };
                    if (DBNull.Value != reader["FinishedDate"])
                    {
                        info.FinishedDate = new DateTime?((DateTime) reader["FinishedDate"]);
                    }
                }
            }
            return info;
        }

        public IList<CashBackInfo> GetUnFinishedCashBackList()
        {
            IList<CashBackInfo> list = new List<CashBackInfo>();
            CashBackInfo item = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CashBack WHERE IsValid=1 AND IsFinished=0");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while ((reader != null) && reader.Read())
                {
                    item = new CashBackInfo {
                        CashBackId = (int) reader["CashBackId"],
                        UserId = (int) reader["UserId"],
                        CashBackAmount = (decimal) reader["CashBackAmount"],
                        RechargeAmount = (decimal) reader["RechargeAmount"],
                        Percentage = (decimal) reader["Percentage"],
                        CashBackType = (CashBackTypes) ((int) reader["CashBackType"]),
                        IsValid = (bool) reader["IsValid"],
                        IsFinished = (bool) reader["IsFinished"],
                        CreateDate = (DateTime) reader["CreateDate"]
                    };
                    if (DBNull.Value != reader["FinishedDate"])
                    {
                        item.FinishedDate = new DateTime?((DateTime) reader["FinishedDate"]);
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public bool UpdateCashBack(CashBackInfo cashBackInfo, DbTransaction dbTrans = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CashBack SET RechargeAmount=@RechargeAmount,IsFinished=@IsFinished,CashBackAmount=@CashBackAmount,Percentage=@Percentage,IsValid=@IsValid WHERE CashBackId=@CashBackId");
            this.database.AddInParameter(sqlStringCommand, "RechargeAmount", DbType.Decimal, cashBackInfo.RechargeAmount);
            this.database.AddInParameter(sqlStringCommand, "CashBackAmount", DbType.Decimal, cashBackInfo.CashBackAmount);
            this.database.AddInParameter(sqlStringCommand, "Percentage", DbType.Decimal, cashBackInfo.Percentage);
            this.database.AddInParameter(sqlStringCommand, "CashBackType", DbType.Int32, (int) cashBackInfo.CashBackType);
            this.database.AddInParameter(sqlStringCommand, "IsValid", DbType.Boolean, cashBackInfo.IsValid);
            this.database.AddInParameter(sqlStringCommand, "IsFinished", DbType.Boolean, cashBackInfo.IsFinished);
            this.database.AddInParameter(sqlStringCommand, "CashBackId", DbType.Int32, cashBackInfo.CashBackId);
            if (dbTrans == null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTrans) > 0);
        }
    }
}

