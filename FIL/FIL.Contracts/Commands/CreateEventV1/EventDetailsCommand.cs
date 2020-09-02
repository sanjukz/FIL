using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class EventDetailsCommand : Contracts.Interfaces.Commands.ICommandWithResult<EventDetailsCommandResult>
    {
        public FIL.Contracts.Models.CreateEventV1.EventDetailModel EventDetail { get; set; }
        public short CurrentStep { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class EventDetailsCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string CompletedStep { get; set; }
        public short CurrentStep { get; set; }
        public FIL.Contracts.Models.CreateEventV1.EventDetailModel EventDetail { get; set; }
    }
}