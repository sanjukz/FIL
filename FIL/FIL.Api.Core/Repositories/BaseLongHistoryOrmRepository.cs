using FIL.Api.Core.Utilities;
using FIL.Contracts.Interfaces;

namespace FIL.Api.Core.Repositories
{
    public abstract class BaseLongHistoryOrmRepository<T, TR> : BaseOrmHistoryRepository<T, TR>
        where T : class
        where TR : T
    {
        protected BaseLongHistoryOrmRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        protected override bool CheckForInsert(T obj)
        {
            return ((IId<long>)obj).Id == 0;
        }

        protected override T ClearId(T obj)
        {
            if (obj is IId<long>)
            {
                ((IId<long>)obj).Id = 0;
            }
            return obj;
        }
    }
}