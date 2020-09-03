using System.Threading.Tasks;

namespace FIL.Window.Service
{
    public interface ITiqetsDataSync
    {
        Task Synchronize();
    }
}