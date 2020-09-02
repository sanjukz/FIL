using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum DiscountValueType
    {
        Percentage = 1,
        Flat,
        Free
    }
}