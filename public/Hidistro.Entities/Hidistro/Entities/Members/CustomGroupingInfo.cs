namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class CustomGroupingInfo
    {
        public string GroupName { get; set; }

        public int Id { get; set; }

        public string Memo { get; set; }

        public DateTime UpdateTime { get; set; }

        public int UserCount { get; set; }
    }
}

