using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.Tiqets
{
    public class ConfirmOrderCommand : ICommandWithResult<ConfirmOrderCommandResult>
    {
        public long TransactionId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ConfirmOrderCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public string TicketPdfLink { get; set; }
        public bool Success { get; set; }
    }
}