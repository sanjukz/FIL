using System.Threading.Tasks;

namespace Kz.Window.Service
{
    public interface IAlgoliaDataSync
    {
        Task Synchronize();
    }
}