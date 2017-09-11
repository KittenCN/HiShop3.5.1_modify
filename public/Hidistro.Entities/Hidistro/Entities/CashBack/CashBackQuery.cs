namespace Hidistro.Entities.CashBack
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CashBackQuery : Pagination
    {
        public Hidistro.Entities.CashBack.CashBackTypes? CashBackTypes { get; set; }

        public string Cellphone { get; set; }

        public bool? IsFinished { get; set; }

        public bool? IsValid { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }
    }
}

