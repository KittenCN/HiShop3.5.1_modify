namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CouponEdit
    {
        public DateTime? begin { get; set; }

        public DateTime? end { get; set; }

        public int? maxReceivNum { get; set; }

        public int? totalNum { get; set; }
    }
}

