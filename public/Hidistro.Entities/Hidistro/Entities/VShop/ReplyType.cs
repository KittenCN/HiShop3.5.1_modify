namespace Hidistro.Entities.VShop
{
    using System;

    public enum ReplyType
    {
        [EnumShowText("关键字回复")]
        Keys = 2,
        [EnumShowText("无匹配回复")]
        NoMatch = 4,
        None = 0,
        [EnumShowText("刮刮卡")]
        Scratch = 0x10,
        [EnumShowText("微报名")]
        SignUp = 0x100,
        [EnumShowText("砸金蛋")]
        SmashEgg = 0x20,
        [EnumShowText("关注时回复")]
        Subscribe = 1,
        [EnumShowText("微抽奖")]
        Ticket = 0x40,
        [EnumShowText("微专题")]
        Topic = 0x200,
        [EnumShowText("微投票")]
        Vote = 0x80,
        [EnumShowText("大转盘")]
        Wheel = 8
    }
}

