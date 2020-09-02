using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum PaymentGateway
    {
        None = 0,
        Stripe = 1,
        Paypal,
        Icici,
        CCAvenue,
        Amex,
        HDFC,
        Payfort,
        FreeRegistration,
        HDFCEMI,
        BOB,
        CitrusNetBanking,
        KyazoongaTestCards,
        NabTransact,
        Payu
    }
}