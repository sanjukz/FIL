using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum SeatStatus
    {
        None = 0,
        UnSold = 1,
        Sold = 2,
        BlockedByCustomer = 4,
        BlockedforSponsor = 8
    }
}