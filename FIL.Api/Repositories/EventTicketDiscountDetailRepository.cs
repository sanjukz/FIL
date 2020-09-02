using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventTicketDiscountDetailRepository : IOrmRepository<EventTicketDiscountDetail, EventTicketDiscountDetail>
    {
        EventTicketDiscountDetail Get(int id);

        EventTicketDiscountDetail GetByEventTicketAttributeId(long eventTicketAttributeId);

        IEnumerable<EventTicketDiscountDetail> GetAllByEventTicketAttributeIds(List<long> eventTicketAttributeIds);

        EventTicketDiscountDetail GetByETAId(long eventTicketAttributeId);

        IEnumerable<EventTicketDiscountDetail> GetAllByEventTicketAttributeId(long eventTicketAttributeId);
    }

    public class EventTicketDiscountDetailRepository : BaseOrmRepository<EventTicketDiscountDetail>, IEventTicketDiscountDetailRepository
    {
        public EventTicketDiscountDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventTicketDiscountDetail Get(int id)
        {
            return Get(new EventTicketDiscountDetail { Id = id });
        }

        public IEnumerable<EventTicketDiscountDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventTicketDiscountDetail(EventTicketDiscountDetail eventTicketDiscountDetail)
        {
            Delete(eventTicketDiscountDetail);
        }

        public EventTicketDiscountDetail SaveEventTicketDiscountDetail(EventTicketDiscountDetail eventTicketDiscountDetail)
        {
            return Save(eventTicketDiscountDetail);
        }

        public EventTicketDiscountDetail GetByEventTicketAttributeId(long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDiscountDetail.EventTicketAttributeId):C} = @eventTicketAttribute  AND IsEnabled=1")
                .WithParameters(new { eventTicketAttribute = eventTicketAttributeId })
            ).FirstOrDefault();
        }

        public IEnumerable<EventTicketDiscountDetail> GetAllByEventTicketAttributeId(long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDiscountDetail.EventTicketAttributeId):C} = @eventTicketAttribute")
                .WithParameters(new { eventTicketAttribute = eventTicketAttributeId })
            );
        }

        public EventTicketDiscountDetail GetByETAId(long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDiscountDetail.EventTicketAttributeId):C} = @eventTicketAttribute")
                .WithParameters(new { eventTicketAttribute = eventTicketAttributeId })
            ).FirstOrDefault();
        }

        public IEnumerable<EventTicketDiscountDetail> GetAllByEventTicketAttributeIds(List<long> eventTicketAttributeIds)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDiscountDetail.EventTicketAttributeId):C} IN @eventTicketAttribute")
                .WithParameters(new { eventTicketAttribute = eventTicketAttributeIds })
            );
        }
    }
}