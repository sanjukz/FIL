using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.Payment
{
    public class PaymentCommand : ICommandWithResult<PaymentCommandResult>
    {
        public long TransactionId { get; set; }
        public Guid UserAltId { get; set; }
        public PaymentCard PaymentCard { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentOptions? PaymentOption { get; set; }
        public Guid BankAltId { get; set; }
        public Guid CardAltId { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
        public Channels? ChannelId { get; set; }
        public string Token { get; set; }
        public string PaymentRedirectUrl { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class PaymentCard
    {
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public string Cvv { get; set; }
        public short ExpiryMonth { get; set; }
        public short ExpiryYear { get; set; }
        public CardType CardType { get; set; }
    }

    public class BillingAddress
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
    }
}