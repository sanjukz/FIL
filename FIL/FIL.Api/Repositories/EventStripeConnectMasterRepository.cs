using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventStripeConnectMasterRepository : IOrmRepository<EventStripeConnectMaster, EventStripeConnectMaster>
    {
        EventStripeConnectMaster Get(int id);

        EventStripeConnectMaster GetByEventId(long eventId);
    }

    public class EventStripeConnectMasterRepository : BaseOrmRepository<EventStripeConnectMaster>, IEventStripeConnectMasterRepository
    {
        public EventStripeConnectMasterRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventStripeConnectMaster Get(int id)
        {
            return Get(new EventStripeConnectMaster { Id = id });
        }

        public EventStripeConnectMaster GetByEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventStripeConnectMaster.EventId):C} = @Id")
                  .WithParameters(new { Id = eventId })).FirstOrDefault();
        }
    }
}