namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberCouponsSearch : Pagination
    {
        public string CouponName { get; set; }

        public int MemberId { get; set; }

        public string OrderNo { get; set; }

        public string Status { get; set; }
    }
}

