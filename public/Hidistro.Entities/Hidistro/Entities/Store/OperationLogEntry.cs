namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class OperationLogEntry
    {
        public DateTime AddedTime { get; set; }

        public string Description { get; set; }

        public string IpAddress { get; set; }

        public long LogId { get; set; }

        public string PageUrl { get; set; }

        public Hidistro.Entities.Store.Privilege Privilege { get; set; }

        public string UserName { get; set; }
    }
}

