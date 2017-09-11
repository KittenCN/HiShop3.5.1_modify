namespace Hidistro.Entities.Insurance
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class InsuranceOrderQuery : Pagination
    {
        public Hidistro.Entities.Insurance.InsuranceOrderTypes? CashBackTypes { get; set; }

        public string Cellphone { get; set; }

        public string OpenId { get; set; }

        public bool? IsFinished { get; set; }

        public bool? IsValid { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }
    }
}

