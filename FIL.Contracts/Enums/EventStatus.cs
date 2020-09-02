using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum EventStatus
    {
        None = 1,
        Pending,
        Draft,
        WaitingForApproval,
        Reject,
        Published
    }
}