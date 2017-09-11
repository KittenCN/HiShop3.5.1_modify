namespace Hidistro.Entities.Commodities
{
    using System;

    public enum ProductActionStatus
    {
        AttributeError = 4,
        DuplicateName = 1,
        DuplicateSKU = 2,
        OffShelfError = 5,
        ProductTagEroor = 6,
        SKUError = 3,
        Success = 0,
        UnknowError = 0x63
    }
}

