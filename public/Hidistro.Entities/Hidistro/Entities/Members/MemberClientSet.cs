namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class MemberClientSet
    {
        public string ClientChar { get; set; }

        public int ClientTypeId { get; set; }

        public decimal ClientValue { get; set; }

        public DateTime? EndTime { get; set; }

        public int LastDay { get; set; }

        public DateTime? StartTime { get; set; }
    }
}

