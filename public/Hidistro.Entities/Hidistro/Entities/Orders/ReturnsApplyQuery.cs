namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ReturnsApplyQuery : Pagination
    {
        public int? HandleStatus { get; set; }

        public string OrderId { get; set; }

        public string ReturnsId { get; set; }
    }
}

