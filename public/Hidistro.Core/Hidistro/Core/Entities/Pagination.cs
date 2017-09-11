namespace Hidistro.Core.Entities
{
    using Hidistro.Core.Enums;
    using System;
    using System.Runtime.CompilerServices;

    public class Pagination
    {
        public Pagination()
        {
            this.IsCount = true;
            this.PageSize = 10;
        }

        public int DeleteBeforeState { get; set; }

        public bool IsCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortBy { get; set; }

        public SortAction SortOrder { get; set; }
    }
}

