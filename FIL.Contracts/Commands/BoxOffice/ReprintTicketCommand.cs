using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class ReprintTicketCommand : ICommandWithResult<ReprintTicketCommandResult>
    {
        public long TransactionId { get; set; }
        public long MatchSeatTicketDetailId { get; set; }
        public List<string> BarcodeNumbers { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ReprintTicketCommandResult : ICommandResult
    {
        public bool Success { get; set; }
        public long Id { get; set; }
        public List<TicketDetail> TicketDetail { get; set; }
    }
}