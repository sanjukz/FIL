using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum SignUpMethods
    {
        None = 0,
        Regular = 1,
        Google = 2,
        Facebook = 4,
        LinkedIn = 8,
        OTP = 9
    }
}