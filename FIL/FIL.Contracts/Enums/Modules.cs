using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum Modules
    {
        Boxoffice = 1,
        Retail,
        KITMS,
        Reporting,
        ConsumerWebsite,
        KzSuite,
        FeelAdmin
    }
}