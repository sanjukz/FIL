using MediatR;
using System;

namespace FIL.Api.Events.Notifications
{
    public abstract class BaseEvent : INotification
    {
        public Guid ModifiedBy { get; set; }
        public DateTime EffectiveUtc { get; set; }
    }
}