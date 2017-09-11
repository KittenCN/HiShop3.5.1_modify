namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class PrizeResultDao
    {
        private Database _db = DatabaseFactory.CreateDatabase();

        public bool AddPrizeLog(PrizeResultInfo model)
        {
            DbCommand storedProcCommand = this._db.GetStoredProcCommand("cp_GamePrize");
            this._db.AddInParameter(storedProcCommand, "@GameId", DbType.Int32, model.GameId);
            this._db.AddInParameter(storedProcCommand, "@PrizeId", DbType.Int32, model.PrizeId);
            this._db.AddInParameter(storedProcCommand, "@UserId", DbType.Int32, model.UserId);
            this._db.AddOutParameter(storedProcCommand, "@Result", DbType.Int32, 4);
            this._db.ExecuteNonQuery(storedProcCommand);
            object obj2 = storedProcCommand.Parameters["@Result"].Value;
            return (((obj2 != null) && !string.IsNullOrEmpty(obj2.ToString())) && (int.Parse(obj2.ToString()) > 0));
        }

        public DbQueryResult GetPrizeLogLists(PrizesDeliveQuery query)
        {
            string selectFields = "LogId, PlayTime, UserId, UserName, PrizeGrade, PrizeType, GivePoint, GiveCouponId, GiveShopBookId,Prize ";
            StringBuilder builder = new StringBuilder(" 1=1 ");
            if (query.GameId.HasValue)
            {
                builder.AppendFormat(" and GameId={0}", query.GameId);
            }
            if (query.IsUsed.HasValue)
            {
                builder.AppendFormat(" and IsUsed={0}", query.IsUsed);
            }
            if (!string.IsNullOrEmpty(query.StartDate))
            {
                builder.AppendFormat(" and PlayTime>='{0}'", query.StartDate);
            }
            if (!string.IsNullOrEmpty(query.EndDate))
            {
                builder.AppendFormat(" and PlayTime<'{0}'", query.EndDate);
            }
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PrizesRecord", "LogId", builder.ToString(), selectFields);
        }

        public IList<PrizeResultViewInfo> GetPrizeLogLists(int gameId, int pageIndex, int pageSize)
        {
            List<PrizeResultViewInfo> list = new List<PrizeResultViewInfo>();
            string selectFields = "LogId, PlayTime, UserId, UserName,Prize, PrizeGrade, PrizeType, GivePoint, GiveCouponId, GiveShopBookId,PrizeId ";
            string filter = string.Format(" GameId={0} and  PrizeId!=0 ", gameId);
            DataTable data = DataHelper.PagingByTopnotin(pageIndex, pageSize, "LogId", SortAction.Desc, false, "vw_Hishop_PrizesRecord", "LogId", filter, selectFields).Data as DataTable;
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    PrizeResultViewInfo item = new PrizeResultViewInfo {
                        LogId = int.Parse(row["LogId"].ToString()),
                        PlayTime = DateTime.Parse(row["PlayTime"].ToString()),
                        UserId = int.Parse(row["UserId"].ToString()),
                        UserName = row["UserName"].ToString(),
                        PrizeType = (PrizeType) int.Parse(row["PrizeType"].ToString()),
                        PrizeGrade = (PrizeGrade) int.Parse(row["PrizeGrade"].ToString()),
                        GivePoint = int.Parse(row["GivePoint"].ToString()),
                        GiveCouponId = row["GiveCouponId"].ToString(),
                        GiveShopBookId = row["GiveShopBookId"].ToString(),
                        PrizeName = row["Prize"].ToString()
                    };
                    list.Add(item);
                }
            }
            return list;
        }

        public bool IsCanPrize(int gameId, int userid)
        {
            DbCommand storedProcCommand = this._db.GetStoredProcCommand("cp_IsCanPrize");
            this._db.AddInParameter(storedProcCommand, "@GameId", DbType.Int32, gameId);
            this._db.AddInParameter(storedProcCommand, "@UserId", DbType.Int32, userid);
            this._db.AddOutParameter(storedProcCommand, "@Result", DbType.Int32, 4);
            this._db.ExecuteNonQuery(storedProcCommand);
            object obj2 = storedProcCommand.Parameters["@Result"].Value;
            if ((obj2 == null) || string.IsNullOrEmpty(obj2.ToString()))
            {
                return false;
            }
            CanPrizeError error = (CanPrizeError) int.Parse(obj2.ToString());
            if (error != CanPrizeError.可以玩)
            {
                throw new Exception(error.ToString());
            }
            return true;
        }

        public bool UpdatePrizeLogStatus(int logId)
        {
            string query = "Update Hishop_PromotionGameResultMembersLog set IsUsed=1 where LogId=@LogId and IsUsed=0";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
            this._db.AddInParameter(sqlStringCommand, "@LogId", DbType.Int32, logId);
            return (this._db.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

