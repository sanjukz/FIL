using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventFeeTypeMappingRepository : IOrmRepository<EventFeeTypeMapping, EventFeeTypeMapping>
    {
        EventFeeTypeMapping Get(long id);

        IEnumerable<EventFeeTypeMapping> GetByEventId(long EventId);
    }

    public class EventFeeTypeMappingRepository : BaseLongOrmRepository<EventFeeTypeMapping>, IEventFeeTypeMappingRepository
    {
        public EventFeeTypeMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventFeeTypeMapping Get(long id)
        {
            return Get(new EventFeeTypeMapping { Id = id });
        }

        public IEnumerable<EventFeeTypeMapping> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<EventFeeTypeMapping> GetByEventId(long EventId)
        {
            return GetAll(s => s.Where($"{nameof(EventFeeTypeMapping.EventId):C} = @EventId")
                  .WithParameters(new { EventId = EventId }));
        }
    }
}