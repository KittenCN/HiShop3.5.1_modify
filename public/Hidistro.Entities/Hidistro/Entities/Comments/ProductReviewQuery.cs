namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewQuery : Pagination
    {
        public int? CategoryId { get; set; }

        [HtmlCoding]
        public string Keywords { get; set; }

        [HtmlCoding]
        public string ProductCode { get; set; }

        public int productId { get; set; }
    }
}

