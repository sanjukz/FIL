using System.Threading.Tasks;

namespace FIL.Window.Service
{
    public interface IAlgoliaDataSync
    {
        Task Synchronize();
    }
}