using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class DeleteTicketCommand : Contracts.Interfaces.Commands.ICommandWithResult<DeleteTicketCommandResult>
    {
        public Guid ETDAltId { get; set; }
        public long EventId { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
        public short TicketLength { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class DeleteTicketCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
        public bool Success { get; set; }
        public bool IsTicketSold { get; set; }
    }
}