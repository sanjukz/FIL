using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventAmenityRepository : IOrmRepository<EventAmenity, EventAmenity>
    {
        EventAmenity Get(int id);

        IEnumerable<EventAmenity> GetByEventId(long eventId);
    }

    public class EventAmenityRepository : BaseOrmRepository<EventAmenity>, IEventAmenityRepository
    {
        public EventAmenityRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventAmenity Get(int id)
        {
            return Get(new EventAmenity { Id = id });
        }

        public IEnumerable<EventAmenity> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventAmenity(EventAmenity discount)
        {
            Delete(discount);
        }

        public EventAmenity SaveEventAmenity(EventAmenity discount)
        {
            return Save(discount);
        }

        public IEnumerable<EventAmenity> GetByEventId(long eventId)
        {
            var eventAmenityList = GetAll(statement => statement
                .Where($"{nameof(EventAmenity.EventId):C}= @Id")
                .WithParameters(new { Id = eventId }));
            return eventAmenityList;
        }
    }
}