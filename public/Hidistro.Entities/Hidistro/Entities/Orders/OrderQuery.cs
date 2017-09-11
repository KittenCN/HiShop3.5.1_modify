namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OrderQuery : Pagination
    {
        public DateTime? EndDate { get; set; }

        public string Gateway { get; set; }

        public int? GroupBuyId { get; set; }

        public int? IsPrinted { get; set; }

        public string OrderId { get; set; }

        public OrderStatus? OrderItemsStatus { get; set; }

        public int? PaymentType { get; set; }

        public string ProductName { get; set; }

        public int? ReferralUserId { get; set; }

        public int? RegionId { get; set; }

        public string ShipId { get; set; }

        public int? ShippingModeId { get; set; }

        public string ShipTo { get; set; }

        public DateTime? StartDate { get; set; }

        public OrderStatus Status { get; set; }

        public string StoreName { get; set; }

        public OrderType? Type { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }

        public enum OrderType
        {
            GroupBuy = 2,
            NormalProduct = 1
        }
    }
}

