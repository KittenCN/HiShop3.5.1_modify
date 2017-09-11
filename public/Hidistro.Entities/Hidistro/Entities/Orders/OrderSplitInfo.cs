namespace Hidistro.Entities.Orders
{
    using System;
    using System.Runtime.CompilerServices;

    public class OrderSplitInfo
    {
        public decimal AdjustedFreight { get; set; }

        public int Id { get; set; }

        public string ItemList { get; set; }

        public string OldOrderId { get; set; }

        public int OrderIDNum { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}

