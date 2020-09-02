using FIL.Contracts.Models;
using MediatR;

namespace FIL.Api.Events.Event.HubSpot
{
    public class CartInfoEvent : INotification
    {
        public CartTrack CartDetail { get; }

        public CartInfoEvent(CartTrack cartDetail)
        {
            CartDetail = cartDetail;
        }
    }
}