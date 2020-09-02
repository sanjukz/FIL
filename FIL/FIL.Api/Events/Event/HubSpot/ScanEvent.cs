using MediatR;
using System;

namespace FIL.Api.Events.Event.HubSpot
{
    public class ScanEvent : INotification
    {
        public long TransactionId { get; set; }
        public DateTime ScanDateTime { get; set; }
    }
}