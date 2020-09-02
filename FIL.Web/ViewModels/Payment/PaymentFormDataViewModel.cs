using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Payment
{
    public class PaymentFormDataViewModel
    {
        public long TransactionId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }        
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public short? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }
        public CardType? CardTypeId { get; set; }
        public PaymentOptions PaymentOption { get; set; }
        public Guid BankAltId { get; set; }
        public Guid CardAltId { get; set; }
        public string Token { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
    }

}
