using System;

namespace FIL.Contracts.Commands.ASI
{
    public class ASIMonumentCommand : Contracts.Interfaces.Commands.ICommandWithResult<ASIMonumentCommandResult>
    {
        public Guid ModifiedBy { get; set; }
    }

    public class ASIMonumentCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}