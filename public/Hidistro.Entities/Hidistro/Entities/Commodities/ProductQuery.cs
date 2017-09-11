namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductQuery : Pagination
    {
        public int? BrandId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsIncludeHomeProduct { get; set; }

        public bool? IsIncludePromotionProduct { get; set; }

        public int? IsMakeTaobao { get; set; }

        [HtmlCoding]
        public string Keywords { get; set; }

        public string MaiCategoryPath { get; set; }

        public decimal? maxPrice { get; set; }

        public decimal? MaxSalePrice { get; set; }

        public decimal? minPrice { get; set; }

        public decimal? MinSalePrice { get; set; }

        [HtmlCoding]
        public string ProductCode { get; set; }

        public ProductSaleStatus SaleStatus { get; set; }

        public string selectQuery { get; set; }

        public DateTime? StartDate { get; set; }

        public int? Stock { get; set; }

        public int? TagId { get; set; }

        public int? TopicId { get; set; }

        public string TwoSaleStatus { get; set; }

        public int? TypeId { get; set; }
    }
}

