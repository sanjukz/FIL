using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum TransactingOption
    {
        Paid = 1,
        Complimentary,
        PrintedToBeSold
    }
}