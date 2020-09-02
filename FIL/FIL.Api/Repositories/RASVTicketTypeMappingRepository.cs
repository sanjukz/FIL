using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IRASVTicketTypeMappingRepository : IOrmRepository<RASVTicketTypeMapping, RASVTicketTypeMapping>
    {
        RASVTicketTypeMapping Get(int id);

        IEnumerable<RASVTicketTypeMapping> GetByEventDetailIds(IEnumerable<long> eventDetailIds);
    }

    public class RASVTicketTypeMappingRepository : BaseLongOrmRepository<RASVTicketTypeMapping>, IRASVTicketTypeMappingRepository
    {
        public RASVTicketTypeMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public RASVTicketTypeMapping Get(int id)
        {
            return Get(new RASVTicketTypeMapping { Id = id });
        }

        public IEnumerable<RASVTicketTypeMapping> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<RASVTicketTypeMapping> GetByEventDetailIds(IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(RASVTicketTypeMapping.EventDetailId):C} IN @EventDetailIds")
            .WithParameters(new { EventDetailIds = eventDetailIds }));
        }
    }
}