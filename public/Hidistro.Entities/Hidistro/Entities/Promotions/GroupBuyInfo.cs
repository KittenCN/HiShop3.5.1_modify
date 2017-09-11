namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;

    public class GroupBuyInfo
    {
        [HtmlCoding]
        public string Content { get; set; }

        public int Count { get; set; }

        public DateTime EndDate { get; set; }

        public int GroupBuyId { get; set; }

        public int MaxCount { get; set; }

        public decimal NeedPrice { get; set; }

        public decimal Price { get; set; }

        public int ProdcutQuantity { get; set; }

        public int ProductId { get; set; }

        public int SoldCount { get; set; }

        public DateTime StartDate { get; set; }

        public GroupBuyStatus Status { get; set; }
    }
}

