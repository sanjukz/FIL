using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventHostMappingRepository : IOrmRepository<EventHostMapping, EventHostMapping>
    {
        EventHostMapping Get(int id);

        IEnumerable<EventHostMapping> GetAllByEventId(long eventId);

        EventHostMapping GetByAltId(Guid altId);

        EventHostMapping GetLatestByEmail(string email);
    }

    public class EventHostMappingRepository : BaseOrmRepository<EventHostMapping>, IEventHostMappingRepository
    {
        public EventHostMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventHostMapping Get(int id)
        {
            return Get(new EventHostMapping { Id = id });
        }

        public IEnumerable<EventHostMapping> GetAllByEventId(long eventId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventHostMapping.EventId):C} = @EventIdParam")
                    .WithParameters(new { EventIdParam = eventId }));
        }

        public EventHostMapping GetByAltId(Guid altId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventHostMapping.AltId):C} = @EventAltId")
                    .WithParameters(new { EventAltId = altId })).FirstOrDefault();
        }

        public EventHostMapping GetLatestByEmail(string email)
        {
            var result = GetAll(statement => statement
                   .Where($"{nameof(EventHostMapping.Email):C} = @EmailParam")
                   .WithParameters(new { EmailParam = email }));

            return result.OrderByDescending(o => o.CreatedUtc).FirstOrDefault();
        }
    }
}