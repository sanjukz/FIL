using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum MasterEventType
    {
        None = 1,
        Offline,
        InRealLife,
        Online
    }
}