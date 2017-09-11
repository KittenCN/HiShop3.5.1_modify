namespace Hidistro.Entities.Promotions
{
    using System;

    public enum GroupBuyStatus
    {
        EndUntreated = 2,
        Failed = 4,
        FailedWaitForReFund = 5,
        Success = 3,
        UnderWay = 1
    }
}

