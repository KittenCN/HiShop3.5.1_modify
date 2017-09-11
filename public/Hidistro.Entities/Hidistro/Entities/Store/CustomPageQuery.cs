namespace Hidistro.Entities.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CustomPageQuery : Pagination
    {
        public string Name { get; set; }

        public int? Status { get; set; }
    }
}

