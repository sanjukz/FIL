using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum PaymentType
    {
        BankAccountTransfer = 1,
        Cash,
        CertifiedChequeOrDemandDraft,
        Cheque,
        CreditDebitCard,
        SaleOnCredit,
        PaymentWithClient,
        RetailPrintedToBeSold,
    }
}