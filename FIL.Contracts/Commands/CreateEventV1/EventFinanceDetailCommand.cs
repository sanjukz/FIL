using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class EventFinanceDetailCommand : Contracts.Interfaces.Commands.ICommandWithResult<EventFinanceDetailCommandResult>
    {
        public FIL.Contracts.Models.CreateEventV1.EventFinanceDetailModel EventFinanceDetailModel { get; set; }
        public long EventId { get; set; }
        public bool Success { get; set; }
        public short CurrentStep { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class EventFinanceDetailCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
        public Guid EventAltId { get; set; }
        public bool Success { get; set; }
        public Guid ModifiedBy { get; set; }
        public FIL.Contracts.Models.CreateEventV1.EventFinanceDetailModel EventFinanceDetailModel { get; set; }
    }
}