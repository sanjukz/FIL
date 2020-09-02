using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Tiqets;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Tiqets
{
    public class GetTimeSlotsCommand : ICommandWithResult<GetTimeSlotsCommandResult>
    {
        public string ProductId { get; set; }
        public string Day { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class GetTimeSlotsCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<Timeslot> TimeSlots { get; set; }
    }
}