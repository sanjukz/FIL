using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers
{
    public interface ICommandHandler<in T> where T : Contracts.Interfaces.Commands.ICommand
    {
        Task Handle(Contracts.Interfaces.Commands.ICommand command);

        TimeSpan TransactionTimeout { get; }
    }
}