namespace FIL.Api.Actions
{
    public interface IActionWithResult<in TP, out TR> where TP : IActionParameters
    {
        TR Execute(TP parameters);
    }
}