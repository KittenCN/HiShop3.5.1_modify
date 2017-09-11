namespace Hidistro.Entities.Members
{
    using System;

    public enum RequesState
    {
        None = -2,
        驳回 = -1,
        发放异常 = 3,
        未审核 = 0,
        已发放 = 2,
        已审核 = 1
    }
}

