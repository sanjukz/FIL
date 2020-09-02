using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class EventScheduleCommand : Contracts.Interfaces.Commands.ICommandWithResult<EventScheduleCommandResult>
    {
        public FIL.Contracts.Models.CreateEventV1.EventScheduleModel EventScheduleModel { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public short CurrentStep { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class EventScheduleCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
        public FIL.Contracts.Models.CreateEventV1.EventScheduleModel EventScheduleModel { get; set; }
    }
}