using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum DiscountCustomersTypes
    {
        None = 0,
        Email = 1,
        Mobile = 2
    }
}