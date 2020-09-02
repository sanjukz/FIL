using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FIL.ExOzConsoleApp
{
    public interface ISynchronizer
    {
        Task Synchronize();
    }

    public interface ISynchronizer<T, in TR>
    {
        Task<T> Synchronize(TR obj);
    }
}
