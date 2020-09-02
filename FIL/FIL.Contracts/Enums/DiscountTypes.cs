using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum DiscountTypes
    {
        None = 0,
        PromoCodeBased = 1,
        CardBinBased = 2,
        CustomerBased = 4,
        Season = 8,
        Family = 16,
        Combo = 32,
        BOGO = 64
    }
}