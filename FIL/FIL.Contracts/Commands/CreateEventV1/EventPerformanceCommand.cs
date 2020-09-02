using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class EventPerformanceCommand : Contracts.Interfaces.Commands.ICommandWithResult<EventPerformanceCommandResult>
    {
        public long EventId { get; set; }
        public short CurrentStep { get; set; }
        public FIL.Contracts.Models.CreateEventV1.PerformanceTypeModel PerformanceTypeModel { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class EventPerformanceCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public long EventId { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
        public string OnlineEventType { get; set; }
        public System.Guid EventAltId { get; set; }
        public FIL.Contracts.Models.CreateEventV1.PerformanceTypeModel PerformanceTypeModel { get; set; }
    }
}