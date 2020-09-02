using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class CreateTicketCommand : Contracts.Interfaces.Commands.ICommandWithResult<CreateTicketCommandResult>
    {
        public long EventId { get; set; }
        public long EventDetailId { get; set; }
        public bool IsCreate { get; set; }
        public short CurrentStep { get; set; }
        public List<FIL.Contracts.Models.CreateEventV1.TicketModel> Tickets { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CreateTicketCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string CompletedStep { get; set; }
        public short CurrentStep { get; set; }
        public long EventId { get; set; }
        public long EventDetailId { get; set; }
        public bool IsCreate { get; set; }
        public List<FIL.Contracts.Models.CreateEventV1.TicketModel> Tickets { get; set; }
    }
}