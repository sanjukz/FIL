using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum OrderType
    {
        Paid = 1,
        Complimentary,
    }
}