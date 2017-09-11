namespace Hidistro.SqlDal.Members
{
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class PointDetailDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public decimal GetIntegral(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(IntegralChange) FROM vshop_IntegralDetail WHERE Userid = @Userid and IntegralChange>0");
            this.database.AddInParameter(sqlStringCommand, "Userid", DbType.Int32, userId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !string.IsNullOrEmpty(obj2.ToString()))
            {
                return (decimal) obj2;
            }
            return 0M;
        }
    }
}

