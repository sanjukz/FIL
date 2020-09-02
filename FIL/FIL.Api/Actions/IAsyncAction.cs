using System.Threading.Tasks;

namespace FIL.Api.Actions
{
    public interface IAsyncAction<in T> where T : IActionParameters
    {
        Task Execute(T parameters);
    }
}