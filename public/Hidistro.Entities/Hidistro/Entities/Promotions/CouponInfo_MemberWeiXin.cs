namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CouponInfo_MemberWeiXin : CouponInfo
    {
        public int Id { get; set; }

        public string OpenId { get; set; }

        public string ValidDays { get; set; }
    }
}

