using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum RegionalOrganisation
    {
        None = 0,
        Foreigner,
        Indian,
        SAARC,
        BIMSTEC
    }
}