namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class SaleStatisticsAnalyzeInfo
    {
        public int OrderCounts { get; set; }

        public decimal OrderTotals { get; set; }

        public int OrderUserCounts { get; set; }

        public int UserCounts { get; set; }

        public int VisitCounts { get; set; }
    }
}

