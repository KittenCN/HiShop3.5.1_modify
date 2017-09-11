namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class OneyuanTaoWinRecordInfo
    {
        public int ActivityId { get; set; }

        public int id { get; set; }

        public bool IsDeliver { get; set; }

        public bool IsReceived { get; set; }

        public string PrizeNum { get; set; }

        public DateTime? ReceivedTime { get; set; }

        public string ShipingCompany { get; set; }

        public string ShippingId { get; set; }

        public string ShippingNum { get; set; }

        public DateTime? ShippingTime { get; set; }

        public int UserId { get; set; }
    }
}

