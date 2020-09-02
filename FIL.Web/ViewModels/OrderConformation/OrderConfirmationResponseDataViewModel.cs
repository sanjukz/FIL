using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Web.Feel.ViewModels.OrderConformation
{
    public class OrderConfirmationResponseDataViewModel
    {
        public string TransactionQrcode { get; set; }
        public FIL.Contracts.Models.Transaction Transaction { get; set; }
        public TransactionPaymentDetail TransactionPaymentDetail { get; set; }
        public UserCardDetail UserCardDetail { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string PaymentOption { get; set; }
        public string cardTypes { get; set; }
        public List<OrderConfirmationSubContainer> orderConfirmationSubContainer { get; set; }
        public string StreamLink { get; set; }
    }
}
