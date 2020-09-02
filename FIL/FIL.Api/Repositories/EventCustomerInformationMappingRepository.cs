using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventCustomerInformationMappingRepository : IOrmRepository<EventCustomerInformationMapping, EventCustomerInformationMapping>
    {
        EventCustomerInformationMapping Get(long id);

        IEnumerable<EventCustomerInformationMapping> GetAllByEventID(long eventId);
    }

    public class EventCustomerInformationMappingRepository : BaseLongOrmRepository<EventCustomerInformationMapping>, IEventCustomerInformationMappingRepository
    {
        public EventCustomerInformationMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventCustomerInformationMapping Get(long id)
        {
            return Get(new EventCustomerInformationMapping { Id = id });
        }

        public IEnumerable<EventCustomerInformationMapping> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<EventCustomerInformationMapping> GetAllByEventID(long eventId)
        {
            var customerInformationMappings = GetAll(statement => statement
                .Where($"{nameof(EventCustomerInformationMapping.EventId):C} = @Id")
                .WithParameters(new { Id = eventId }));
            return customerInformationMappings;
        }
    }
}