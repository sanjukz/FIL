using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class DeleteHostCommand : Contracts.Interfaces.Commands.ICommandWithResult<DeleteHostCommandResult>
    {
        public Guid EventHostAltId { get; set; }
        public short CurrentStep { get; set; }
        public short TicketLength { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class DeleteHostCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
        public bool IsHostStreamLinkCreated { get; set; }
    }
}