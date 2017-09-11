namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class GamePrizeInfo
    {
        public int GameId { get; set; }

        public string GiveCouponId { get; set; }

        public int GivePoint { get; set; }

        public string GiveShopBookId { get; set; }

        public string GriveShopBookPicUrl { get; set; }

        public int IsLogistics { get; set; }

        public string Prize { get; set; }

        public int PrizeCount { get; set; }

        public Hidistro.Entities.Promotions.PrizeGrade PrizeGrade { get; set; }

        public int PrizeId { get; set; }

        public string PrizeImage { get; set; }

        public string PrizeName { get; set; }

        public int PrizeRate { get; set; }

        public Hidistro.Entities.Promotions.PrizeType PrizeType { get; set; }
    }
}

