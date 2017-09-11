namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.StatisticsReport;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ShopStatisticDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AutoStatisticsOrders(out string RetInfo)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Auto");
            this.database.AddInParameter(storedProcCommand, "@RecDate", DbType.Date, DateTime.Today);
            this.database.AddOutParameter(storedProcCommand, "@RetCode", DbType.Int32, 0);
            this.database.AddOutParameter(storedProcCommand, "@RetInfo", DbType.String, 250);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
                return (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
            }
            catch (Exception exception)
            {
                RetInfo = exception.Message;
                return false;
            }
        }

        public bool AutoStatisticsOrdersV2(string AppPath, out string RetInfo)
        {
            RetInfo = "";
            DateTime today = DateTime.Today;
            string commandText = "select MIN( RecDate) as RecDate  from vshop_Statistics_Log where 1=1";
            if (string.IsNullOrEmpty(Convert.ToString(this.database.ExecuteScalar(CommandType.Text, commandText))))
            {
                commandText = "select isnull(MIN( CreateDate), getdate()) as RecDate  from aspnet_Members where 1=1 ";
                today = Convert.ToDateTime(this.database.ExecuteScalar(CommandType.Text, commandText));
            }
            else
            {
                commandText = "select MIN(RecDate) as RecDate  from vshop_Statistics_Log where IsSuccess<>1 ";
                if (!string.IsNullOrEmpty(Convert.ToString(this.database.ExecuteScalar(CommandType.Text, commandText))))
                {
                    today = Convert.ToDateTime(this.database.ExecuteScalar(CommandType.Text, commandText));
                }
                else
                {
                    commandText = "select top 1  Max( RecDate) as RecDate  from vshop_Statistics_Log where 1=1 ";
                    if (!string.IsNullOrEmpty(Convert.ToString(this.database.ExecuteScalar(CommandType.Text, commandText))))
                    {
                        today = Convert.ToDateTime(this.database.ExecuteScalar(CommandType.Text, commandText));
                    }
                }
            }
            for (DateTime time2 = today; Convert.ToInt32(time2.ToString("yyyyMMdd")) < Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd")); time2 = time2.AddDays(1.0))
            {
                if (!this.IsFoundSuccessStatisticRec(time2))
                {
                    this.StatisticsOrdersByRecDate(time2, UpdateAction.AllUpdate, 1, out RetInfo);
                }
                Globals.Debuglog("WebApplication指定日期完毕。RecDate：" + time2.ToString("yyyy-MM-dd") + "  结果：" + RetInfo, "_Tonji.txt");
            }
            return true;
        }

        public DataRow Distributor_GetGlobal(DateTime dDate)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select \r\n\t                (\r\n\t\t                select   sum(ValidOrderTotal) as ValidOrderTotal  \r\n\t\t                from vw_VShop_FinishOrder_Main \r\n\t\t                where \r\n                         (Gateway<>'hishop.plugins.payment.podrequest' and CONVERT( varchar(10), PayDate, 120) =  CONVERT( varchar(10), @RecDate, 120))\r\n                         or(Gateway='hishop.plugins.payment.podrequest'  and CONVERT( varchar(10), OrderDate, 120) =  CONVERT( varchar(10), @RecDate, 120))\r\n\t                ) as ValidOrderTotal,\r\n                * from\r\n                (\r\n\t                select   sum(ValidOrderTotal) as FXValidOrderTotal,   sum( SumCommission) as FXSumCommission  , COUNT(*) as FXOrderNumber\r\n\t                from vw_VShop_FinishOrder_Main \r\n\t                where  ReferralUserId>0 and (\r\n                   (Gateway<>'hishop.plugins.payment.podrequest' and CONVERT( varchar(10), PayDate, 120) =  CONVERT( varchar(10), @RecDate, 120))\r\n                         or(Gateway='hishop.plugins.payment.podrequest'  and CONVERT( varchar(10), OrderDate, 120) =  CONVERT( varchar(10), @RecDate, 120)))\r\n                ) T1\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "RecDate", DbType.String, dDate.ToString("yyyy-MM-dd"));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataRow Distributor_GetGlobalTotal(DateTime dYesterday)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n\t            select \r\n\t            (select COUNT(*) from aspnet_Distributors where ReferralStatus<=1) as DistributorNumber,\r\n\t            (\r\n\t            select  COUNT(*)\r\n\t\t            from aspnet_Distributors\r\n\t\t            where ReferralStatus<=1 and  CONVERT(varchar(10), CreateTime , 120 ) =  CONVERT(varchar(10), @RecDate  , 120 ) \r\n\t            ) as NewAgentNumber,\t\r\n\t            (\r\n\t            SELECT  ISNULL(SUM(Amount),0) from Hishop_BalanceDrawRequest   where isnull(IsCheck,0)=2  \r\n\t            ) as FinishedDrawCommissionFee,\r\n\t\r\n\t            (SELECT SUM(ReferralBlance) FROM aspnet_Distributors ) as WaitDrawCommissionFee\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "RecDate", DbType.String, dYesterday.ToString("yyyy-MM-dd"));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataRow GetOrder_Member_CountInfo(DateTime BeginDate, DateTime EndDate)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select a.*, b.* from \r\n                (\r\n                select 1 as RecNO1,sum(ValidOrderTotal) as SaleAmountFee,count(OrderId) as OrderNumber,count(distinct username) as BuyerNumber, \r\n               sum(SumCommission) as CommissionAmountFee\r\n               from vw_VShop_FinishOrder_Main\r\n               where\r\n               (CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120) and\r\n               CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                and Gateway<>'hishop.plugins.payment.podrequest'\r\n                )\r\n                or(\r\n                CONVERT( varchar(10), OrderDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120) and\r\n                CONVERT( varchar(10), OrderDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                and Gateway='hishop.plugins.payment.podrequest'\r\n                )\r\n\t            )  a    \r\n                left join\r\n                (\r\n                    select  \r\n                    1 as RecNO2, \r\n                     SUM(NewAgentNumber) as NewAgentNumber, SUM(NewMemberNumber) as NewMemberNumber ,\r\n                     sum(isnull(FXOrderNumber,0)) as FXOrderNumber, SUM(FXSaleAmountFee) as FXSaleAmountFee,\r\n                    --AVG(FXResultPercent) as FXResultPercent,\r\n                     SUM ( isnull(CommissionFee,0)) as FXCommissionFee\r\n                    from vshop_Statistics_Globals\r\n                    where 1=1\r\n                        and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                        and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120) \r\n                ) b on a.RecNO1=b.RecNO2\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0];
            }
            return null;
        }

        public DataSet GetOrder_Member_Rebuy(DateTime BeginDate, DateTime EndDate)
        {
            DataSet set = new DataSet();
            DataTable table = null;
            DataTable table2 = null;
            DbCommand sqlStringCommand = null;
            sqlStringCommand = this.database.GetSqlStringCommand("\r\n                            with\r\n               cr as (\r\n                select  distinct Userid from vw_VShop_FinishOrder_Main a\r\n                where \r\n               \r\n                (CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10),  @EndDate, 120)\r\n                and Gateway<>'hishop.plugins.payment.podrequest'\r\n                )\r\n                \r\n                or\r\n                (CONVERT( varchar(10), OrderDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                and CONVERT( varchar(10), OrderDate, 120) <=  CONVERT( varchar(10),  @EndDate, 120)\r\n                and Gateway='hishop.plugins.payment.podrequest'\r\n                )\r\n                ),\r\n                b1 as(\r\n                select Userid,  count(Userid) as c from vw_VShop_FinishOrder_Main a group by Userid\r\n                )\r\n              select( select count(b1.userid) as c from cr,b1 where cr.UserId=b1.UserId and c>1 )as OldBuy,\r\n              (select count(cr.userid) as c from cr) as totalBuy\r\n               ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("\r\n                with cr as(select a.Userid, case Gateway when 'hishop.plugins.payment.podrequest' \r\n                then \r\n                CONVERT( varchar(10), OrderDate, 120)\r\n                else\r\n                 CONVERT( varchar(10), PayDate, 120)\r\n                end\r\n                as gpDate,\r\n                b.c\r\n                 from vw_VShop_FinishOrder_Main a,(select Userid,  count(Userid) as c from vw_VShop_FinishOrder_Main a group by Userid) b\r\n                where a.UserId=b.UserId\r\n                and(\r\n                (CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                and Gateway<>'hishop.plugins.payment.podrequest'\r\n                )\r\n                \r\n                or\r\n                (CONVERT( varchar(10), OrderDate, 120) >=  CONVERT( varchar(10),@BeginDate, 120)\r\n                and CONVERT( varchar(10), OrderDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                and Gateway='hishop.plugins.payment.podrequest'\r\n                )\r\n                )\r\n                )\r\n                select COUNT(distinct UserId) as TotalBuy, \r\n                COUNT(distinct case when c>1 then UserId else null end) as OldBuy,\r\n                gpDate from cr group by gpDate\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
            {
                table2 = DataHelper.ConverDataReaderToDataTable(reader2);
            }
            set.Tables.Add(table);
            set.Tables.Add(table2);
            return set;
        }

        public DataTable GetOrderCountInfo(DateTime BeginDate, DateTime EndDate)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n            select * from \r\n                (\r\n                   select 1 as RecNO,\r\n                                count(*) as OrderNumber,  sum(   ValidOrderTotal) as SaleAmountFee\r\n                                from vw_VShop_FinishOrder_Main\r\n                                where 1=1\r\n                                 and CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                                 and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)                   \r\n                ) T1\r\n                left join\r\n                (\r\n                   select 1 as FXRecNO,\r\n                                count(*) as FXOrderNumber,  sum(   ValidOrderTotal) as FXSaleAmountFee\r\n                                from vw_VShop_FinishOrder_Main\r\n                                where  ReferralUserId>0\r\n                                 and CONVERT( varchar(10), PayDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                                 and CONVERT( varchar(10), PayDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)                   \r\n                ) T2 on T1.RecNO= T2.FXRecNO\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetOrderStatisticReport(OrderStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string table = string.Format("\r\n                    (\r\n                        select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n                        from \r\n                        (\r\n\t                        select AgentId,\r\n\t                        sum(OrderNumber) as OrderNumber, sum(SaleAmountFee) as SaleAmountFee, sum(BuyerNumber) as BuyerNumber, \r\n\t                        AVG(BuyerAvgPrice) as BuyerAvgPrice , sum(CommissionAmountFee)  as CommissionAmountFee\r\n\t                        from dbo.vshop_Statistics_Distributors a\r\n\t                         where 1=1\r\n\t                         and AgentID>0\r\n\t                         and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t                         and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120) \r\n\t                         group by \t AgentID  \r\n                        ) T1\r\n                        left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId \r\n                       -- where b.ReferralStatus<=1\r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "AgentId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult GetOrderStatisticReport_UnderShop(OrderStatisticsQuery_UnderShop query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string table = "";
            if (query.ShopLevel == 1)
            {
                table = string.Format("\r\n                    (\r\n                        select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n                        from \r\n                        (\r\n                            select  AgentID, SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                select UserId \r\n                                from aspnet_Distributors\r\n                                where ReferralUserId= {2} and UserId <> ReferralUserId\r\n                                 ) \r\n                            group by AgentID\r\n                         ) T1\r\n                        left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId \r\n                    ) P  ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"), query.AgentId);
            }
            else if (query.ShopLevel == 2)
            {
                table = string.Format("\r\n                    (\r\n                        select T1.*, b.RealName, b.CellPhone, b.UserName,  b.UserHead , b.StoreName\r\n                        from \r\n                        (\r\n                            select  AgentID, SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), '{0}', 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), '{1}', 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId in\r\n                                    (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId= {2} and UserId <> ReferralUserId\r\n                                    )\r\n                                    and UserId <> ReferralUserId\r\n                                 ) \r\n                            group by AgentID\r\n                         ) T1\r\n                        left join vw_Hishop_DistributorsMembers b on T1.AgentId= b.UserId \r\n                    ) P  ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"), query.AgentId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "AgentId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        private DataRow GetOrderStatisticReportGlobal_UnderShop_BAD(OrderStatisticsQuery_UnderShop query)
        {
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = null;
            if (query.ShopLevel == 1)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("\r\n                            select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                select UserId \r\n                                from aspnet_Distributors\r\n                                where ReferralUserId= @AgentId   and UserId <> ReferralUserId \r\n                                 ) \r\n\r\n                     ");
            }
            else if (query.ShopLevel == 2)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("\r\n                            select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                            from dbo.vshop_Statistics_Distributors\r\n                            where AgentID >0\r\n\t                             and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10),  @BeginDate, 120)\r\n\t                             and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10),  @EndDate, 120)\r\n                                 and AgentId in\r\n                                 (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId in\r\n                                    (\r\n                                    select UserId \r\n                                    from aspnet_Distributors\r\n                                    where ReferralUserId=  @AgentId and UserId <> ReferralUserId \r\n                                    )\r\n                                    and UserId <> ReferralUserId\r\n                                 ) \r\n                    ");
            }
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, query.BeginDate.Value);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, query.EndDate.Value);
            this.database.AddInParameter(sqlStringCommand, "AgentId", DbType.Int32, query.AgentId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0];
            }
            return null;
        }

        public DataRow GetOrderStatisticReportGlobalByAgentID(OrderStatisticsQuery_UnderShop query)
        {
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = null;
            sqlStringCommand = this.database.GetSqlStringCommand("\r\n                        select  SUM(OrderNumber) OrderNumber, SUM(SaleAmountFee) SaleAmountFee, SUM(BuyerNumber) BuyerNumber, AVG(BuyerAvgPrice) BuyerAvgPrice, SUM(CommissionAmountFee) CommissionAmountFee\r\n                        from dbo.vshop_Statistics_Distributors\r\n                        where AgentID >0\r\n\t                            and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n\t                            and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)\r\n                                and AgentId = @AgentId \r\n                                \r\n\r\n                    ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, query.BeginDate.Value);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, query.EndDate.Value);
            this.database.AddInParameter(sqlStringCommand, "AgentId", DbType.Int32, query.AgentId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0];
            }
            return null;
        }

        public DataTable GetSaleReport(DateTime BeginDate, DateTime EndDate)
        {
            DataSet set = new DataSet();
            DbCommand sqlStringCommand = null;
            sqlStringCommand = this.database.GetSqlStringCommand("\r\n                 select  * from vshop_Statistics_Globals\r\n                    where 1=1\r\n                     and CONVERT( varchar(10), RecDate, 120) >=  CONVERT( varchar(10), @BeginDate, 120)\r\n                     and CONVERT( varchar(10), RecDate, 120) <=  CONVERT( varchar(10), @EndDate, 120)       \r\n\r\n                ");
            this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.Date, BeginDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.Date, EndDate);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            set.Tables.Add(table);
            return set.Tables[0];
        }

        public DataTable GetTrendDataList_FX(DateTime BeginDate, int Days)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID", typeof(int)));
            table.Columns.Add(new DataColumn("RecDate", typeof(DateTime)));
            table.Columns.Add(new DataColumn("NewAgentCount", typeof(decimal)));
            table.Columns.Add(new DataColumn("FXAmountFee", typeof(decimal)));
            table.Columns.Add(new DataColumn("FXCommisionFee", typeof(decimal)));
            for (int i = 0; i < Days; i++)
            {
                DataRow row = table.NewRow();
                DateTime time = BeginDate.AddDays((double) i);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                    select\r\n                    (select  COUNT(*) from aspnet_Distributors where ReferralStatus<=1 and  CONVERT(varchar(10),  CreateTime , 120)= CONVERT(varchar(10),  @RecDate , 120 ) ) as NewAgentCount ,\r\n                    isnull(sum(ValidOrderTotal),0) as ValidOrderTotal, isnull(sum(SumCommission),0) as SumCommission \r\n                        from vw_VShop_FinishOrder_Main where  ReferralUserId>0 and\r\n                        ((Gateway <> 'hishop.plugins.payment.podrequest' and CONVERT(varchar(10),  PayDate, 120 )= CONVERT(varchar(10),  @RecDate ,120))\r\n                          or(Gateway = 'hishop.plugins.payment.podrequest' and CONVERT(varchar(10),  OrderDate, 120 )= CONVERT(varchar(10),  @RecDate ,120))\r\n                        )\r\n               \r\n                    ");
                this.database.AddInParameter(sqlStringCommand, "RecDate", DbType.Date, time);
                DataTable table2 = new DataTable();
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (table2.Rows.Count > 0)
                {
                    row["NewAgentCount"] = table2.Rows[0]["NewAgentCount"];
                    row["FXAmountFee"] = table2.Rows[0]["ValidOrderTotal"];
                    row["FXCommisionFee"] = table2.Rows[0]["SumCommission"];
                }
                else
                {
                    row["NewAgentCount"] = 0;
                    row["FXAmountFee"] = 0;
                    row["FXCommisionFee"] = 0;
                }
                row["RecDate"] = time;
                table.Rows.Add(row);
            }
            return table;
        }

        private bool IsFoundSuccessStatisticRec(DateTime dDate)
        {
            string commandText = "select top 1 RecDate from vshop_Statistics_Log  WITH (NOLOCK)  where IsSuccess=1 and RecDate='" + dDate.ToString("yyyy-MM-dd") + "' ";
            return !string.IsNullOrEmpty(Convert.ToString(this.database.ExecuteScalar(CommandType.Text, commandText)));
        }

        public DataTable Member_GetInCreateReport(OrderStatisticsQuery query)
        {
            new StringBuilder().Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string str = string.Format("\r\n                    select * from   vshop_Statistics_Globals\r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120)   \r\n                    order by RecDate \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult Member_GetRegionReport(OrderStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            string table = string.Format("\r\n                    (\r\n                         select  ROW_NUMBER() over(order by TotalRec desc) as RowIndex  ,  * \r\n                         from \r\n                         (\r\n\t                        select isnull(TotalRec,0) as TotalRec, RegionID_Group as RegionID, RegionName\r\n\t                        from VShop_Region X1\r\n\t                        left join \r\n\t                        (\r\n\t\t                        select sum( TotalRec) as TotalRec, RegionID_Group \r\n\t\t                        from\r\n\t\t                        (\r\n\t\t\t                        select a.*, isnull(b.RegionID,0) as RegionID_Group , b.RegionName\r\n\t\t\t                        from \r\n\t\t\t                        (\r\n\t\t\t                        select COUNT(*) TotalRec,  TopRegionId\r\n\t\t\t\t                        from aspnet_Members\r\n\t\t\t\t                        where Status=1\r\n\t\t\t\t                        group by TopRegionId\r\n\t\t\t                        ) a\r\n\t\t\t                        left join VShop_Region b on a.TopRegionId= b.RegionId\r\n\t\t                        )  T1\r\n\t\t                        group by T1.RegionID_Group\r\n\t                        ) X2 on X1.RegionId= X2.RegionID_Group\r\n                         ) Y1\r\n                    ) P  \r\n                    ", query.BeginDate, query.EndDate);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "RowIndex", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult Member_GetStatisticReport(OrderStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string table = string.Format("\r\n                    (\r\n\t                select T1.*, b.RealName, b.CellPhone, b.UserName, b.CreateDate, b.UserHead\r\n\t                from \r\n\t                (\r\n\t                select  UserId ,COUNT(*) as OrderNumber, SUM(ValidOrderTotal) as OrderTotal, \r\n\t                case \r\n\t\t                when  SUM(ValidOrderTotal)>0 then SUM(ValidOrderTotal) * 1.0 / COUNT(*) \r\n\t\t                else 0\r\n\t                end as AvgPrice\r\n\t                from  dbo.vw_VShop_FinishOrder_Main a\r\n                    where CONVERT(varchar(10), PayDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), PayDate, 120) <=CONVERT(varchar(10), '{1}', 120)  \r\n \t                group by UserId\r\n\t                ) T1\r\n\t                left join aspnet_Members b on T1.UserID= b.UserId \r\n                    where b.Status=1\r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "UserId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable Member_GetStatisticReport_NoPage(OrderStatisticsQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.Substring(0, str.Length - 1);
            new StringBuilder().Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string str3 = string.Format("\r\n                    (\r\n\t                select   ROW_NUMBER() over(order by OrderTotal desc) as RankIndex  , \r\n                        T1.*, b.RealName, b.CellPhone, b.UserName, b.CreateDate, b.UserHead\r\n\t                from \r\n\t                (\r\n\t                select  UserId ,COUNT(*) as OrderNumber, SUM(ValidOrderTotal) as OrderTotal, \r\n\t                case \r\n\t\t                when  SUM(ValidOrderTotal)>0 then SUM(ValidOrderTotal) * 1.0 / COUNT(*) \r\n\t\t                else 0\r\n\t                end as AvgPrice\r\n\t                from  dbo.vw_VShop_FinishOrder_Main a\r\n                    where CONVERT(varchar(10), PayDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), PayDate, 120) <=CONVERT(varchar(10), '{1}', 120)  \r\n \t                group by UserId\r\n\t                ) T1\r\n\t                left join aspnet_Members b on T1.UserID= b.UserId \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select " + str + " from " + str3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataRow MemberGlobal_GetCountInfo()
        {
            DataTable table = new DataTable();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_Statistics_Member");
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataTable MemberGlobal_GetStatisticList(int FuncID)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = null;
            if (FuncID == 1)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("\r\n                    select isnull(T1.Total ,0) as Total, isnull(T2.Name,'其它') as Name\r\n                    from\r\n                    (\r\n                    select  COUNT(*) as Total, a.GradeId\r\n\t                    from aspnet_Members a\r\n\t                    where  1=1 and a.Status=1\r\n\t                    group by a.GradeId\r\n                    ) T1\r\n                    left join aspnet_MemberGrades T2 on T1.GradeId= T2.GradeId\r\n                    ");
            }
            else if (FuncID == 2)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select v.*, ISNULL( T1.Total,0) as Total\r\n                from VShop_Region v\r\n                left join\r\n                (\r\n                select  COUNT(*) as Total, a.TopRegionId\r\n\t                from aspnet_Members a\r\n\t                where  1=1 and a.Status=1\r\n\t                group by a.TopRegionId\r\n                ) T1 on v.RegionID = T1.TopRegionId\r\n                ");
            }
            else
            {
                return null;
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult Product_GetStatisticReport(OrderStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string table = string.Format("\r\n                    (\r\n                    select  ROW_NUMBER() over(order by a.SaleAmountFee desc) as RankIndex  , \r\n                    a.* , b.ProductName,b.ThumbnailUrl60\r\n                    from (select productid,sum(SaleQty) as SaleQty,sum(SaleAmountFee) as SaleAmountFee,sum(buyernumber) as buyernumber,sum(TotalVisits) as TotalVisits,sum(ConversionRate)/count(ConversionRate) as ConversionRate,getdate() as RecDate from vshop_Statistics_Products \r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120) group by ProductID)  a \r\n                    left join Hishop_Products b on a.ProductID  = b.ProductId\r\n                      \r\n                    ) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "ProductId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable Product_GetStatisticReport_NoPage(OrderStatisticsQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.Substring(0, str.Length - 1);
            new StringBuilder().Append(" 1=1 ");
            if (!query.BeginDate.HasValue)
            {
                query.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
            }
            if (!query.EndDate.HasValue)
            {
                query.EndDate = new DateTime?(DateTime.Today);
            }
            string str3 = string.Format("\r\n                    (\r\n                    select  ROW_NUMBER() over(order by a.SaleAmountFee desc) as RankIndex  , \r\n                    a.* , b.ProductName,b.ThumbnailUrl60\r\n                    from (select productid,sum(SaleQty) as SaleQty,sum(SaleAmountFee) as SaleAmountFee,sum(buyernumber) as buyernumber,sum(TotalVisits) as TotalVisits,sum(ConversionRate)/count(ConversionRate) as ConversionRate,getdate() as RecDate from vshop_Statistics_Products \r\n                    where CONVERT(varchar(10), RecDate, 120) >= CONVERT(varchar(10), '{0}', 120)  \r\n                          and  CONVERT(varchar(10), RecDate, 120) <=CONVERT(varchar(10), '{1}', 120) group by ProductID)  a \r\n                    left join Hishop_Products b on a.ProductID  = b.ProductId) P  \r\n                    ", query.BeginDate.Value.ToString("yyyy-MM-dd"), query.EndDate.Value.ToString("yyyy-MM-dd"));
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select " + str + " from " + str3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataRow ShopGlobal_GetMemberCount()
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n              select  \r\n                ( select count(*)   from Hishop_Orders where OrderStatus=2   or ( OrderStatus=1 and  Gateway='hishop.plugins.payment.podrequest' ) ) as 'WaitSendOrderQty',\r\n                ( select count(*)  from Hishop_Products  where SaleStatus=1) as GoodsQty ,\r\n                ( select    COUNT(*)  from aspnet_Members) as  MemberQty,\r\n                ( select  COUNT(*) from aspnet_Distributors where  ReferralStatus<=1 ) as DistributorQty,\r\n                (\r\n                    select COUNT(*) from\r\n                    (\r\n                    select COUNT(*) SumRec from  Hishop_OrderItems where OrderItemsStatus>=6 and  OrderItemsStatus<=8\r\n                    group by OrderId\r\n                    ) T1\r\n                ) as ServiceOrderQty\r\n               \r\n                ");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataRow ShopGlobal_GetOrderCountByDate(DateTime dDate)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                select \r\n                (select count(*) as OrderQty from  Hishop_Orders \r\n                where OrderStatus <> 4 and\r\n                (\r\n                 (CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate1 and OrderStatus <> 1 and Gateway <> 'hishop.plugins.payment.podrequest')\r\n                  or\r\n                 (CONVERT(varchar(10),  OrderDate  , 120 ) = @RecDate1 and Gateway = 'hishop.plugins.payment.podrequest' )\r\n                )\r\n                 ) as OrderQty,\r\n                (\r\n                select \r\n                 SUM( (a.ItemAdjustedPrice) * a.Quantity - a.ReturnMoney- a.DiscountAverage-a.ItemAdjustedCommssion ) as  OrderAmountFee\r\n                 --a.*, b.OrderStatus\r\n                from Hishop_OrderItems a \r\n                INNER join Hishop_Orders b on a.OrderId=b.OrderId\r\n                where \r\n                 OrderItemsStatus<>4\r\n                    and OrderStatus<>4 and ((\r\n                     CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate2 and Gateway <>'hishop.plugins.payment.podrequest' and OrderStatus<>1) \r\n                     or (CONVERT(varchar(10),  OrderDate  , 120 ) = @RecDate1 and Gateway ='hishop.plugins.payment.podrequest'))\r\n                )+\r\n                (select SUM(c.AdjustedFreight) as  OrderAmountFee\r\n                from Hishop_Orders c \r\n                where OrderStatus <> 4 and (CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate1 and OrderStatus<>1 and Gateway <>'hishop.plugins.payment.podrequest')\r\n                or (CONVERT(varchar(10),  OrderDate  , 120 ) = @RecDate1 and Gateway ='hishop.plugins.payment.podrequest'  )\r\n               ) as OrderAmountFee");
            this.database.AddInParameter(sqlStringCommand, "RecDate1", DbType.String, dDate.ToString("yyyy-MM-dd"));
            this.database.AddInParameter(sqlStringCommand, "RecDate2", DbType.String, dDate.ToString("yyyy-MM-dd"));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return table.Rows[0];
        }

        public DataTable ShopGlobal_GetSortList_Distributor(DateTime BeginDate, int TopCount)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(" select ROW_NUMBER() OVER(ORDER BY CommTotalSum DESC) AS rownum, a.*,d.StoreName  from");
            builder.AppendLine(" (select top " + TopCount + " UserId,COUNT(DISTINCT case  userid when ReferralUserId then OrderId else null end) as Ordernums,");
            builder.AppendLine(" SUM(case  userid when ReferralUserId then OrderTotal else 0 end) as OrderTotalSum,SUM(CommTotal) as CommTotalSum");
            builder.AppendLine(" from vw_Hishop_CommissionWithBuyUserId  where 1=1 ");
            builder.AppendFormat(" and TradeTime>='{0}' ", BeginDate.ToString("yyyy-MM-dd") + " 00:00:00");
            builder.AppendLine(" group by UserId  order by CommTotalSum desc)  a");
            builder.AppendLine(" INNER JOIN aspnet_Members m ON a.UserId = m.UserId ");
            builder.AppendLine(" LEFT JOIN aspnet_Distributors d on a.UserId=d.UserId ");
            new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable ShopGlobal_GetSortList_Member(DateTime BeginDate, int TopCount)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n              select   top(@TopCount ) \r\n                T1.*,   T1.UserId as UserID, b.UserName, RANK() OVER  ( ORDER BY ValidOrderTotal desc) AS Rank,\r\n                T2.OrderQty\r\n                from \r\n                (                                     \r\n\t                select \r\n\t                  b.UserId,\r\n\t                  SUM( a.ItemAdjustedPrice * a.Quantity - a.ReturnMoney- a.DiscountAverage ) as  ValidOrderTotal \r\n\t                from Hishop_OrderItems a \r\n\t                left join Hishop_Orders b on a.OrderId=b.OrderId\r\n\t                where \r\n\t                1=1 \r\n\t                and UserId>0\r\n\t                and ( OrderItemsStatus<>1 and  OrderItemsStatus<>4 and OrderItemsStatus<>9 and OrderItemsStatus<>10 )\r\n\t                and ( OrderStatus<>1 and OrderStatus<>4 and OrderStatus<>9 and OrderStatus<>10 )\r\n\t                group by b.UserId\r\n                ) T1\r\n                left join \r\n                (\r\n\t                select UserId as UserId2, COUNT(*) as OrderQty from Hishop_Orders\r\n\t                where 1=1 \r\n\t\t                and UserId>0\r\n\t\t                and ( OrderStatus<>1 and OrderStatus<>4 and OrderStatus<>9 and OrderStatus<>10 )\r\n\t                group by  UserId\r\n                ) T2\t on T1.UserId= T2.UserId2\r\n                left join  aspnet_Members  b on T1.UserId= b.UserId\r\n                where b.Status<=1\r\n                order by T1.ValidOrderTotal desc ");
            this.database.AddInParameter(sqlStringCommand, "TopCount", DbType.Int32, TopCount);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable ShopGlobal_GetTrendDataList(DateTime BeginDate, int Days)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID", typeof(int)));
            table.Columns.Add(new DataColumn("RecDate", typeof(DateTime)));
            table.Columns.Add(new DataColumn("OrderCount", typeof(int)));
            table.Columns.Add(new DataColumn("NewMemberCount", typeof(int)));
            table.Columns.Add(new DataColumn("NewDistributorCount", typeof(int)));
            for (int i = 0; i < Days; i++)
            {
                DataRow row = table.NewRow();
                DateTime time = BeginDate.AddDays((double) i);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("\r\n                    select\r\n                    (select count(*) from  Hishop_Orders where\r\n                    OrderStatus <> 4 and\r\n                    ((CONVERT(varchar(10),  PayDate  , 120 ) = @RecDate0 and OrderStatus<>1 and Gateway <>'hishop.plugins.payment.podrequest' )\r\n                    or \r\n                      (CONVERT(varchar(10),  OrderDate  , 120 ) = @RecDate0 and Gateway ='hishop.plugins.payment.podrequest')\r\n                    ))  as OrderCount,\r\n                    ( select    COUNT(*)  from aspnet_Members where Status=1 and CONVERT(varchar(10),  CreateDate, 120 )= CONVERT(varchar(10),  @RecDate1, 120 )  ) as MemberQty,\r\n                    ( select  COUNT(*) from aspnet_Distributors where ReferralStatus<=1 and  CONVERT(varchar(10),  CreateTime, 120 )= CONVERT(varchar(10),  @RecDate2, 120 ) ) as DistributorQty \r\n                    ");
                this.database.AddInParameter(sqlStringCommand, "RecDate0", DbType.Date, time);
                this.database.AddInParameter(sqlStringCommand, "RecDate1", DbType.Date, time);
                this.database.AddInParameter(sqlStringCommand, "RecDate2", DbType.Date, time);
                DataTable table2 = new DataTable();
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (table2.Rows.Count > 0)
                {
                    row["OrderCount"] = table2.Rows[0]["OrderCount"];
                    row["NewMemberCount"] = table2.Rows[0]["MemberQty"];
                    row["NewDistributorCount"] = table2.Rows[0]["DistributorQty"];
                }
                else
                {
                    row["OrderCount"] = 0;
                    row["NewMemberCount"] = 0;
                    row["NewDistributorCount"] = 0;
                }
                row["RecDate"] = time;
                table.Rows.Add(row);
            }
            return table;
        }

        public bool StatisticsOrdersByNotify(DateTime RecDate, UpdateAction FuncAction, string ActionDesc, out string RetInfo)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Notify");
            this.database.AddInParameter(storedProcCommand, "@CalDate", DbType.Date, RecDate);
            this.database.AddInParameter(storedProcCommand, "@FuncAction", DbType.Int32, FuncAction);
            this.database.AddInParameter(storedProcCommand, "@ActionDesc", DbType.String, ActionDesc);
            this.database.AddOutParameter(storedProcCommand, "@RetCode", DbType.Int32, 0);
            this.database.AddOutParameter(storedProcCommand, "@RetInfo", DbType.String, 250);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
                return (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
            }
            catch (Exception exception)
            {
                RetInfo = exception.Message;
                return false;
            }
        }

        public bool StatisticsOrdersByRecDate(DateTime RecDate, UpdateAction FuncAction, int IsUpdateLog, out string RetInfo)
        {
            try
            {
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_vshop_Statistics_Daily");
                this.database.AddInParameter(storedProcCommand, "@RecDate", DbType.Date, RecDate);
                this.database.AddInParameter(storedProcCommand, "@FuncAction", DbType.Int32, FuncAction);
                this.database.AddInParameter(storedProcCommand, "@IsUpdateLog", DbType.Int32, IsUpdateLog);
                this.database.AddOutParameter(storedProcCommand, "@RetCode", DbType.Int32, 0);
                this.database.AddOutParameter(storedProcCommand, "@RetInfo", DbType.String, 250);
                this.database.ExecuteNonQuery(storedProcCommand);
                RetInfo = storedProcCommand.Parameters["@RetInfo"].Value.ToString();
                return (storedProcCommand.Parameters["@RetCode"].Value.ToString() == "1");
            }
            catch (Exception exception)
            {
                RetInfo = exception.Message;
                return false;
            }
        }
    }
}

