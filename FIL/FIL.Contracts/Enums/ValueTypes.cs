using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum ValueTypes
    {
        None = 0,
        Percentage = 1,
        Flat = 2,
        Free = 3
    }
}