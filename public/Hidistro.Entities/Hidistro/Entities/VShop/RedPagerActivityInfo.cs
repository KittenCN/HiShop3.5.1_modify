namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class RedPagerActivityInfo
    {
        public int CategoryId { get; set; }

        public int ExpiryDays { get; set; }

        public bool IsOpen { get; set; }

        public decimal ItemAmountLimit { get; set; }

        public int MaxGetTimes { get; set; }

        public decimal MinOrderAmount { get; set; }

        public string Name { get; set; }

        public decimal OrderAmountCanUse { get; set; }

        public string OrderId { get; set; }

        public int RedPagerActivityId { get; set; }
    }
}

