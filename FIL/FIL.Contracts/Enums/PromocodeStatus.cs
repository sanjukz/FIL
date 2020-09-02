using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum PromoCodeStatus
    {
        None = 0,
        Available = 1,
        Intrasition = 2,
        NotAvailable = 4,
        Reverted = 8
    }
}