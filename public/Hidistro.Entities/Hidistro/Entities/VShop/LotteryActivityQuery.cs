namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class LotteryActivityQuery : Pagination
    {
        public LotteryActivityType ActivityType { get; set; }
    }
}

