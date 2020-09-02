using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.PaymentChargers;
using System;

namespace FIL.Contracts.Commands.Payment
{
    public class PaymentResponseCommandResult : ICommandResult
    {
        public IPaymentResponse PaymentResponse { get; set; }
        public long Id { get; set; }
        public Guid TransactionAltId { get; set; }
    }
}