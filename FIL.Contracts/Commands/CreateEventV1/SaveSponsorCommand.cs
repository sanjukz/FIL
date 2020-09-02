using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CreateEventV1
{
    public class SaveSponsorCommand : Contracts.Interfaces.Commands.ICommandWithResult<SaveSponsorCommandResult>
    {
        public long EventId { get; set; }
        public bool IsCreate { get; set; }
        public short CurrentStep { get; set; }
        public List<FIL.Contracts.DataModels.FILSponsorDetail> SponsorDetail { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveSponsorCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public short CurrentStep { get; set; }
        public string CompletedStep { get; set; }
        public List<FIL.Contracts.DataModels.FILSponsorDetail> SponsorDetail { get; set; }
    }
}