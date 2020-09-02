using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum PaymentOptions
    {
        None = 0,
        CreditCard = 1,
        DebitCard = 2,
        NetBanking = 4,
        Wallet = 8,
        CashCard = 16
    }
}