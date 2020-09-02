using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.TMS
{
    public class AddCategorySponsorCommand : ICommandWithResult<AddCategorySponsorCommandResult>
    {
        public long? EventTicketAttributeId { get; set; }
        public long TicketCategoryId { get; set; }
        public Guid? EventAltId { get; set; }
        public Guid? VenueAltId { get; set; }
        public long SponsorId { get; set; }
        public Guid ModifiedBy { get; set; }
        public AllocationType AllocationType { get; set; }
    }

    public class AddCategorySponsorCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}