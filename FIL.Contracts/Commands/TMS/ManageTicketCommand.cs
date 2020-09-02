using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.TMS;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TMS
{
    public class ManageTicketCommand : ICommandWithResult<ManageTicketCommandResult>
    {
        public long? CorporateTicketAllocationDetailId { get; set; }
        public long? TicketCategoryId { get; set; }
        public long? EventTicketAttributeId { get; set; }
        public List<long> EventDetailIds { get; set; }
        public long SponsorId { get; set; }
        public long TransfertoSponsorId { get; set; }
        public int Quantity { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
        public AllocationOption AllocationOptionId { get; set; }
        public AllocationType AllocationType { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ManageTicketCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public long EventTicketAttributeId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool ConfirmCheckEnable { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
    }
}