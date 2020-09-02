using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.PaymentChargers;
using System;

namespace FIL.Contracts.Commands.Payment
{
    public class PaymentCommandResult : ICommandResult
    {
        public IPaymentHtmlPostResponse PaymentHtmlPostResponse { get; set; }
        public IPaymentResponse PaymentResponse { get; set; }
        public Guid TransactionAltId { get; set; }
        public long Id { get; set; }
    }
}