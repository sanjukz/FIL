using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class ClosingStockCommand : ICommandWithResult<ClosingStockCommandResult>
    {
        public Guid UserAltId { get; set; }
        public string TicketStockStartSrNo { get; set; }
        public int WasteTickets { get; set; }
        public decimal CashAmount { get; set; }
        public decimal CardAmount { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ClosingStockCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}