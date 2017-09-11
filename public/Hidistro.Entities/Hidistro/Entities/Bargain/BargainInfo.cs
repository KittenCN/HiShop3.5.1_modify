namespace Hidistro.Entities.Bargain
{
    using System;
    using System.Runtime.CompilerServices;

    public class BargainInfo
    {
        public string ActivityCover { get; set; }

        public int ActivityStock { get; set; }

        public int BargainType { get; set; }

        public float BargainTypeMaxVlue { get; set; }

        public float BargainTypeMinVlue { get; set; }

        public DateTime BeginDate { get; set; }

        public int CommissionDiscount { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal FloorPrice { get; set; }

        public int Id { get; set; }

        public decimal InitialPrice { get; set; }

        public bool IsCommission { get; set; }

        public int ProductId { get; set; }

        public int PurchaseNumber { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public int TranNumber { get; set; }
    }
}

