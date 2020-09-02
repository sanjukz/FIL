using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class EventStepCommand : Contracts.Interfaces.Commands.ICommandWithResult<EventStepCommandResult>
    {
        public long EventId { get; set; }
        public short CurrentStep { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class EventStepCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string CompletedStep { get; set; }
        public short CurrentStep { get; set; }
    }
}