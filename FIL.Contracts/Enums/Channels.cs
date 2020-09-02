using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum Channels
    {
        None = 0,
        Website = 1,
        Retail = 2,
        Boxoffice = 4,
        Corporate = 8,
        MobileApp = 16,
        Feel = 32,
        ZSuite = 64,
    }
}