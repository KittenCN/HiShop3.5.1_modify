namespace Hishop.Weixin.MP.Response
{
    using Domain;
    using Hishop.Weixin.MP;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NewsResponse : AbstractResponse
    {
        public int ArticleCount
        {
            get
            {
                if (this.Articles != null)
                {
                    return this.Articles.Count;
                }
                return 0;
            }
        }

        public IList<Article> Articles { get; set; }

        public override ResponseMsgType MsgType
        {
            get
            {
                return ResponseMsgType.News;
            }
        }
    }
}

