namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class DateStatisticDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        private string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
        {
            StringBuilder builder = new StringBuilder();
            switch (saleStatisticsType)
            {
                case SaleStatisticsType.SaleCounts:
                    builder.Append("SELECT COUNT(OrderId) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                    break;

                case SaleStatisticsType.SaleTotal:
                    builder.Append("SELECT Isnull(SUM(OrderTotal),0)");
                    builder.Append(" FROM Hishop_orders WHERE  (OrderDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                    break;

                case SaleStatisticsType.Profits:
                    builder.Append("SELECT IsNull(SUM(OrderProfit),0) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                    break;
            }
            return builder.ToString();
        }

        private DataTable CreateTable()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("Date", typeof(int));
            DataColumn column2 = new DataColumn("SaleTotal", typeof(decimal));
            DataColumn column3 = new DataColumn("Percentage", typeof(decimal));
            DataColumn column4 = new DataColumn("Lenth", typeof(decimal));
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column3);
            table.Columns.Add(column4);
            return table;
        }

        public decimal GetAddUserTotal(int year)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate)  AS UserAdd");
            DateTime time = new DateTime(year, 1, 1);
            DateTime time2 = time.AddYears(1);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToDecimal(obj2);
            }
            return 0M;
        }

        private int GetDayCount(int year, int month)
        {
            if (month == 2)
            {
                if ((((year % 4) != 0) || ((year % 100) == 0)) && ((year % 400) != 0))
                {
                    return 0x1c;
                }
                return 0x1d;
            }
            if ((((month == 1) || (month == 3)) || ((month == 5) || (month == 7))) || (((month == 8) || (month == 10)) || (month == 12)))
            {
                return 0x1f;
            }
            return 30;
        }

        public DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DataTable table = this.CreateTable();
            decimal allSalesTotal = this.GetMonthSaleTotal(year, month, saleStatisticsType);
            int dayCount = this.GetDayCount(year, month);
            int num3 = ((year == DateTime.Now.Year) && (month == DateTime.Now.Month)) ? DateTime.Now.Day : dayCount;
            for (int i = 1; i <= num3; i++)
            {
                DateTime time = new DateTime(year, month, i);
                DateTime time2 = time.AddDays(1.0);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", time);
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", time2);
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                decimal salesTotal = (obj2 == null) ? 0M : Convert.ToDecimal(obj2);
                this.InsertToTable(table, i, salesTotal, allSalesTotal);
            }
            return table;
        }

        public decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, month, day);
            DateTime time2 = time.AddDays(1.0);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        public DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DataTable table = this.CreateTable();
            int num = (year == DateTime.Now.Year) ? DateTime.Now.Month : 12;
            for (int i = 1; i <= num; i++)
            {
                DateTime time = new DateTime(year, i, 1);
                DateTime time2 = time.AddMonths(1);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", time);
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", time2);
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                decimal salesTotal = (obj2 == null) ? 0M : Convert.ToDecimal(obj2);
                decimal yearSaleTotal = this.GetYearSaleTotal(year, saleStatisticsType);
                this.InsertToTable(table, i, salesTotal, yearSaleTotal);
            }
            return table;
        }

        public decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, month, 1);
            DateTime time2 = time.AddMonths(1);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        public IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
        {
            int num6;
            IList<UserStatisticsForDate> list = new List<UserStatisticsForDate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate) AS UserAdd ");
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DateTime time = new DateTime();
            DateTime time2 = new DateTime();
            if (days.HasValue)
            {
                time = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double) -days.Value);
            }
            else if (year.HasValue && month.HasValue)
            {
                time = new DateTime(year.Value, month.Value, 1);
            }
            else if (year.HasValue && !month.HasValue)
            {
                time = new DateTime(year.Value, 1, 1);
            }
            if (!days.HasValue)
            {
                if (year.HasValue && month.HasValue)
                {
                    int num2 = DateTime.DaysInMonth(year.Value, month.Value);
                    for (int i = 1; i <= num2; i++)
                    {
                        UserStatisticsForDate item = new UserStatisticsForDate();
                        if (i > 1)
                        {
                            time = time2;
                        }
                        time2 = time.AddDays(1.0);
                        this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                        this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                        item.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                        item.TimePoint = i;
                        list.Add(item);
                    }
                    return list;
                }
                if (year.HasValue && !month.HasValue)
                {
                    int num4 = 12;
                    for (int j = 1; j <= num4; j++)
                    {
                        UserStatisticsForDate date3 = new UserStatisticsForDate();
                        if (j > 1)
                        {
                            time = time2;
                        }
                        time2 = time.AddMonths(1);
                        this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                        this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                        date3.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                        date3.TimePoint = j;
                        list.Add(date3);
                    }
                }
                return list;
            }
            int num = 1;
        Label_0174:
            num6 = num;
            if (num6 <= days)
            {
                UserStatisticsForDate date = new UserStatisticsForDate();
                if (num > 1)
                {
                    time = time2;
                }
                time2 = time.AddDays(1.0);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                date.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                date.TimePoint = time.Day;
                list.Add(date);
                num++;
                goto Label_0174;
            }
            return list;
        }

        public DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = DateTime.Now.AddDays(-6.0);
            DateTime time2 = new DateTime(time.Year, time.Month, time.Day);
            DateTime now = DateTime.Now;
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time2);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, now);
            decimal allSalesTotal = 0M;
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                allSalesTotal = Convert.ToDecimal(obj2);
            }
            DataTable table = this.CreateTable();
            for (int i = 0; i < 7; i++)
            {
                DateTime time4 = DateTime.Now.AddDays((double) -i);
                decimal salesTotal = this.GetDaySaleTotal(time4.Year, time4.Month, time4.Day, saleStatisticsType);
                this.InsertToTable(table, time4.Day, salesTotal, allSalesTotal);
            }
            return table;
        }

        public decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, 1, 1);
            DateTime time2 = time.AddYears(1);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        private void InsertToTable(DataTable table, int date, decimal salesTotal, decimal allSalesTotal)
        {
            DataRow row = table.NewRow();
            row["Date"] = date;
            row["SaleTotal"] = salesTotal;
            if (allSalesTotal != 0M)
            {
                row["Percentage"] = (salesTotal / allSalesTotal) * 100M;
            }
            else
            {
                row["Percentage"] = 0;
            }
            row["Lenth"] = ((decimal) row["Percentage"]) * 4M;
            table.Rows.Add(row);
        }
    }
}

