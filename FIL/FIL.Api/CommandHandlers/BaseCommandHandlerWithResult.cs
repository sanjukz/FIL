using MediatR;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace FIL.Api.CommandHandlers
{
    public abstract class BaseCommandHandlerWithResult<T, TR> : ICommandHandlerWithResult<T, TR>
        where T : Contracts.Interfaces.Commands.ICommandWithResult<TR>
        where TR : Contracts.Interfaces.Commands.ICommandResult
    {
        protected readonly IMediator Mediator;

        public virtual TimeSpan TransactionTimeout => TransactionManager.DefaultTimeout;

        protected BaseCommandHandlerWithResult(IMediator mediator)
        {
            Mediator = mediator;
        }

        public Task<Contracts.Interfaces.Commands.ICommandResult> Handle(Contracts.Interfaces.Commands.ICommand command)
        {
            return Handle((T)command);
        }

        protected abstract Task<Contracts.Interfaces.Commands.ICommandResult> Handle(T command);
    }
}