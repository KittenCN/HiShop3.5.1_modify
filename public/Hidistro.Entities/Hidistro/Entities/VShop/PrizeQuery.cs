namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class PrizeQuery : Pagination
    {
        public int ActivityId { get; set; }

        public LotteryActivityType ActivityType { get; set; }

        public string CellPhone { get; set; }

        public bool IsPrize { get; set; }

        public string UserName { get; set; }
    }
}

