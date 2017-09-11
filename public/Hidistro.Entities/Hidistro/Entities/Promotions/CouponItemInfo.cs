namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CouponItemInfo
    {
        public CouponItemInfo()
        {
        }

        public CouponItemInfo(int couponId, string claimCode, int? userId, string username, string emailAddress, DateTime generateTime)
        {
            this.CouponId = couponId;
            this.ClaimCode = claimCode;
            this.UserId = userId;
            this.UserName = username;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
        }

        public string ClaimCode { get; set; }

        public int CouponId { get; set; }

        public int? CouponStatus { get; set; }

        public string EmailAddress { get; set; }

        public DateTime GenerateTime { get; set; }

        public string OrderId { get; set; }

        public DateTime? UsedTime { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }
    }
}

