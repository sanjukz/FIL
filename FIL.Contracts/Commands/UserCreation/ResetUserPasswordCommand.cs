using System;

namespace FIL.Contracts.Commands.UserCreation
{
    public class ResetUserPasswordCommand : Contracts.Interfaces.Commands.ICommandWithResult<ResetUserPasswordCommandResult>
    {
        public Guid UserAltId { get; set; }
        public Guid ModifiedBy { get; set; }
        public string Password { get; set; }
    }

    public class ResetUserPasswordCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}