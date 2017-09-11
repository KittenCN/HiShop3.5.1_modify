namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class AlarmDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool Delete(int id)
        {
            string query = string.Format("DELETE FROM vshop_AlarmNotify WHERE AlarmNotifyId = {0}", id);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DbQueryResult List(int pageIndex, int pageSize)
        {
            return DataHelper.PagingByRownumber(pageIndex, pageSize, "AlarmNotifyId", SortAction.Desc, true, "vshop_AlarmNotify p", "AlarmNotifyId", " 1=1 ", "*");
        }

        public bool Save(AlarmInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_AlarmNotify (AppId, ErrorType, Description, AlarmContent, TimeStamp) VALUES (@AppId, @ErrorType, @Description, @AlarmContent, @TimeStamp)");
            this.database.AddInParameter(sqlStringCommand, "AppId", DbType.String, info.AppId);
            this.database.AddInParameter(sqlStringCommand, "ErrorType", DbType.Int32, info.ErrorType);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, info.Description);
            this.database.AddInParameter(sqlStringCommand, "AlarmContent", DbType.String, info.AlarmContent);
            this.database.AddInParameter(sqlStringCommand, "TimeStamp", DbType.DateTime, info.TimeStamp);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

