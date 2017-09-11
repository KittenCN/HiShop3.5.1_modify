namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberAmountQuery : Pagination
    {
        public string EndTime { get; set; }

        public string PayId { get; set; }

        public string StartTime { get; set; }

        public string TradeType { get; set; }

        public string TradeWays { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}

