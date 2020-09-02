using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventTokenMappingRepository : IOrmRepository<EventTokenMapping, EventTokenMapping>
    {
        EventTokenMapping Get(int id);

        EventTokenMapping GetByTokenId(int tokenId);
    }

    public class EventTokenMappingRepository : BaseOrmRepository<EventTokenMapping>, IEventTokenMappingRepository
    {
        public EventTokenMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventTokenMapping Get(int id)
        {
            return Get(new EventTokenMapping { Id = id });
        }

        public IEnumerable<EventTokenMapping> GetAll()
        {
            return GetAll(null);
        }

        public EventTokenMapping GetByTokenId(int tokenId)
        {
            return GetAll(s => s.Where($"{nameof(EventTokenMapping.TokenId):C} = @TokenId")
                .WithParameters(new { TokenId = tokenId })
            ).FirstOrDefault();
        }
    }
}