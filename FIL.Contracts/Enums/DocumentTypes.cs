using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum DocumentTypes
    {
        None = 0,
        Passport,
        DrivingLicence,
        Aadhar,
        VoterID,
    }
}