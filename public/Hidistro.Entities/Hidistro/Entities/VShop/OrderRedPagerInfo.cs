namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class OrderRedPagerInfo
    {
        public int AlreadyGetTimes { get; set; }

        public int ExpiryDays { get; set; }

        public decimal ItemAmountLimit { get; set; }

        public int MaxGetTimes { get; set; }

        public decimal OrderAmountCanUse { get; set; }

        public string OrderID { get; set; }

        public int RedPagerActivityId { get; set; }

        public string RedPagerActivityName { get; set; }

        public int UserID { get; set; }
    }
}

