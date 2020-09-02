using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class TicketStockCommand : ICommandWithResult<TicketStockCommandResult>
    {
        public Guid UserAltId { get; set; }
        public string TicketStockStartSrNo { get; set; }
        public string TicketStockEndSrNo { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class TicketStockCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}