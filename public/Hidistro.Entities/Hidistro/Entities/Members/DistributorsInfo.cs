namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Enums;
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorsInfo : MemberInfo
    {
        public DateTime? AccountTime { get; set; }

        public string BackImage { get; set; }

        public DateTime CardCreatTime { get; set; }

        public DateTime CreateTime { get; set; }

        public DistributorGrade DistributorGradeId { get; set; }

        public int DistriGradeId { get; set; }

        public string Logo { get; set; }

        public decimal OrdersTotal { get; set; }

        public int? ParentUserId { get; set; }

        public decimal ReferralBlance { get; set; }

        public int ReferralOrders { get; set; }

        public string ReferralPath { get; set; }

        public decimal ReferralRequestBalance { get; set; }

        public int ReferralStatus { get; set; }

        public string RequestAccount { get; set; }

        public string StoreCard { get; set; }

        public string StoreDescription { get; set; }

        public string StoreName { get; set; }
    }
}

