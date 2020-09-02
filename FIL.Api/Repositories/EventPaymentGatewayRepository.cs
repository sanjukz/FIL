using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventPaymentGatewayRepository : IOrmRepository<EventPaymentGateway, EventPaymentGateway>
    {
        EventPaymentGateway Get(int id);
    }

    public class EventPaymentGatewayRepository : BaseOrmRepository<EventPaymentGateway>, IEventPaymentGatewayRepository
    {
        public EventPaymentGatewayRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventPaymentGateway Get(int id)
        {
            return Get(new EventPaymentGateway { Id = id });
        }

        public IEnumerable<EventPaymentGateway> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventPaymentGateway(EventPaymentGateway eventPaymentGateway)
        {
            Delete(eventPaymentGateway);
        }

        public EventPaymentGateway SaveEventPaymentGateway(EventPaymentGateway eventPaymentGateway)
        {
            return Save(eventPaymentGateway);
        }
    }
}