using System.Threading.Tasks;

namespace Kz.Window.Service
{
    public interface ITiqetsDataSync
    {
        Task Synchronize();
    }
}