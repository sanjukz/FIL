using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class TicketPickupCommand : ICommandWithResult<TicketPickupCommandResult>
    {
        public long TransactionId { get; set; }
        public long TransactionDeliveryDetailId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class TicketPickupCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}