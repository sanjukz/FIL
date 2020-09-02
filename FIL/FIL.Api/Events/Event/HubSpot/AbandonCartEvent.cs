using FIL.Contracts.DataModels;
using MediatR;
using System.Collections.Generic;

namespace FIL.Api.Events.Event.HubSpot
{
    public class AbandonCartEvent : INotification
    {
        public IEnumerable<HubspotCartTrack> AbandonCart { get; set; }
    }
}