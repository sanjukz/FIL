using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum TiqetOrderStatus
    {
        None = 0,
        New = 1,
        Pending = 2,
        Failed = 4,
        Done = 8,
        Reverted = 16
    }
}