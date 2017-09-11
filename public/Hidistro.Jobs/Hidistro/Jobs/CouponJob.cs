namespace Hidistro.Jobs
{
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Quartz;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class CouponJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Coupon_Coupons SET Finished = 1 WHERE Finished = 0 AND EndDate <= @CurrentTime;");
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            database.AddInParameter(sqlStringCommand, "CurrentTime", DbType.DateTime, DateTime.Now);
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

