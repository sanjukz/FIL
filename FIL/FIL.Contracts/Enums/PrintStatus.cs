using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum PrintStatus
    {
        None = 0,
        NotPrinted = 1,
        Printed = 2,
        Reprint = 4
    }
}