using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventAttendeeDetailRepository : IOrmRepository<EventAttendeeDetail, EventAttendeeDetail>
    {
        EventAttendeeDetail Get(long id);
    }

    public class EventAttendeeDetailRepository : BaseLongOrmRepository<EventAttendeeDetail>, IEventAttendeeDetailRepository
    {
        public EventAttendeeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventAttendeeDetail Get(long id)
        {
            return Get(new EventAttendeeDetail { Id = id });
        }

        public IEnumerable<EventAttendeeDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteUserRegistrationDetail(EventAttendeeDetail discount)
        {
            Delete(discount);
        }

        public EventAttendeeDetail SaveUserRegistrationDetail(EventAttendeeDetail discount)
        {
            return Save(discount);
        }
    }
}