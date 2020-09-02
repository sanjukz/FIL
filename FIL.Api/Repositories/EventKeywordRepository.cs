using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventKeywordRepository : IOrmRepository<EventKeyword, EventKeyword>
    {
        EventKeyword Get(int id);
    }

    public class EventKeywordRepository : BaseOrmRepository<EventKeyword>, IEventKeywordRepository
    {
        public EventKeywordRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventKeyword Get(int id)
        {
            return Get(new EventKeyword { Id = id });
        }

        public IEnumerable<EventKeyword> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventKeyword(EventKeyword eventKeyword)
        {
            Delete(eventKeyword);
        }

        public EventKeyword SaveEventKeyword(EventKeyword eventKeyword)
        {
            return Save(eventKeyword);
        }
    }
}