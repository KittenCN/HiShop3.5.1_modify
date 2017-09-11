namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CouponsSearch : Pagination
    {
        public DateTime? beginDate { get; set; }

        public string CouponName { get; set; }

        public DateTime? endDate { get; set; }

        public bool? Finished { get; set; }

        public decimal? maxValue { get; set; }

        public decimal? minValue { get; set; }

        public int? SearchType { get; set; }
    }
}

