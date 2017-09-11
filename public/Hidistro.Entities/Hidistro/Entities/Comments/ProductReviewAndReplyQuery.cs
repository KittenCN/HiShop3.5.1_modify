namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewAndReplyQuery : Pagination
    {
        public virtual int ProductId { get; set; }

        public long ReviewId { get; set; }
    }
}

