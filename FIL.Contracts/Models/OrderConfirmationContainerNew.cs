using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class OrderConfirmationContainerNew
    {
        public Transaction Transaction { get; set; }
        public TransactionPaymentDetail TransactionPaymentDetail { get; set; }
        public UserCardDetail UserCardDetail { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public IEnumerable<OrderConfirmationSubContainer> orderConfirmationSubContainer { get; set; }
    }
}