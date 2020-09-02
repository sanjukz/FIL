using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum ReportSummaryType
    {
        None = 0,
        CurrencyWise = 1,
        ChannelWise,
        TicketType,
        PaymentModeWise
    }
}