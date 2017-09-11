namespace Hidistro.Entities.CashBack
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CashBackDetailsQuery : Pagination
    {
        public int CashBackId { get; set; }

        public int? UserId { get; set; }
    }
}

