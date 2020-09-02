using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventTagMappingRepository : IOrmRepository<eventtagmappings, eventtagmappings>
    {
        eventtagmappings Get(long id);

        IEnumerable<eventtagmappings> GetByEventId(long eventId);
    }

    public class EventTagMappingRepository : BaseLongOrmRepository<eventtagmappings>, IEventTagMappingRepository
    {
        public EventTagMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public eventtagmappings Get(long id)
        {
            return Get(new eventtagmappings { Id = id });
        }

        public IEnumerable<eventtagmappings> GetByEventId(long eventId)
        {
            var eventAmenityList = GetAll(statement => statement
                .Where($"{nameof(eventtagmappings.EventId):C}= @Id")
                .WithParameters(new { Id = eventId }));
            return eventAmenityList;
        }
    }
}