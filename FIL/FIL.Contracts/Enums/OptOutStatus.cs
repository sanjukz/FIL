using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum OptOutStatus
    {
        None = 0,
        OptInMarketing = 1,
        OptInUpdates = 2
    }
}