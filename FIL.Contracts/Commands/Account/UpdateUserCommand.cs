using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.Account
{
    public class UpdateUserCommand : ICommandWithResult<UpdateUserCommandResult>
    {
        public FIL.Contracts.Models.UserProfile UserProfile { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class UpdateUserCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public FIL.Contracts.Models.UserProfile UserProfile { get; set; }
    }
}