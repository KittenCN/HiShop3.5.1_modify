namespace Hidistro.Entities.Weibo
{
    using Hidistro.Entities.VShop;
    using System;

    public enum LinkType
    {
        [EnumShowText("阅读原文")]
        ArticleDetail = 1,
        [EnumShowText("商品及分类")]
        Category = 2,
        [EnumShowText("店铺主页")]
        HomePage = 4,
        [EnumShowText("自定义链接")]
        Userdefined = 8
    }
}

