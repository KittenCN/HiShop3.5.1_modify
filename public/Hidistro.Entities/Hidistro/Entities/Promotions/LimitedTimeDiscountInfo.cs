namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class LimitedTimeDiscountInfo
    {
        public string ActivityName { get; set; }

        public string ApplyMembers { get; set; }

        public DateTime BeginTime { get; set; }

        public int CommissionDiscount { get; set; }

        public DateTime CreateTime { get; set; }

        public string CustomGroup { get; set; }

        public string DefualtGroup { get; set; }

        public string Description { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsCommission { get; set; }

        public int LimitedTimeDiscountId { get; set; }

        public int LimitNumber { get; set; }

        public string Status { get; set; }
    }
}

