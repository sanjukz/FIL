using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum SiteLevel
    {
        None = 0,

        [SearchTerm("city")]
        City,

        [SearchTerm("state")]
        State,

        [SearchTerm("country")]
        Country,

        Global,
    }
}