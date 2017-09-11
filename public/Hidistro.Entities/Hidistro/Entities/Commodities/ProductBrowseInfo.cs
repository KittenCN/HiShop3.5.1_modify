namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class ProductBrowseInfo
    {
        public string BrandName { get; set; }

        public string CategoryName { get; set; }

        public DataTable DbAttribute { get; set; }

        public DataTable DBConsultations { get; set; }

        public DataTable DbCorrelatives { get; set; }

        public DataTable DBReviews { get; set; }

        public DataTable DbSKUs { get; set; }

        public ProductInfo Product { get; set; }
    }
}

