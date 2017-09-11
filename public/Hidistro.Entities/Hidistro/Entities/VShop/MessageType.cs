namespace Hidistro.Entities.VShop
{
    using System;

    public enum MessageType
    {
        [EnumShowText("图片")]
        Image = 8,
        [EnumShowText("多图文")]
        List = 4,
        [EnumShowText("单图文")]
        News = 2,
        [EnumShowText("文本")]
        Text = 1
    }
}

