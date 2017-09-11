namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CouponItemInfoQuery : Pagination
    {
        public string CounponName { get; set; }

        public int? CouponId { get; set; }

        public int? CouponStatus { get; set; }

        public string OrderId { get; set; }

        public string UserName { get; set; }
    }
}

