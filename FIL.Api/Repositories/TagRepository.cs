using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ITagRepository : IOrmRepository<Tags, Tags>
    {
        Tags Get(long id);
    }

    public class TagRepository : BaseLongOrmRepository<Tags>, ITagRepository
    {
        public TagRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public Tags Get(long id)
        {
            return Get(new Tags { Id = id });
        }
    }
}