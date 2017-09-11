namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class DebitNoteQuery : Pagination
    {
        public string OrderId { get; set; }
    }
}

