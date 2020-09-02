namespace FIL.Api.Actions
{
    public interface IAction<in T> where T : IActionParameters
    {
        void Execute(T parameters);
    }
}