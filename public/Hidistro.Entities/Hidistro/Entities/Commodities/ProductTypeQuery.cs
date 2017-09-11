namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductTypeQuery : Pagination
    {
        public string TypeName { get; set; }
    }
}

