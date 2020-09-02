using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CitySightSeeing
{
    public class GetAvailabilityCommand : ICommandWithResult<GetAvailabilityCommandResult>
    {
        public string Date { get; set; }
        public string TicketId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class GetAvailabilityCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<string> AvailableSlots { get; set; }
    }
}