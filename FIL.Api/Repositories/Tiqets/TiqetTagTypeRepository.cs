using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetTagTypeRepository : IOrmRepository<TiqetTagType, TiqetTagType>
    {
        TiqetTagType Get(long id);

        TiqetTagType GetByName(string name);

        IEnumerable<TiqetTagType> GetByNames(IEnumerable<string> names);
    }

    public class TiqetTagTypeRepository : BaseLongOrmRepository<TiqetTagType>, ITiqetTagTypeRepository
    {
        public TiqetTagTypeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TiqetTagType Get(long id)
        {
            return Get(new TiqetTagType { Id = id });
        }

        public IEnumerable<TiqetTagType> GetAll()
        {
            return GetAll(null);
        }

        public TiqetTagType GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(TiqetTagType.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).LastOrDefault();
        }

        public IEnumerable<TiqetTagType> GetByNames(IEnumerable<string> names)
        {
            return GetAll(s => s.Where($"{nameof(TiqetTagType.Name):C}IN @Names")
                 .WithParameters(new { Names = names })
             );
        }
    }
}