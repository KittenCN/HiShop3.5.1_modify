namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorGradeInfo
    {
        public decimal AddCommission { get; set; }

        public decimal CommissionsLimit { get; set; }

        public string Description { get; set; }

        public decimal FirstCommissionRise { get; set; }

        public int GradeId { get; set; }

        public string Ico { get; set; }

        public bool IsDefault { get; set; }

        public string Name { get; set; }

        public decimal SecondCommissionRise { get; set; }

        public decimal ThirdCommissionRise { get; set; }
    }
}

