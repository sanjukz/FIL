using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.TMS
{
    public class AddMatchSponsorCommand : ICommandWithResult<AddMatchSponsorCommandResult>
    {
        public Guid? EventDetailAltId { get; set; }
        public Guid? EventAltId { get; set; }
        public Guid? VenueAltId { get; set; }
        public AllocationType AllocationType { get; set; }
        public long SponsorId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class AddMatchSponsorCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}