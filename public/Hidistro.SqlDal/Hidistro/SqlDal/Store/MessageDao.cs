namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class MessageDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool Create(ManagerInfo manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (RoleId, UserName, Password, Email, CreateDate) VALUES (@RoleId, @UserName, @Password, @Email, @CreateDate)");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, manager.RoleId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, manager.UserName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, manager.Password);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, manager.Email);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, manager.CreateDate);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteManager(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_Managers WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ManagerInfo GetManager(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ManagerInfo>(reader);
            }
        }

        public ManagerInfo GetManager(string userName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserName = @UserName");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ManagerInfo>(reader);
            }
        }

        public DbQueryResult GetManagers(ManagerQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            if (query.RoleId != 0)
            {
                builder.AppendFormat(" AND RoleId = {0}", query.RoleId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Managers", "UserId", builder.ToString(), "*");
        }

        public bool Update(ManagerInfo manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Managers SET RoleId = @RoleId, UserName = @UserName, Password = @Password, Email = @Email WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, manager.UserId);
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, manager.RoleId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, manager.UserName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, manager.Password);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, manager.Email);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

