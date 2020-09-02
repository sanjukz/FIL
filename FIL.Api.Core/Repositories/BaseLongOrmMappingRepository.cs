using FIL.Api.Core.Utilities;
using FIL.Contracts.Interfaces;

namespace FIL.Api.Core.Repositories
{
    public abstract class BaseLongOrmMappingRepository<T, TV> : BaseOrmMappingRepository<T, TV>
        where T : class
        where TV : class
    {
        protected BaseLongOrmMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        protected override bool CheckForInsert(T obj)
        {
            return ((IId<long>)obj).Id == 0;
        }
    }
}