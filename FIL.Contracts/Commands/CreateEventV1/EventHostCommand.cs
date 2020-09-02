using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class EventHostCommand : Contracts.Interfaces.Commands.ICommandWithResult<EventHostCommandResult>
    {
        public long EventId { get; set; }
        public bool IsCreate { get; set; }
        public short CurrentStep { get; set; }
        public List<FIL.Contracts.DataModels.EventHostMapping> EventHostMappings { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class EventHostCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
        public List<FIL.Contracts.DataModels.EventHostMapping> EventHostMappings { get; set; }
    }
}