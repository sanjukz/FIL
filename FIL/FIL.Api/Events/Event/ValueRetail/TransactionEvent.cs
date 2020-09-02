using MediatR;

namespace FIL.Api.Events.Event.ValueRetail
{
    public class TransactionEvent : INotification
    {
        //Needs to be added property for Value Retail
        public string TransactionStatus { get; set; }

        public string EmailId { get; set; }
        public string ZipCode { get; set; }
        public long TransactionId { get; set; }
    }
}