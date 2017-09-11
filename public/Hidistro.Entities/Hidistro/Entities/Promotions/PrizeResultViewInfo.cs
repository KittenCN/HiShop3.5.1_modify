namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class PrizeResultViewInfo : PrizeResultInfo
    {
        public string GiveCouponId { get; set; }

        public int GivePoint { get; set; }

        public string GiveShopBookId { get; set; }

        public string Prize { get; set; }

        public Hidistro.Entities.Promotions.PrizeGrade PrizeGrade { get; set; }

        public string PrizeName { get; set; }

        public Hidistro.Entities.Promotions.PrizeType PrizeType { get; set; }

        public string UserName { get; set; }
    }
}

