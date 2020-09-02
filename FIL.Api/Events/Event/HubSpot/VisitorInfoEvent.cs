using FIL.Contracts.DataModels;
using MediatR;

namespace FIL.Api.Events.Event.HubSpot
{
    public class VisitorInfoEvent : INotification
    {
        public User User { get; }

        public VisitorInfoEvent(User userData)
        {
            User = userData;
        }
    }
}