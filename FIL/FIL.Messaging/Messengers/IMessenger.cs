using FIL.Messaging.Models;
using System.Threading.Tasks;

namespace FIL.Messaging.Messengers
{
    public interface IMessenger<in T>
    {
        Task<IResponse> Send(T request);
    }
}