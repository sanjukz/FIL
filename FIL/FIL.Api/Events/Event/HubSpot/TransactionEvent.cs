using FIL.Contracts.Enums;
using MediatR;

namespace FIL.Api.Events.Event.HubSpot
{
    public class TransactionEvent : INotification
    {
        public TransactionStatus TransactionStatus { get; set; }
        public string EmailId { get; set; }
        public string ZipCode { get; set; }
        public long TransactionId { get; set; }
        // Not required as we can get it from using TransactionId
        // public Guid UserAltId { get; set; }
    }
}