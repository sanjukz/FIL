using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum StripeAccount
    {
        None = 0,
        StripeUSA,
        StripeAustralia,
        StripeIndia,
        StripeUk,
        StripeSingapore,
    }
}