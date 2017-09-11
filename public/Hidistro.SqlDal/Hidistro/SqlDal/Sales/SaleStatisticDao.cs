namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class SaleStatisticDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        private static string BuildMemberStatisticsQuery(SaleStatisticsQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT UserId, UserName ");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
                builder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = aspnet_Members.UserId) as SaleTotals");
                builder.AppendFormat(",(select Count(OrderId) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = aspnet_Members.UserId) as OrderCount ");
            }
            else
            {
                builder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
            }
            builder.Append(" from aspnet_Members where Expenditure > 0");
            if (!query.StartDate.HasValue)
            {
                bool hasValue = query.EndDate.HasValue;
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildOrdersQuery(OrderQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE 1 = 1 ", new object[0]);
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            else
            {
                if (query.PaymentType.HasValue)
                {
                    builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
                }
                if (query.GroupBuyId.HasValue)
                {
                    builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
                }
                if (!string.IsNullOrEmpty(query.ProductName))
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
                }
                if (query.RegionId.HasValue)
                {
                    builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
                }
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
                }
                if (query.Status == OrderStatus.History)
                {
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
                }
                else if (query.Status == OrderStatus.BuyerAlreadyPaid)
                {
                    builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int) query.Status);
                }
                else if (query.Status != OrderStatus.All)
                {
                    builder.AppendFormat(" AND OrderStatus = {0}", (int) query.Status);
                }
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                if (query.ShippingModeId.HasValue)
                {
                    builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
                }
                if (query.IsPrinted.HasValue)
                {
                    builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
                }
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildProductSaleQuery(SaleStatisticsQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
            builder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
            builder.AppendFormat(" FROM Hishop_OrderItems o  WHERE 0=0 ", new object[0]);
            builder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2})", 1, 4, 9);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            builder.Append(" GROUP BY ProductId HAVING ProductId IN");
            builder.Append(" (SELECT ProductId FROM Hishop_Products)");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildProductVisitAndBuyStatisticsQuery(SaleStatisticsQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId,(SaleCounts*100/(case when VistiCounts=0 then 1 else VistiCounts end)) as BuyPercentage");
            builder.Append(" FROM Hishop_products where SaleCounts>0");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildRegionsUserQuery(Pagination page)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT r.RegionId, r.RegionName, SUM(au.UserCount) AS Usercounts,");
            builder.Append(" (SELECT (SELECT SUM(COUNT) FROM aspnet_Members)) AS AllUserCounts ");
            builder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r ");
            builder.Append(" WHERE (r.AreaId IS NOT NULL) AND ((au.path LIKE r.path + LTRIM(RTRIM(STR(r.RegionId))) + ',%') OR au.RegionId = r.RegionId)");
            builder.Append(" group by r.RegionId, r.RegionName ");
            builder.Append(" UNION SELECT 0, '0', sum(au.Usercount) AS Usercounts,");
            builder.Append(" (SELECT (SELECT count(*) FROM aspnet_Members)) AS AllUserCounts ");
            builder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r  ");
            builder.Append(" WHERE au.regionid IS NULL OR au.regionid = 0 group by r.RegionId, r.RegionName");
            if (!string.IsNullOrEmpty(page.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(page.SortBy), page.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildUserOrderQuery(OrderQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
                return builder.ToString();
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND  OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND  OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildMemberStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(BuildMemberStatisticsQuery(query));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSales_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSalesNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductVisitAndBuyStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductVisitAndBuyStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductName,VistiCounts,SaleCounts as BuyCount ,(SaleCounts/(case when VistiCounts=0 then 1 else VistiCounts end))*100 as BuyPercentage ");
            builder.Append("FROM Hishop_Products WHERE SaleCounts>0 ORDER BY BuyPercentage DESC;");
            builder.Append("SELECT COUNT(*) as TotalProductSales FROM Hishop_Products WHERE SaleCounts>0;");
            sqlStringCommand.CommandText = builder.ToString();
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    table = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult() && reader.Read())
                {
                    totalProductSales = (int) reader["TotalProductSales"];
                    return table;
                }
                totalProductSales = 0;
            }
            return table;
        }

        public DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat("orderDate >= '{0}'", query.StartDate.Value);
            }
            if (query.EndDate.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("orderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            if (builder.Length > 0)
            {
                builder.Append(" AND ");
            }
            builder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_SaleDetails", "OrderId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_SaleDetails WHERE 1=1");
            if (query.StartDate.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND OrderDate >= '{0}'", query.StartDate);
            }
            if (query.EndDate.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND OrderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format("AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public DbQueryResult GetSaleTargets()
        {
            DbQueryResult result = new DbQueryResult();
            string query = string.Empty;
            query = string.Format("select (select Count(OrderId) from Hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1}  AND OrderStatus != {2}) as OrderNumb,", 1, 4, 9) + string.Format("(select isnull(sum(OrderTotal),0) - isnull(sum(RefundAmount),0) from hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}) as OrderPrice, ", 1, 4, 9) + " (select COUNT(*) from aspnet_Members) as UserNumb,  (select count(*) from aspnet_Members where UserID in (select userid from Hishop_orders)) as UserOrderedNumb,  ISNULL((select sum(VistiCounts) from Hishop_products),0) as ProductVisitNumb ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public OrderStatisticsInfo GetUserOrders(OrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, userOrder.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, userOrder.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, userOrder.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfPage += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfPage += (decimal) reader["Profits"];
                    }
                }
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["Profits"];
                    }
                }
            }
            info.TotalCount = (int) this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return info;
        }

        public OrderStatisticsInfo GetUserOrdersNoPage(OrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatisticsNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["Profits"];
                    }
                }
            }
            info.TotalCount = (int) this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return info;
        }

        public IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalRegionsUsers)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(select count(*) from aspnet_Members) as AllUserCounts FROM aspnet_Members  GROUP BY TopRegionId ");
            IList<UserStatisticsInfo> list = new List<UserStatisticsInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                UserStatisticsInfo item = null;
                while (reader.Read())
                {
                    item = DataMapper.PopulateUserStatistics(reader);
                    list.Add(item);
                }
                if (item != null)
                {
                    totalRegionsUsers = int.Parse(item.AllUserCounts.ToString());
                    return list;
                }
                totalRegionsUsers = 0;
            }
            return list;
        }
    }
}

