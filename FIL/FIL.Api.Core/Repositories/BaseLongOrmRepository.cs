using FIL.Api.Core.Utilities;
using System.Collections.Generic;

namespace FIL.Api.Core.Repositories
{
    public abstract class BaseLongOrmRepository<T> : BaseLongOrmMappingRepository<T, T>
        where T : class
    {
        protected BaseLongOrmRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        protected override T ToDto(T obj)
        {
            return obj;
        }

        protected override IEnumerable<T> ToDto(IEnumerable<T> obj)
        {
            return obj;
        }

        protected override T FromDto(T obj)
        {
            return obj;
        }

        protected override IEnumerable<T> FromDto(IEnumerable<T> obj)
        {
            return obj;
        }
    }
}