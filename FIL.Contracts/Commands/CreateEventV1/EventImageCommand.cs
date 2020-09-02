using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class EventImageCommand : Contracts.Interfaces.Commands.ICommandWithResult<EventImageCommandResult>
    {
        public short CurrentStep { get; set; }
        public FIL.Contracts.Models.CreateEventV1.EventImageModel EventImageModel { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class EventImageCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public bool IsValidLink { get; set; }
        public bool IsDraft { get; set; }
        public FIL.Contracts.Models.CreateEventV1.EventImageModel EventImageModel { get; set; }
        public string CompletedStep { get; set; }
        public short CurrentStep { get; set; }
    }
}