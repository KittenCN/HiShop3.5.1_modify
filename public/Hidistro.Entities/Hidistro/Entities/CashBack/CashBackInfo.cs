namespace Hidistro.Entities.CashBack
{
    using System;
    using System.Runtime.CompilerServices;

    public class CashBackInfo
    {
        public decimal CashBackAmount { get; set; }

        public int CashBackId { get; set; }

        public CashBackTypes CashBackType { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? FinishedDate { get; set; }

        public bool IsFinished { get; set; }

        public bool IsValid { get; set; }

        public decimal Percentage { get; set; }

        public decimal RechargeAmount { get; set; }

        public int UserId { get; set; }
    }
}

