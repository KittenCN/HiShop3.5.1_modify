namespace Hidistro.SqlDal.Settings
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Settings;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class CustomerServiceDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        public string Error = "";

        public int CreateCustomer(CustomerServiceInfo info, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT userver  FROM MeiQia_Userver WHERE userver=@Name");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, info.userver);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    msg = "登录手机号重复";
                    return 0;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [MeiQia_Userver]([unit],[userver],[password],[nickname],[realname],[level],[tel]) VALUES (@unit,@userver,@password,@nickname,@realname,@level,@tel); SELECT CAST(scope_identity() AS int);");
                this.database.AddInParameter(sqlStringCommand, "unit", DbType.String, info.unit);
                this.database.AddInParameter(sqlStringCommand, "userver", DbType.String, info.userver);
                this.database.AddInParameter(sqlStringCommand, "password", DbType.String, info.password);
                this.database.AddInParameter(sqlStringCommand, "nickname", DbType.String, info.nickname);
                this.database.AddInParameter(sqlStringCommand, "realname", DbType.String, info.realname);
                this.database.AddInParameter(sqlStringCommand, "level", DbType.String, info.level);
                this.database.AddInParameter(sqlStringCommand, "tel", DbType.String, info.tel);
                int num = (int) this.database.ExecuteScalar(sqlStringCommand);
                msg = "";
                return num;
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return 0;
            }
        }

        public bool DeletCustomer(int id)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Delete  FROM MeiQia_Userver WHERE id = @ID");
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }

        public CustomerServiceInfo GetCustomer(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM MeiQia_Userver WHERE id = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CustomerServiceInfo>(reader);
            }
        }

        public DataTable GetCustomers(string unit)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select * from  MeiQia_Userver where unit=@unit", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "unit", DbType.String, unit);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool UpdateCustomer(CustomerServiceInfo info, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT userver  FROM MeiQia_Userver WHERE userver=@Name and id <> @ID");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, info.userver);
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, info.id);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    msg = "登录手机号重复";
                    return false;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [MeiQia_Userver] SET [unit] = @unit,[userver] = @userver,[password] = @password,[nickname] = @nickname,[realname] = @realname,[level] = @level,[tel] = @tel WHERE id=@id");
                this.database.AddInParameter(sqlStringCommand, "unit", DbType.String, info.unit);
                this.database.AddInParameter(sqlStringCommand, "userver", DbType.String, info.userver);
                this.database.AddInParameter(sqlStringCommand, "password", DbType.String, info.password);
                this.database.AddInParameter(sqlStringCommand, "nickname", DbType.String, info.nickname);
                this.database.AddInParameter(sqlStringCommand, "realname", DbType.String, info.realname);
                this.database.AddInParameter(sqlStringCommand, "level", DbType.String, info.level);
                this.database.AddInParameter(sqlStringCommand, "tel", DbType.String, info.tel);
                this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, info.id);
                int num = this.database.ExecuteNonQuery(sqlStringCommand);
                msg = "";
                return (num > 0);
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return false;
            }
        }
    }
}

