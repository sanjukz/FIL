using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum ReportExportStatus
    {
        None = 0,
        Authorized,
        Processing,
        Transferred,
    }
}