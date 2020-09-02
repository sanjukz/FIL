using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum EventType
    {
        Regular = 1,
        Perennial,
        Tournament,
        Registration,
        FreeRegistration,
        TicketAlert
    }
}