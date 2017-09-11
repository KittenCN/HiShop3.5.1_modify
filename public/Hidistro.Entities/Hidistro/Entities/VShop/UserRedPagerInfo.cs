namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class UserRedPagerInfo
    {
        public decimal Amount { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; }

        public decimal OrderAmountCanUse { get; set; }

        public string OrderID { get; set; }

        public string RedPagerActivityName { get; set; }

        public int RedPagerID { get; set; }

        public int UserID { get; set; }
    }
}

