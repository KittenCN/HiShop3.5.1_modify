namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class OneyuanTaoParticipantInfo
    {
        public string ActivityId { get; set; }

        public int BuyNum { get; set; }

        public DateTime BuyTime { get; set; }

        public bool IsDeliver { get; set; }

        public bool IsPay { get; set; }

        public bool IsReceived { get; set; }

        public bool IsRefund { get; set; }

        public bool IsSuccess { get; set; }

        public bool IsWin { get; set; }

        public string out_refund_no { get; set; }

        public string PayNum { get; set; }

        public DateTime? PayTime { get; set; }

        public string PayWay { get; set; }

        public string Pid { get; set; }

        public decimal ProductPrice { get; set; }

        public DateTime? ReceivedTime { get; set; }

        public bool RefundErr { get; set; }

        public string RefundNum { get; set; }

        public DateTime? RefundTime { get; set; }

        public string Remark { get; set; }

        public string ShipingCompany { get; set; }

        public string ShippingNum { get; set; }

        public DateTime? ShippingTime { get; set; }

        public string SkuId { get; set; }

        public string SkuIdStr { get; set; }

        public decimal TotalPrice { get; set; }

        public int UserId { get; set; }

        public string WinIds { get; set; }

        public DateTime? WinTime { get; set; }
    }
}

