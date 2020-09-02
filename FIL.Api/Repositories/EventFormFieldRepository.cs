using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventFormFieldRepository : IOrmRepository<EventFormField, EventFormField>
    {
        EventFormField Get(long id);
    }

    public class EventFormFieldRepository : BaseOrmRepository<EventFormField>, IEventFormFieldRepository
    {
        public EventFormFieldRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventFormField Get(long id)
        {
            return Get(new EventFormField { Id = id });
        }

        public IEnumerable<EventFormField> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventFormField(EventFormField discount)
        {
            Delete(discount);
        }

        public EventFormField SaveEventFormField(EventFormField discount)
        {
            return Save(discount);
        }
    }
}