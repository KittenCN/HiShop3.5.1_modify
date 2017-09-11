namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class LimitedTimeDiscountProductInfo
    {
        public DateTime BeginTime { get; set; }

        public DateTime CreateTime { get; set; }

        public decimal Discount { get; set; }

        public DateTime EndTime { get; set; }

        public decimal FinalPrice { get; set; }

        public int IsChamferPoint { get; set; }

        public int IsDehorned { get; set; }

        public int LimitedTimeDiscountId { get; set; }

        public int LimitedTimeDiscountProductId { get; set; }

        public decimal Minus { get; set; }

        public int ProductId { get; set; }

        public int Status { get; set; }
    }
}

