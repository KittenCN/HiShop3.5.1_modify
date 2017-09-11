namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ShareActivitySearch : Pagination
    {
        public string CouponName { get; set; }

        public ShareActivityStatus status { get; set; }
    }
}

