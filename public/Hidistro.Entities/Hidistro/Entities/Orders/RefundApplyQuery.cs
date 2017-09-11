namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class RefundApplyQuery : Pagination
    {
        public int? HandleStatus { get; set; }

        public string OrderId { get; set; }
    }
}

