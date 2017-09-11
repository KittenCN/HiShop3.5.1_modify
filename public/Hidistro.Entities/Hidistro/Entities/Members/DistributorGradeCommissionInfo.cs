namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorGradeCommissionInfo
    {
        public decimal Commission { get; set; }

        public int CommType { get; set; }

        public int Id { get; set; }

        public string Memo { get; set; }

        public decimal OldCommissionTotal { get; set; }

        public string OperAdmin { get; set; }

        public string OrderID { get; set; }

        public DateTime PubTime { get; set; }

        public int ReferralUserId { get; set; }

        public int UserId { get; set; }
    }
}

