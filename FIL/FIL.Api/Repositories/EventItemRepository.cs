using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventItemRepository : IOrmRepository<EventItem, EventItem>
    {
        EventItem Get(int id);
    }

    public class EventItemRepository : BaseOrmRepository<EventItem>, IEventItemRepository
    {
        public EventItemRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventItem Get(int id)
        {
            return Get(new EventItem { Id = id });
        }

        public IEnumerable<EventItem> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventData(EventItem eventItem)
        {
            Delete(eventItem);
        }

        public EventItem SaveEventData(EventItem eventItem)
        {
            return Save(eventItem);
        }
    }
}