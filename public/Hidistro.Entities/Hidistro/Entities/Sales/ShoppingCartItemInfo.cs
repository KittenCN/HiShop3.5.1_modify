namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class ShoppingCartItemInfo
    {
        public decimal GetSubWeight()
        {
            return (this.Weight * this.Quantity);
        }

        public decimal AdjustedPrice { get; set; }

        public decimal CubicMeter { get; set; }

        public int ExchangeId { get; set; }

        public decimal FirstCommission { get; set; }

        public int FreightTemplateId { get; set; }

        public decimal FreightWeight { get; set; }

        public bool IsfreeShipping { get; set; }

        public bool IsSendGift { get; set; }

        public bool IsSetCommission { get; set; }

        public int LimitedTimeDiscountId { get; set; }

        public string MainCategoryPath { get; set; }

        public decimal MemberPrice { get; set; }

        public string Name { get; set; }

        public int PointNumber { get; set; }

        public int ProductId { get; set; }

        public int ProductNumber { get; set; }

        public int PromotionId { get; set; }

        public string PromotionName { get; set; }

        public int Quantity { get; set; }

        public decimal SecondCommission { get; set; }

        public int ShippQuantity { get; set; }

        public string SKU { get; set; }

        public string SkuContent { get; set; }

        public string SkuId { get; set; }

        public int Status { get; set; }

        public decimal SubTotal
        {
            get
            {
                return (this.AdjustedPrice * this.Quantity);
            }
        }

        public decimal SumSubTotal { get; set; }

        public decimal ThirdCommission { get; set; }

        public string ThumbnailUrl100 { get; set; }

        public string ThumbnailUrl40 { get; set; }

        public string ThumbnailUrl60 { get; set; }

        public int Type { get; set; }

        public int UserId { get; set; }

        public decimal Weight { get; set; }
    }
}

