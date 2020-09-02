using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum VenueLayoutArea
    {
        None = 0,
        Stand = 1,
        Level,
        Block,
        Section
    }
}