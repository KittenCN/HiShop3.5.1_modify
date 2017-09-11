namespace Hidistro.SqlDal.Sales
{
    using Entities.Settings;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class ExpressTemplateDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddExpressTemplate(string expressName, string xmlFile)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ExpressTemplates(ExpressName, XmlFile, IsUse) VALUES(@ExpressName, @XmlFile, 1)");
            this.database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
            this.database.AddInParameter(sqlStringCommand, "XmlFile", DbType.String, xmlFile);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteExpressTemplate(int expressId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ExpressTemplates WHERE ExpressId = @ExpressId");
            this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int DeleteExpressTemplates(string expressIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ExpressTemplates WHERE ExpressId in(" + expressIds + ")");
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DataTable GetExpressTemplates(bool? isUser)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates");
            if (isUser.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE IsUse = '{0}'", isUser);
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public IList<FreightTemplate> GetFreightTemplates()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_FreightTemplate_Templates order by TemplateId desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<FreightTemplate>(reader);
            }
        }

        public bool IsExistExpress(string ExpressName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ExpressId) as c FROM Hishop_ExpressTemplates WHERE ExpressName=@ExpressName");
            this.database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, ExpressName);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool SetExpressDefault(int expressId)
        {
            string query = "UPDATE Hishop_ExpressTemplates SET IsUse = 1 WHERE ExpressId = @ExpressId;";
            query = query + "UPDATE Hishop_ExpressTemplates SET IsDefault = 0 WHERE IsDefault = 1 and ExpressId!=@ExpressId;" + "UPDATE Hishop_ExpressTemplates SET IsDefault = ~IsDefault WHERE ExpressId = @ExpressId;";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetExpressIsUse(int expressId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET IsUse = ~IsUse WHERE ExpressId = @ExpressId");
            this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateExpressTemplate(int expressId, string expressName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET ExpressName = @ExpressName WHERE ExpressId = @ExpressId");
            this.database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
            this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

