using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum EventFrequencyType
    {
        None = 0,
        Single,
        Recurring,
        OnDemand
    }
}