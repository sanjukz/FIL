using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.Account
{
    public class UpdateNotificationCommand : ICommandWithResult<UpdateNotificationCommandResult>
    {
        public Guid UserAltId { get; set; }
        public bool? IsMailOpt { get; set; }
        public bool? ShouldUpdate { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class UpdateNotificationCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool IsMailOpt { get; set; }
    }
}