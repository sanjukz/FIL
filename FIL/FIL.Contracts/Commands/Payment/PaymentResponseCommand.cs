using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.Payment
{
    public class PaymentResponseCommand : ICommandWithResult<PaymentResponseCommandResult>
    {
        public long TransactionId { get; set; }
        public PaymentOptions? PaymentOption { get; set; }
        public string QueryString { get; set; }
        public Channels ChannelId { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}