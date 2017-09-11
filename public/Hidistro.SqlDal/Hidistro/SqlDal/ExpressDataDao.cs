namespace Hidistro.SqlDal
{
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class ExpressDataDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddExpressData(ExpressDataInfo model)
        {
            string query = "";
            if (!string.IsNullOrEmpty(this.GetExpressDataList(model.CompanyCode, model.ExpressNumber)))
            {
                query = "update Hishop_OrderExpressData set DataContent=@DataContent where CompanyCode=@CompanyCode and ExpressNumber=@ExpressNumber";
            }
            else
            {
                query = "insert into Hishop_OrderExpressData(CompanyCode,ExpressNumber,DataContent) values(@CompanyCode,@ExpressNumber,@DataContent)";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "CompanyCode", DbType.String, model.CompanyCode);
            this.database.AddInParameter(sqlStringCommand, "ExpressNumber", DbType.String, model.ExpressNumber);
            this.database.AddInParameter(sqlStringCommand, "DataContent", DbType.String, model.DataContent);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public string GetExpressDataList(string computer, string expressNo)
        {
            string query = "select top 1 DataContent from Hishop_OrderExpressData where CompanyCode=@CompanyCode and ExpressNumber=@ExpressNumber ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "CompanyCode", DbType.String, computer);
            this.database.AddInParameter(sqlStringCommand, "ExpressNumber", DbType.String, expressNo);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return obj2.ToString();
            }
            return "";
        }
    }
}

