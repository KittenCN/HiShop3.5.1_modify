namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Runtime.CompilerServices;

    public class AdvancedProductQuery : ProductQuery
    {
        public bool IncludeInStock { get; set; }

        public bool IncludeOnSales { get; set; }

        public bool IncludeUnSales { get; set; }
    }
}

