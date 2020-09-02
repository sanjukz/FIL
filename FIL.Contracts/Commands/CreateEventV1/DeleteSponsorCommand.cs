using System;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class DeleteSponsorCommand : Contracts.Interfaces.Commands.ICommandWithResult<DeleteSponsorCommandResult>
    {
        public Guid SponsorAltId { get; set; }
        public short CurrentStep { get; set; }
        public short completedStep { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class DeleteSponsorCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
    }
}