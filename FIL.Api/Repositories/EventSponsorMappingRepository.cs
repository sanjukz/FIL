using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IEventSponsorMappingRepository : IOrmRepository<EventSponsorMapping, EventSponsorMapping>
    {
        EventSponsorMapping Get(long id);

        IEnumerable<EventSponsorMapping> GetByEventDetailId(long eventDetailId);

        IEnumerable<EventSponsorMapping> GetByEventDetailIdandSponsorId(long eventDetailId, long sponsorId);

        IEnumerable<EventSponsorMapping> GetByEventDetailIds(IEnumerable<long> eventDetailIds);
    }

    public class EventSponsorMappingRepository : BaseLongOrmRepository<EventSponsorMapping>, IEventSponsorMappingRepository
    {
        public EventSponsorMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventSponsorMapping Get(long id)
        {
            return Get(new EventSponsorMapping { Id = id });
        }

        public IEnumerable<EventSponsorMapping> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<EventSponsorMapping> GetByEventDetailId(long eventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(EventSponsorMapping.EventDetailId):c} = @EventDetailId")
            .WithParameters(new { EventDetailId = eventDetailId }));
        }

        public IEnumerable<EventSponsorMapping> GetByEventDetailIdandSponsorId(long eventDetailId, long sponsorId)
        {
            return GetAll(s => s.Where($"{nameof(EventSponsorMapping.EventDetailId):c} = @EventDetailId AND {nameof(EventSponsorMapping.SponsorId):C} = @SponsorId ")
            .WithParameters(new { EventDetailId = eventDetailId, SponsorId = sponsorId }));
        }

        public IEnumerable<EventSponsorMapping> GetByEventDetailIds(IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventSponsorMapping.EventDetailId):c} IN @EventDetailIds")
            .WithParameters(new { EventDetailIds = eventDetailIds }));
        }
    }
}