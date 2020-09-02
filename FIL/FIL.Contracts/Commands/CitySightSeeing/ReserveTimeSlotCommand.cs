using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CitySightSeeing
{
    public class ReserveTimeSlotCommand : ICommandWithResult<ReserveTimeSlotCommandResult>
    {
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
        public Guid UserAltId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ReserveTimeSlotCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Reservation_reference { get; set; }
        public string Distributor_reference { get; set; }
        public DateTime Reservation_valid_until { get; set; }
        public string FromTime { get; set; }
        public string EndTime { get; set; }
        public string TicketId { get; set; }
        public string TimeSlot { get; set; }
    }
}