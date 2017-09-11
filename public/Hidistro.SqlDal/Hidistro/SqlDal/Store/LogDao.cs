namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class LogDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DeleteAllLogs()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("TRUNCATE TABLE Hishop_Logs");
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteLog(long logId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Logs WHERE LogId = @LogId");
            this.database.AddInParameter(sqlStringCommand, "LogId", DbType.Int64, logId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public int DeleteLogs(string strIds)
        {
            if (strIds.Length <= 0)
            {
                return 0;
            }
            string query = string.Format("DELETE FROM Hishop_Logs WHERE LogId IN ({0})", strIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DbQueryResult GetLogs(OperationLogQuery query)
        {
            StringBuilder builder = new StringBuilder();
            Pagination page = query.Page;
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat("AddedTime >= '{0}'", query.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            }
            if (query.ToDate.HasValue)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append(" AND");
                }
                builder.AppendFormat(" AddedTime <= '{0}'", query.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            }
            if (!string.IsNullOrEmpty(query.OperationUserName))
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append(" AND");
                }
                builder.AppendFormat(" UserName = '{0}'", DataHelper.CleanSearchString(query.OperationUserName));
            }
            return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Logs", "LogId", builder.ToString(), "*");
        }

        public IList<string> GetOperationUserNames()
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT UserName FROM aspnet_Managers");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader["UserName"].ToString());
                }
            }
            return list;
        }

        public void WriteOperationLogEntry(OperationLogEntry entry)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Logs]([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description]) VALUES(@PageUrl,@AddedTime,@UserName,@IPAddress,@Privilege,@Description)");
            this.database.AddInParameter(sqlStringCommand, "PageUrl", DbType.String, entry.PageUrl);
            this.database.AddInParameter(sqlStringCommand, "AddedTime", DbType.DateTime, entry.AddedTime);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, entry.UserName);
            this.database.AddInParameter(sqlStringCommand, "IPAddress", DbType.String, entry.IpAddress);
            this.database.AddInParameter(sqlStringCommand, "Privilege", DbType.Int32, (int) entry.Privilege);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, entry.Description);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

