namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorGradeQuery : Pagination
    {
        public string Description { get; set; }

        public int GradeId { get; set; }

        public string Name { get; set; }
    }
}

