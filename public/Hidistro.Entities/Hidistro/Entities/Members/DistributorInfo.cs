namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorInfo : MemberInfo
    {
        public DateTime CreateTime { get; set; }

        public string DistributorGradeId { get; set; }

        public string StoreName { get; set; }
    }
}

