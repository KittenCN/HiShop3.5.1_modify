namespace Hidistro.Entities.VShop
{
    using System;

    public enum LocationType
    {
        [EnumShowText("活动")]
        Activity = 2,
        [EnumShowText("地址")]
        Address = 10,
        [EnumShowText("品牌")]
        Brand = 12,
        [EnumShowText("分类页")]
        Category = 4,
        [EnumShowText("首页")]
        Home = 3,
        [EnumShowText("链接")]
        Link = 8,
        [EnumShowText("会员中心")]
        OrderCenter = 6,
        [EnumShowText("电话")]
        Phone = 9,
        [EnumShowText("购物车")]
        ShoppingCart = 5,
        [EnumShowText("投票")]
        Vote = 1
    }
}

