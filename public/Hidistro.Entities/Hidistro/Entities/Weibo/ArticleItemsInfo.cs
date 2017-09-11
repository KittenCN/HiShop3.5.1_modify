namespace Hidistro.Entities.Weibo
{
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleItemsInfo
    {
        public int ArticleId { get; set; }

        public string Content { get; set; }

        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public bool IsShare { get; set; }

        public Hidistro.Entities.Weibo.LinkType LinkType { get; set; }

        public string MediaId { get; set; }

        public DateTime PubTime { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}

