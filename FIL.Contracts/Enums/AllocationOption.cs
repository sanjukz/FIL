using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum AllocationOption
    {
        Block = 1,
        Release,
        Transfer,
        Kill
    }
}