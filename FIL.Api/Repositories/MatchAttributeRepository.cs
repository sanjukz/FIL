using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IMatchAttributeRepository : IOrmRepository<MatchAttribute, MatchAttribute>
    {
        MatchAttribute Get(long id);

        IEnumerable<MatchAttribute> GetByEventDetailIds(IEnumerable<long> eventDetailIds);

        IEnumerable<MatchAttribute> GetByEventDetailId(long eventDetailId);
    }

    public class MatchAttributeRepository : BaseLongOrmRepository<MatchAttribute>, IMatchAttributeRepository
    {
        public MatchAttributeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MatchAttribute Get(long id)
        {
            return Get(new MatchAttribute { Id = id });
        }

        public IEnumerable<MatchAttribute> GetByEventDetailIds(IEnumerable<long> eventDetailIds)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(MatchAttribute.EventDetailId):C} IN @Ids")
                    .WithParameters(new { Ids = eventDetailIds }));
        }

        public IEnumerable<MatchAttribute> GetByEventDetailId(long eventDetailId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(MatchAttribute.EventDetailId):C} = @Id")
                    .WithParameters(new { Id = eventDetailId }));
        }
    }
}