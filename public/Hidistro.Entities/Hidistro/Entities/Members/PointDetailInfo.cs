namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class PointDetailInfo
    {
        public int? Increased { get; set; }

        public long JournalNumber { get; set; }

        public string OrderId { get; set; }

        public int Points { get; set; }

        public int? Reduced { get; set; }

        public string Remark { get; set; }

        public DateTime TradeDate { get; set; }

        public PointTradeType TradeType { get; set; }

        public int UserId { get; set; }
    }
}

