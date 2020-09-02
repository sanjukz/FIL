using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class FloatCommand : ICommandWithResult<FloatCommandResult>
    {
        public Guid UserAltId { get; set; }
        public Decimal CashInHand { get; set; }
        public Decimal CashInHandLocal { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class FloatCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}