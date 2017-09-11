namespace Hidistro.Entities.StatisticsReport
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OrderStatisticsQuery_UnderShop : Pagination
    {
        public int? AgentId { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ShopLevel { get; set; }

        public int? Top { get; set; }
    }
}

