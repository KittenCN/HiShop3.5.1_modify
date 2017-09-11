namespace Hidistro.Entities.Orders
{
    using System;
    using System.Runtime.CompilerServices;

    public class LineItemInfo
    {
        public decimal GetSubTotal()
        {
            return (this.ItemAdjustedPrice * this.Quantity);
        }

        public decimal BalancePayMoney { get; set; }

        public int CommissionDiscount { get; set; }

        public decimal DiscountAverage { get; set; }

        public int ExchangeId { get; set; }

        public decimal FirstCommission { get; set; }

        public int ID { get; set; }

        public bool IsAdminModify { get; set; }

        public bool IsSetCommission { get; set; }

        public decimal ItemAdjustedCommssion { get; set; }

        public decimal ItemAdjustedPrice { get; set; }

        public decimal ItemCostPrice { get; set; }

        public string ItemDescription { get; set; }

        public decimal ItemListPrice { get; set; }

        public decimal ItemsCommission { get; set; }

        public decimal ItemsCommissionScale { get; set; }

        public decimal ItemWeight { get; set; }

        public int LimitedTimeDiscountId { get; set; }

        public string MainCategoryPath { get; set; }

        public string OrderID { get; set; }

        public Hidistro.Entities.Orders.OrderStatus OrderItemsStatus { get; set; }

        public Hidistro.Entities.Orders.OrderStatus OrderStatus { get; set; }

        public int PointNumber { get; set; }

        public int ProductId { get; set; }

        public int PromotionId { get; set; }

        public string PromotionName { get; set; }

        public int Quantity { get; set; }

        public decimal ReturnMoney { get; set; }

        public decimal SecondCommission { get; set; }

        public decimal SecondItemsCommission { get; set; }

        public decimal SecondItemsCommissionScale { get; set; }

        public int ShipmentQuantity { get; set; }

        public string SKU { get; set; }

        public string SKUContent { get; set; }

        public string SkuId { get; set; }

        public decimal ThirdCommission { get; set; }

        public decimal ThirdItemsCommission { get; set; }

        public decimal ThirdItemsCommissionScale { get; set; }

        public string ThumbnailsUrl { get; set; }

        public int Type { get; set; }
    }
}

