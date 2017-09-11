namespace Hidistro.Entities.Promotions
{
    using System;

    public enum CouponActionStatus
    {
        CreateClaimCodeError = 7,
        CreateClaimCodeSuccess = 6,
        Disabled = 3,
        DuplicateName = 1,
        InvalidClaimCode = 2,
        OutOfExpiryDate = 5,
        OutOfTimes = 4,
        Success = 0,
        UnknowError = 0x63
    }
}

