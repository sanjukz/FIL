using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.Zoom
{
    public class ZoomDeActivateUserCommand : ICommandWithResult<ZoomDeActivateUserCommandResult>
    {
        public Guid AltId { get; set; }
        public string UniqueId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ZoomDeActivateUserCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool success { get; set; }
    }
}