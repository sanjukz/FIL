using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventFacebookReservationRepository : IOrmRepository<EventFacebookReservation, EventFacebookReservation>
    {
        EventFacebookReservation Get(int id);
    }

    public class EventFacebookReservationRepository : BaseLongOrmRepository<EventFacebookReservation>, IEventFacebookReservationRepository
    {
        public EventFacebookReservationRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventFacebookReservation Get(int id)
        {
            return Get(new EventFacebookReservation { Id = id });
        }

        public IEnumerable<EventFacebookReservation> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventFacebookReservation(EventFacebookReservation eventFacebookReservation)
        {
            Delete(eventFacebookReservation);
        }

        public EventFacebookReservation SaveEventFacebookReservation(EventFacebookReservation eventFacebookReservation)
        {
            return Save(eventFacebookReservation);
        }
    }
}