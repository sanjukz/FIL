using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TMS.Booking
{
    public class HandoverDetailsCommand : ICommandWithResult<HandoverDetailsCommandResult>
    {
        public long TransactionId { get; set; }
        public List<TicketDetail> TicketDetails { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class TicketDetail
    {
        public string SerialStart { get; set; }
        public string SerialEnd { get; set; }
        public int Quantity { get; set; }
        public string TicketHandedTo { get; set; }
    }

    public class HandoverDetailsCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}