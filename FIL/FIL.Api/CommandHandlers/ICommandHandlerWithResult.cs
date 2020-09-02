using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers
{
    public interface ICommandHandlerWithResult<in T, out TR>
        where T : Contracts.Interfaces.Commands.ICommandWithResult<TR>
        where TR : Contracts.Interfaces.Commands.ICommandResult
    {
        Task<Contracts.Interfaces.Commands.ICommandResult> Handle(Contracts.Interfaces.Commands.ICommand command);

        TimeSpan TransactionTimeout { get; }
    }
}