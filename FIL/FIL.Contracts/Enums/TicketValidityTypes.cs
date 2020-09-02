using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum TicketValidityTypes
    {
        None = 0,
        Rolling,
        Fixed
    }
}