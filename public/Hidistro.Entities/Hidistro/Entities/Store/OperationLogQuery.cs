namespace Hidistro.Entities.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OperationLogQuery
    {
        public OperationLogQuery()
        {
            this.Page = new Pagination();
        }

        public DateTime? FromDate { get; set; }

        public string OperationUserName { get; set; }

        public Pagination Page { get; set; }

        public DateTime? ToDate { get; set; }
    }
}

