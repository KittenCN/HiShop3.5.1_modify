namespace Hidistro.Core.Entities
{
    using System;
    using System.Runtime.CompilerServices;

    public class DbQueryResult
    {
        public object Data { get; set; }

        public int TotalRecords { get; set; }
    }
}

