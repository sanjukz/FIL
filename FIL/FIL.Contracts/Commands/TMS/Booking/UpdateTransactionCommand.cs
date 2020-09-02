using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.TMS.Booking
{
    public class UpdateTransactionCommand : ICommandWithResult<UpdateTransactionCommandResult>
    {
        public long TransactionId { get; set; }
        public AllocationType AllocationType { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class UpdateTransactionCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}