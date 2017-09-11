namespace Hidistro.Entities.Commodities
{
    using System;

    public enum CategoryActionStatus
    {
        DeleteForbid = 2,
        DeleteForbidProducts = 3,
        DuplicateName = 1,
        Success = 0,
        UnknowError = 0x63,
        UpdateParentError = 4
    }
}

