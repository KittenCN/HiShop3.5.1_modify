namespace Hidistro.Entities.FenXiao
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorGradeCommissionQuery : Pagination
    {
        public DateTime? EndTime { get; set; }

        public DateTime? StartTime { get; set; }

        public string Title { get; set; }
    }
}

