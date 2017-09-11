namespace Hidistro.Entities.Promotions
{
    using System;

    public enum CanPrizeError
    {
        超过每人每天限次 = 3,
        超过每人最多限次 = 4,
        此抽奖活动不在有效期内 = 7,
        此抽奖活动还没开始 = 8,
        会员等级不在此活动范围内 = 1,
        积分不够 = 2,
        可以玩 = 0
    }
}

