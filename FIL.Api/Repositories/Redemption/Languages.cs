using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Redemption
{
    public interface ILanguagesRepository : IOrmRepository<Languages, Languages>
    {
        Languages Get(int Id);
    }

    public class LanguagesRepository : BaseOrmRepository<Languages>, ILanguagesRepository
    {
        public LanguagesRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Languages Get(int Id)
        {
            return Get(new Languages { Id = Id });
        }

        public IEnumerable<Languages> GetAll()
        {
            return GetAll(null);
        }
    }
}