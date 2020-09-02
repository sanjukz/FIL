using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    //[Flags]
    public enum FeeType
    {
        [FeeTypeData("None")]
        None = 0,

        [FeeTypeData("Convenience Charge")]
        ConvenienceCharge,

        [FeeTypeData("GST")]
        ServiceCharge,

        [FeeTypeData("SMS")]
        SMSCharge,

        [FeeTypeData("Print-at-Home")]
        PrintAtHomeCharge,

        [FeeTypeData("Bank")]
        BankCharge,

        [FeeTypeData("M-Ticket")]
        MTicketCharge,

        [FeeTypeData("Service")]
        OtherCharge,

        [FeeTypeData("Transaction Fee")]
        TransactionFee,

        [FeeTypeData("Credit Card Surcharge")]
        CreditCardSurcharge,

        [FeeTypeData("Shipping Charge")]
        ShippingCharge
    }
}