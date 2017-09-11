namespace Hidistro.Entities.Sales
{
    using System;

    public enum PaymentModeActionStatus
    {
        DuplicateGateway = 3,
        DuplicateName = 1,
        OutofNumber = 2,
        Success = 0,
        UnknowError = 0x63
    }
}

