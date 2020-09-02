using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum OccuranceType
    {
        Once = 1,
        Daily,
        Weekly,
        Monthly
    }
}