using FIL.Contracts.DataModels;
using MediatR;

namespace FIL.Api.Events.Event.HubSpot
{
    public class LoginEvent : INotification
    {
        public User User { get; set; }
    }
}