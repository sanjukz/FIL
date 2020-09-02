using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum PerformanceType
    {
        None = 0,
        Individual = 1,
        Group = 2
    }
}