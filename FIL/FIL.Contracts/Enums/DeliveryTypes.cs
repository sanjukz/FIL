using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    //[Flags]
    public enum DeliveryTypes
    {
        None = 0,
        Courier,
        VenuePickup,
        PrintAtHome,
        MTicket,
        ETicket,
        HotelDelivery
    }
}