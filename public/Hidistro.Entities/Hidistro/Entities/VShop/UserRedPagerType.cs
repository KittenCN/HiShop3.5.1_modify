namespace Hidistro.Entities.VShop
{
    using System;

    public enum UserRedPagerType
    {
        [EnumShowText("所有")]
        All = 0,
        [EnumShowText("已过期")]
        Expiry = 2,
        [EnumShowText("可用")]
        Usable = 1
    }
}

