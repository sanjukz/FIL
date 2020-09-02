using AutoMapper;
using Dapper.FastCrud;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Interfaces;

namespace FIL.Api.Core.Repositories
{
    public abstract class BaseOrmHistoryRepository<T, TR> : BaseOrmRepository<T>
        where T : class
        where TR : T
    {
        protected BaseOrmHistoryRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public override void Delete(T obj)
        {
            var prev = Get(obj);
            prev = ClearId(prev);
            GetCurrentConnection().Insert(Mapper.Map<TR>(prev), s => s.AttachToTransaction(GetCurrentTransaction()));
            base.Delete(obj);
        }

        public override T Save(T obj)
        {
            if (!CheckForInsert(obj))
            {
                var prev = Get(obj);
                prev = ClearId(prev);
                GetCurrentConnection().Insert(Mapper.Map<TR>(prev), s => s.AttachToTransaction(GetCurrentTransaction()));
            }
            return base.Save(obj);
        }

        protected virtual T ClearId(T obj)
        {
            if (obj is IId<int>)
            {
                ((IId<int>)obj).Id = 0;
            }
            return obj;
        }
    }
}