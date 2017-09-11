namespace Hidistro.Entities.Orders
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class OrderStatisticsInfo
    {
        public DataTable OrderTbl { get; set; }

        public decimal ProfitsOfPage { get; set; }

        public decimal ProfitsOfSearch { get; set; }

        public int TotalCount { get; set; }

        public decimal TotalOfPage { get; set; }

        public decimal TotalOfSearch { get; set; }
    }
}

