using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IPathTypeRepository : IOrmRepository<PathType, PathType>
    {
        PathType Get(int id);
    }

    public class PathTypeRepository : BaseOrmRepository<PathType>, IPathTypeRepository
    {
        public PathTypeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public PathType Get(int id)
        {
            return Get(new PathType { Id = id });
        }

        public IEnumerable<PathType> GetAll()
        {
            return GetAll(null);
        }
    }
}