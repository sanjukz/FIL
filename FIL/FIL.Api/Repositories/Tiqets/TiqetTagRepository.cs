using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetTagRepository : IOrmRepository<TiqetTag, TiqetTag>
    {
        TiqetTag Get(long id);

        TiqetTag GetByName(string name);

        TiqetTag GetByTagId(string tagId);

        IEnumerable<TiqetTag> GetByTagIds(IEnumerable<string> tagIds);
    }

    public class TiqetTagRepository : BaseLongOrmRepository<TiqetTag>, ITiqetTagRepository
    {
        public TiqetTagRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TiqetTag Get(long id)
        {
            return Get(new TiqetTag { Id = id });
        }

        public IEnumerable<TiqetTag> GetAll()
        {
            return GetAll(null);
        }

        public TiqetTag GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(TiqetTag.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).LastOrDefault();
        }

        public TiqetTag GetByTagId(string tagId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetTag.TagId):C} = @TagId")
                .WithParameters(new { TagId = tagId })
            ).LastOrDefault();
        }

        public IEnumerable<TiqetTag> GetByTagIds(IEnumerable<string> tagIds)
        {
            return GetAll(s => s.Where($"{nameof(TiqetTag.TagId):C}IN @TagIds")
                 .WithParameters(new { TagIds = tagIds })
             );
        }
    }
}