namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class MemberAmountDetailedInfo
    {
        public decimal AvailableAmount { get; set; }

        public string GatewayPayId { get; set; }

        public int Id { get; set; }

        public string PayId { get; set; }

        public string Remark { get; set; }

        public int State { get; set; }

        public decimal TradeAmount { get; set; }

        public DateTime TradeTime { get; set; }

        public Hidistro.Entities.Members.TradeType TradeType { get; set; }

        public Hidistro.Entities.Members.TradeWays TradeWays { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}

