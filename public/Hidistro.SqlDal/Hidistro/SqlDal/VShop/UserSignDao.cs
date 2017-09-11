namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class UserSignDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int InsertUserSign(UserSign us)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" INSERT INTO [dbo].[Hishop_UserSign] ");
            builder.Append(" ([UserID],[SignDay],[Continued],[Stage]) ");
            builder.Append(" VALUES ");
            builder.Append(" (@UserID,@SignDay,@Continued,@Stage) ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, us.UserID);
            this.database.AddInParameter(sqlStringCommand, "SignDay", DbType.Date, us.SignDay);
            this.database.AddInParameter(sqlStringCommand, "Continued", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "Stage", DbType.Int32, 0);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DataTable SignInfoByUser(int userID)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT *");
            builder.Append(" FROM [dbo].[Hishop_UserSign] ");
            builder.Append(" WHERE ");
            builder.Append(" [UserID]=").Append(userID);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public int UpdateUserSign(UserSign us)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" UPDATE Hishop_UserSign ");
            builder.Append(" SET  [SignDay] = @SignDay");
            builder.Append(" ,[Continued] = @Continued ");
            builder.Append(" ,[Stage] = @Stage ");
            builder.Append("  WHERE  ");
            builder.Append(" [UserID] = @UserID ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, us.UserID);
            this.database.AddInParameter(sqlStringCommand, "SignDay", DbType.Date, us.SignDay);
            this.database.AddInParameter(sqlStringCommand, "Continued", DbType.Int32, us.Continued);
            this.database.AddInParameter(sqlStringCommand, "Stage", DbType.Int32, us.Stage);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

