using MediatR;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace FIL.Api.CommandHandlers
{
    public abstract class BaseCommandHandler<T> : ICommandHandler<T> where T : Contracts.Interfaces.Commands.ICommand
    {
        protected readonly IMediator Mediator;

        public virtual TimeSpan TransactionTimeout => TransactionManager.DefaultTimeout;

        protected BaseCommandHandler(IMediator mediator)
        {
            Mediator = mediator;
        }

        public Task Handle(Contracts.Interfaces.Commands.ICommand command)
        {
            return Handle((T)command);
        }

        protected abstract Task Handle(T command);
    }
}