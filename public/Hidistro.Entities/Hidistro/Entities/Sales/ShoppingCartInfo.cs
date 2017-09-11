namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ShoppingCartInfo
    {
        private bool isSendGift;
        private IList<ShoppingCartItemInfo> lineItems;
        private decimal timesPoint = 1M;

        public decimal GetAmount()
        {
            decimal num = 0M;
            foreach (ShoppingCartItemInfo info in this.lineItems)
            {
                num += info.SubTotal;
            }
            return num;
        }

        public int GetPoint()
        {
            return Globals.GetPoint(this.GetTotal());
        }

        public int GetQuantity()
        {
            int num = 0;
            foreach (ShoppingCartItemInfo info in this.lineItems)
            {
                num += info.Quantity;
            }
            return num;
        }

        public decimal GetTotal()
        {
            return (this.GetAmount() - this.ReducedPromotionAmount);
        }

        public int GetTotalPoint()
        {
            int num = 0;
            foreach (ShoppingCartItemInfo info in this.lineItems)
            {
                num += info.PointNumber * info.Quantity;
            }
            return num;
        }

        public decimal Amount { get; set; }

        public int CategoryId { get; set; }

        public decimal Exemption { get; set; }

        public int FreightFreePromotionId { get; set; }

        public string FreightFreePromotionName { get; set; }

        public int GetPointNumber { get; set; }

        public bool IsFreightFree { get; set; }

        public bool IsReduced { get; set; }

        public bool IsSendGift
        {
            get
            {
                foreach (ShoppingCartItemInfo info in this.lineItems)
                {
                    if (info.IsSendGift)
                    {
                        return true;
                    }
                }
                return this.isSendGift;
            }
            set
            {
                this.isSendGift = value;
            }
        }

        public bool IsSendTimesPoint { get; set; }

        public IList<ShoppingCartItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new List<ShoppingCartItemInfo>();
                }
                return this.lineItems;
            }
        }

        public int MemberPointNumber { get; set; }

        public decimal ReducedPromotionAmount { get; set; }

        public int ReducedPromotionId { get; set; }

        public string ReducedPromotionName { get; set; }

        public int SendGiftPromotionId { get; set; }

        public string SendGiftPromotionName { get; set; }

        public int SentTimesPointPromotionId { get; set; }

        public string SentTimesPointPromotionName { get; set; }

        public decimal ShipCost { get; set; }

        public string TemplateId { get; set; }

        public decimal TimesPoint
        {
            get
            {
                return this.timesPoint;
            }
            set
            {
                this.timesPoint = value;
            }
        }

        public decimal Total { get; set; }

        public decimal TotalWeight
        {
            get
            {
                decimal num = 0M;
                foreach (ShoppingCartItemInfo info in this.lineItems)
                {
                    num += info.GetSubWeight();
                }
                return num;
            }
        }

        public decimal Weight
        {
            get
            {
                decimal num = 0M;
                foreach (ShoppingCartItemInfo info in this.lineItems)
                {
                    if (!info.IsfreeShipping)
                    {
                        num += info.GetSubWeight();
                    }
                }
                return num;
            }
        }
    }
}

