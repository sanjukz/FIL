using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum AddressTypes
    {
        None = 0,
        Shipping = 1,
        Billing = 2
    }
}