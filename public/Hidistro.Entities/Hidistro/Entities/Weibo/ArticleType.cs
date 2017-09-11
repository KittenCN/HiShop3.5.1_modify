namespace Hidistro.Entities.Weibo
{
    using Hidistro.Entities.VShop;
    using System;

    public enum ArticleType
    {
        [EnumShowText("多图文")]
        List = 4,
        [EnumShowText("单图文")]
        News = 2,
        [EnumShowText("文本")]
        Text = 1
    }
}

