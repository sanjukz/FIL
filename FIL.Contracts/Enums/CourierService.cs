using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum CourierService
    {
        None = 0,
        Bluedart = 1,
        FedEx,
        UPS,
        FirstFlight
    }
}