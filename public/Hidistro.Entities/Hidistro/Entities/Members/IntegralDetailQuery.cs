namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class IntegralDetailQuery : Pagination
    {
        public DateTime? EndTime { get; set; }

        public int IntegralSourceType { get; set; }

        public int IntegralStatus { get; set; }

        public DateTime? StartTime { get; set; }

        public int UserId { get; set; }
    }
}

