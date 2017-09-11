namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class IntegralDetailDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddIntegralDetail(IntegralDetailInfo point, DbTransaction dbTran = null)
        {
            string query = string.Concat(new object[] { "INSERT INTO vshop_IntegralDetail  ([IntegralSourceType],[IntegralSource],[IntegralChange],[Remark],[Userid],[GoToUrl],[IntegralStatus]) VALUES(@IntegralSourceType,@IntegralSource,@IntegralChange,@Remark,@Userid,@GoToUrl,@IntegralStatus); UPDATE dbo.aspnet_Members SET Points=Points+ ", Convert.ToInt32(point.IntegralChange), " WHERE UserId=", point.Userid });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "IntegralSourceType", DbType.Int32, point.IntegralSourceType);
            this.database.AddInParameter(sqlStringCommand, "IntegralSource", DbType.String, point.IntegralSource);
            this.database.AddInParameter(sqlStringCommand, "IntegralChange", DbType.Decimal, point.IntegralChange);
            this.database.AddInParameter(sqlStringCommand, "Userid", DbType.Int32, point.Userid);
            this.database.AddInParameter(sqlStringCommand, "GoToUrl", DbType.String, point.GoToUrl);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, point.Remark);
            this.database.AddInParameter(sqlStringCommand, "IntegralStatus", DbType.Int32, point.IntegralStatus);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DbQueryResult GetIntegralDetail(IntegralDetailQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.IntegralSourceType > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IntegralSourceType = {0}", query.IntegralSourceType);
            }
            if (query.IntegralStatus > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IntegralStatus = {0}", query.IntegralStatus);
            }
            if (query.StartTime.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("  datediff(dd,'{0}',TrateTime) >=0 ", DataHelper.GetSafeDateTimeFormat(query.StartTime.Value));
            }
            if (query.EndTime.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',TrateTime)<=0 ", DataHelper.GetSafeDateTimeFormat(query.EndTime.Value));
            }
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vshop_IntegralDetail", "Id", (builder.Length > 0) ? builder.ToString() : null, "*");
        }
    }
}

