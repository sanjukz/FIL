using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventStripeAccountMappingRepository : IOrmRepository<EventStripeAccountMapping, EventStripeAccountMapping>
    {
        EventStripeAccountMapping Get(int id);

        EventStripeAccountMapping GetByEventId(long Id);
    }

    public class EventStripeAccountMappingRepository : BaseOrmRepository<EventStripeAccountMapping>, IEventStripeAccountMappingRepository
    {
        public EventStripeAccountMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventStripeAccountMapping Get(int id)
        {
            return Get(new EventStripeAccountMapping { Id = id });
        }

        public IEnumerable<EventStripeAccountMapping> GetAll()
        {
            return GetAll(null);
        }

        public EventStripeAccountMapping GetByEventId(long Id)
        {
            return GetAll(s => s.Where($"{nameof(EventStripeAccountMapping.EventId):C} = @EventId")
               .WithParameters(new { EventId = Id })
           ).FirstOrDefault();
        }
    }
}