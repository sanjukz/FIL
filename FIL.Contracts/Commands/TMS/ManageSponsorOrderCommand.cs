using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.TMS;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TMS
{
    public class ManageSponsorOrderCommand : ICommandWithResult<ManageSponsorOrderCommandResult>
    {
        public long OrderId { get; set; }
        public long? SponsorId { get; set; }
        public int Quantity { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
        public AllocationOption AllocationOption { get; set; }
        public bool IsApprove { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ManageSponsorOrderCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
    }
}