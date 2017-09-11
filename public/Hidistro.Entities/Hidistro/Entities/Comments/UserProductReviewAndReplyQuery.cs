namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class UserProductReviewAndReplyQuery : Pagination
    {
        public int ProductId { get; set; }
    }
}

