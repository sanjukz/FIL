using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.CitySightSeeing
{
    public class CreateBookingCommand : ICommandWithResult<CreateBookingCommandResult>
    {
        public long TransactionId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CreateBookingCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public bool IsExists { get; set; }
    }
}