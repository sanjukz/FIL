using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum ReportFrequencyType
    {
        Daily = 1,
        Weekly,
        Monthly
    }
}