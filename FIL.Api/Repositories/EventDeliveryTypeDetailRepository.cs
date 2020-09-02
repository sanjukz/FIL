using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventDeliveryTypeDetailRepository : IOrmRepository<EventDeliveryTypeDetail, EventDeliveryTypeDetail>
    {
        EventDeliveryTypeDetail Get(long id);

        IEnumerable<EventDeliveryTypeDetail> GetByEventDetailIds(IEnumerable<long> eventDetailIds);

        IEnumerable<EventDeliveryTypeDetail> GetAllActivatedByEventDetailIds(IEnumerable<long> eventDetailIds);

        IEnumerable<EventDeliveryTypeDetail> GetByEventDetailId(long eventDetailId);

        EventDeliveryTypeDetail GetByEventDetailIdAndDocumentTypeId(long eventDetailId, FIL.Contracts.Enums.DeliveryTypes deliveyType);
    }

    public class EventDeliveryTypeDetailRepository : BaseLongOrmRepository<EventDeliveryTypeDetail>, IEventDeliveryTypeDetailRepository
    {
        public EventDeliveryTypeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventDeliveryTypeDetail Get(long id)
        {
            return Get(new EventDeliveryTypeDetail { Id = id });
        }

        public IEnumerable<EventDeliveryTypeDetail> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<EventDeliveryTypeDetail> GetByEventDetailIds(IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventDeliveryTypeDetail.EventDetailId):C}IN @EventDetailIds")
                 .WithParameters(new { EventDetailIds = eventDetailIds })
             );
        }

        public IEnumerable<EventDeliveryTypeDetail> GetByEventDetailId(long eventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(EventDeliveryTypeDetail.EventDetailId):C}  = @EventDetailIds")
                 .WithParameters(new { EventDetailIds = eventDetailId })
             );
        }

        public void DeleteEventDeliveryTypeDetail(EventDeliveryTypeDetail eventDeliveryTypeDetail)
        {
            Delete(eventDeliveryTypeDetail);
        }

        public EventDeliveryTypeDetail SaveEventDeliveryTypeDetail(EventDeliveryTypeDetail eventDeliveryTypeDetail)
        {
            return Save(eventDeliveryTypeDetail);
        }

        public EventDeliveryTypeDetail GetByEventDetailIdAndDocumentTypeId(long eventDetailId, FIL.Contracts.Enums.DeliveryTypes deliveyType)
        {
            return GetAll(s => s.Where($"{nameof(EventDeliveryTypeDetail.EventDetailId):C} = @EventDetailId AND  {nameof(EventDeliveryTypeDetail.DeliveryTypeId):C} = @DeliveyType")
                .WithParameters(new { EventDetailId = eventDetailId, DeliveyType = deliveyType })
            ).FirstOrDefault();
        }

        public IEnumerable<EventDeliveryTypeDetail> GetAllActivatedByEventDetailIds(IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventDeliveryTypeDetail.EventDetailId):C}IN @EventDetailIds and IsEnabled = 1")
                 .WithParameters(new { EventDetailIds = eventDetailIds })
             );
        }
    }
}