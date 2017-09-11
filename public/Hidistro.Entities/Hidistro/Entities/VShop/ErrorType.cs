namespace Hidistro.Entities.VShop
{
    using System;

    public enum ErrorType
    {
        后台错误 = 4,
        前台404 = 1,
        前台其它错误 = 3,
        前台商品下架 = 2,
        无权限 = 5
    }
}

