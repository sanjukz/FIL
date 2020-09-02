using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum TransactionType
    {
        None = 0,
        Regular,
        Season,
        Itinerary,
        ASI,
        ASIQR,
        QRCode,
        LiveOnline,
        AddOns
    }
}