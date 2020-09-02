using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum OnlineEventTypes
    {
        None = 0,
        Prerecorded = 1,
        LivePerformance = 2,
        LivePerformanceWithArtist = 4,
        LivePerformanceWithGroup = 8
    }
}