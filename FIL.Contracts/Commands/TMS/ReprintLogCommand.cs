using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.TMS
{
    public class ReprintLogCommand : ICommandWithResult<ReprintLogCommandResult>
    {
        public long TransactionId { get; set; }
        public string Remarks { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ReprintLogCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}