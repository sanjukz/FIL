using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IKeywordRepository : IOrmRepository<Keyword, Keyword>
    {
        Keyword Get(int id);
    }

    public class KeywordRepository : BaseOrmRepository<Keyword>, IKeywordRepository
    {
        public KeywordRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Keyword Get(int id)
        {
            return Get(new Keyword { Id = id });
        }

        public IEnumerable<Keyword> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteKeyword(Keyword keyword)
        {
            Delete(keyword);
        }

        public Keyword SaveKeyword(Keyword keyword)
        {
            return Save(keyword);
        }
    }
}