﻿namespace Hidistro.Jobs
{
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Quartz;
    using System;
    using System.Data;
    using System.Data.Common;

    public class CartJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            int num = 5;
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE AddTime <= @CurrentTime");
            database.AddInParameter(sqlStringCommand, "CurrentTime", DbType.DateTime, DateTime.Now.AddDays((double) -num));
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

