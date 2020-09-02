namespace FIL.Contracts.Interfaces.Commands
{
    public interface ICommandWithResult<T> : ICommand
        where T : ICommandResult
    {
    }
}