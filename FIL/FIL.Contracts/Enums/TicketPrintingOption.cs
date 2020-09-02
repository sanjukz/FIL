using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum TicketPrintingOption
    {
        NoPrice = 1,
        Price,
        Complimentary,
        ComplimentaryZeroPrice,
        ZeroPrice,
        Hospitality,
        GovernmentComplimentary,
        ChildComplimentary,
        ChildPrice
    }
}