using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum InventoryType
    {
        Seated = 1,
        SeatedPrePrinted,
        NoneSeated,
        NoneSeatedPrePrinted,
        SeatedWithSeatSelection
    }
}