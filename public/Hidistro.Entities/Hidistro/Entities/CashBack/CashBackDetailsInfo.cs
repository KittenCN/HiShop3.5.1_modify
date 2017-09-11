namespace Hidistro.Entities.CashBack
{
    using System;
    using System.Runtime.CompilerServices;

    public class CashBackDetailsInfo
    {
        public decimal CashBackAmount { get; set; }

        public DateTime CashBackDate { get; set; }

        public int CashBackDetailId { get; set; }

        public int CashBackId { get; set; }

        public CashBackTypes CashBackType { get; set; }

        public int UserId { get; set; }
    }
}

