using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum RASVTicketType
    {
        None = 0,
        Exhibit,
        Ride,
    }
}