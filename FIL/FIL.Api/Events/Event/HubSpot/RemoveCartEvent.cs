using FIL.Contracts.Models;
using MediatR;

namespace FIL.Api.Events.Event.HubSpot
{
    public class RemoveCartEvent : INotification
    {
        public CartTrack CartDetail { get; }

        public RemoveCartEvent(CartTrack cartDetail)
        {
            CartDetail = cartDetail;
        }
    }
}