namespace Hidistro.Entities.StatisticsReport
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OrderStatisticsQuery : Pagination
    {
        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsNoPage { get; set; }

        public int? Top { get; set; }
    }
}

