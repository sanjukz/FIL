using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum SeatType
    {
        SeatSpace = 0,
        Available,
        Killed,
        Blocked,
        PartialView,
        ObstructedView,
        WheelChair,
        Companion,
    }
}