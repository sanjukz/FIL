using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface ICcavenueCharge : ICharge
    {
        IPaymentCard PaymentCard { get; set; }
        PaymentOptions PaymentOption { get; set; }
        IBillingAddress BillingAddress { get; set; }
        Guid BankAltId { get; set; }
        Guid CardAltId { get; set; }
    }

    public class CcavenueCharge : Charge, ICcavenueCharge
    {
        public IPaymentCard PaymentCard { get; set; }
        public PaymentOptions PaymentOption { get; set; }
        public IBillingAddress BillingAddress { get; set; }
        public Guid BankAltId { get; set; }
        public Guid CardAltId { get; set; }
    }
}