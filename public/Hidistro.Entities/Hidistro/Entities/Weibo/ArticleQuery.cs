namespace Hidistro.Entities.Weibo
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleQuery : Pagination
    {
        public ArticleQuery()
        {
            this.IsShare = -1;
        }

        public int ArticleType { get; set; }

        public int IsShare { get; set; }

        public string Memo { get; set; }

        public string Title { get; set; }
    }
}

