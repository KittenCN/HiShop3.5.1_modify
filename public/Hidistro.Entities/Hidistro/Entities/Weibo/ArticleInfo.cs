namespace Hidistro.Entities.Weibo
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ArticleInfo
    {
        public int ArticleId { get; set; }

        public Hidistro.Entities.Weibo.ArticleType ArticleType { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public bool IsShare { get; set; }

        public IList<ArticleItemsInfo> ItemsInfo { get; set; }

        public Hidistro.Entities.Weibo.LinkType LinkType { get; set; }

        public string MediaId { get; set; }

        public string Memo { get; set; }

        public DateTime PubTime { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}

