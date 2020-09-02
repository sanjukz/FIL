using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum AllocationType
    {
        Match = 1,
        Venue,
        Sponsor
    }
}