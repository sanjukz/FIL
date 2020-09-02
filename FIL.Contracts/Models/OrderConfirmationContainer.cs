using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class OrderConfirmationContainer
    {
        public Transaction Transaction { get; set; }
        public List<TransactionDetail> TransactionDetail { get; set; }
        public List<EventTicketDetail> EventTicketDetail { get; set; }
        public List<TicketCategory> TicketCategory { get; set; }
        public List<EventDetail> EventDetail { get; set; }
        public List<Event> Event { get; set; }
        public List<TransactionDeliveryDetail> TransactionDeliveryDetail { get; set; }
        public List<TransactionSeatDetail> TransactionSeatDetail { get; set; }
        public List<TransactionPaymentDetail> TransactionPaymentDetail { get; set; }
        public List<CurrencyType> CurrencyType { get; set; }
        public List<EventDeliveryTypeDetail> EventDeliveryTypeDetail { get; set; }
    }
}