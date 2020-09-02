using FIL.Contracts.Attributes;
using System;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    [Flags]
    public enum TransactionStatus
    {
        None = 0,
        Blocked = 1,
        UnderPayment = 2,
        PaymentReceived = 4,
        Success = 8,
        Reverted = 16,
        RevertedFromReport = 32
    }
}