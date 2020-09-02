using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventTicketDetailRepository : IOrmRepository<EventTicketDetail, EventTicketDetail>
    {
        EventTicketDetail Get(long id);

        IEnumerable<EventTicketDetail> GetByEventDetailId(long eventDetailId);

        IEnumerable<EventTicketDetail> GetBOByEventDetailId(long eventDetailId);

        IEnumerable<EventTicketDetail> GetByEventTicketDetailIds(IEnumerable<long> ids);

        IEnumerable<EventTicketDetail> GetByEventTicketDetailsIds(IEnumerable<long> ids);

        IEnumerable<EventTicketDetail> GetByEventDetailIds(IEnumerable<long> ids);

        IEnumerable<EventTicketDetail> GetAllByEventTicketDetailIds(IEnumerable<long> ids);

        IEnumerable<EventTicketDetail> GetAllByEventDetailIds(IEnumerable<long> ids);

        IEnumerable<EventTicketDetail> GetByTicketCategoryIdAndeventDetailId(long ticketCategoryId, long eventDetailId);

        IEnumerable<EventTicketDetail> GetByIds(IEnumerable<long> ids);

        IEnumerable<EventTicketDetail> GetByEventDetailIdsAndIds(IEnumerable<long> eventDetailids, IEnumerable<long> ids);

        IEnumerable<EventTicketDetail> GetRASVRideEventTicketDetails();

        IEnumerable<EventTicketDetail> GetByTicketCategoryIdAndeventDetailIds(long ticketCategoryId, IEnumerable<long> eventDetailIds);

        IEnumerable<EventTicketDetail> GetByBOTicketCategoryIdAndeventDetailIds(long ticketCategoryId, IEnumerable<long> eventDetailIds);

        EventTicketDetail GetByTicketCategoryIdAndEventTicketDetailId(long ticketCategoryId, long eventTicketDetailId);

        EventTicketDetail GetBySingleEventDetailId(long eventId);

        EventTicketDetail GetByAltId(Guid AltId);

        IEnumerable<EventTicketDetail> GetEventTicketDetailByTicketIdAndcitySightSeeingTicketTypeDetailId(string ticketId, int citySightSeeingTicketTypeDetailId);

        EventTicketDetail GetByTicketCategoryIdAndEventDetailId(long ticketCategoryId, long eventDetailId);

        EventTicketDetail GetAllByTicketCategoryIdAndEventDetailId(long ticketCategoryId, long eventDetailId);

        IEnumerable<FIL.Contracts.Models.TMS.SponsorRequestTicketCategory> GetSponsorRequestTicketCategories(long eventDetailId);
    }

    public class EventTicketDetailRepository : BaseLongOrmRepository<EventTicketDetail>, IEventTicketDetailRepository
    {
        public EventTicketDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventTicketDetail Get(long id)
        {
            return Get(new EventTicketDetail { Id = id });
        }

        public IEnumerable<EventTicketDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventTicketDetail(EventTicketDetail eventTicketDetail)
        {
            Delete(eventTicketDetail);
        }

        public EventTicketDetail SaveEventTicketDetail(EventTicketDetail eventTicketDetail)
        {
            return Save(eventTicketDetail);
        }

        public IEnumerable<EventTicketDetail> GetByEventDetailId(long eventDetailId)
        {
            var eventTicketDetailList = GetAll(statement => statement
                .Where($"{nameof(EventTicketDetail.EventDetailId):C} = @Id")
                .WithParameters(new { Id = eventDetailId }));
            return eventTicketDetailList;
        }

        public IEnumerable<EventTicketDetail> GetBOByEventDetailId(long eventDetailId)
        {
            var eventTicketDetailList = GetAll(statement => statement
                .Where($"{nameof(EventTicketDetail.EventDetailId):C} = @Id AND ISNULL(IsBOEnabled,0)=1")
                .WithParameters(new { Id = eventDetailId }));
            return eventTicketDetailList;
        }

        public IEnumerable<EventTicketDetail> GetByIds(IEnumerable<long> ids)
        {
            var EventTicketDetailList = GetAll(statement => statement
                                 .Where($"{nameof(EventTicketDetail.Id):C} IN @Ids")
                                 .WithParameters(new { Ids = ids }));
            return EventTicketDetailList;
        }

        public IEnumerable<EventTicketDetail> GetByEventTicketDetailIds(IEnumerable<long> ids)
        {
            var EventTicketDetailList = GetAll(statement => statement
                                 .Where($"{nameof(EventTicketDetail.Id):C} IN @EventDetailIds AND IsEnabled = 1")
                                 .WithParameters(new { EventDetailIds = ids }));
            return EventTicketDetailList;
        }

        public IEnumerable<EventTicketDetail> GetByEventTicketDetailsIds(IEnumerable<long> ids)
        {
            var EventTicketDetailList = GetAll(statement => statement
                                 .Where($"{nameof(EventTicketDetail.Id):C} IN @EventDetailIds")
                                 .WithParameters(new { EventDetailIds = ids }));
            return EventTicketDetailList;
        }

        public IEnumerable<EventTicketDetail> GetByEventDetailIds(IEnumerable<long> ids)
        {
            var eventTicketDetailList = GetAll(statement => statement
                .Where($"{nameof(EventTicketDetail.EventDetailId):C} IN @Id AND IsEnabled = 1")
                .WithParameters(new { Id = ids }));
            return eventTicketDetailList;
        }

        public EventTicketDetail GetByAltId(Guid AltId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventTicketDetail.AltId):C} = @TicketAltId ")
                .WithParameters(new { TicketAltId = AltId })).FirstOrDefault();
        }

        public IEnumerable<EventTicketDetail> GetByTicketCategoryIdAndeventDetailId(long ticketCategoryId, long eventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetail.TicketCategoryId):C} = @TicketCategoryId AND  {nameof(EventTicketDetail.EventDetailId):C} = @EventDetailId AND IsEnabled = 1")
                .WithParameters(new { TicketCategoryId = ticketCategoryId, EventDetailId = eventDetailId })
            );
        }

        public IEnumerable<EventTicketDetail> GetByEventDetailIdsAndIds(IEnumerable<long> eventDetailIds, IEnumerable<long> ids)
        {
            var eventTicketDetailList = GetAll(statement => statement
                .Where($"{nameof(EventTicketDetail.EventDetailId):C} IN @EventDetailIds AND {nameof(EventTicketDetail.Id):C} IN @Ids")
                .WithParameters(new { EventDetailIds = eventDetailIds, Ids = ids }));
            return eventTicketDetailList;
        }

        public IEnumerable<EventTicketDetail> GetAllByEventDetailIds(IEnumerable<long> ids)
        {
            var eventTicketDetailList = GetAll(statement => statement
                .Where($"{nameof(EventTicketDetail.EventDetailId):C} IN @Ids")
                .WithParameters(new { Ids = ids }));
            return eventTicketDetailList;
        }

        public IEnumerable<EventTicketDetail> GetAllByEventTicketDetailIds(IEnumerable<long> ids)
        {
            var EventTicketDetailList = GetAll(statement => statement
                                 .Where($"{nameof(EventTicketDetail.Id):C} IN @EventDetailIds")
                                 .WithParameters(new { EventDetailIds = ids }));
            return EventTicketDetailList;
        }

        public IEnumerable<EventTicketDetail> GetRASVRideEventTicketDetails()
        {
            return GetAll(statement => statement
                                 .Where($"EventDetailId IN(554439,554401)"));
        }

        public IEnumerable<EventTicketDetail> GetByTicketCategoryIdAndeventDetailIds(long ticketCategoryId, IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetail.TicketCategoryId):C} = @TicketCategoryId AND  {nameof(EventTicketDetail.EventDetailId):C} IN @EventDetailIds")
                .WithParameters(new { TicketCategoryId = ticketCategoryId, EventDetailIds = eventDetailIds })
            );
        }

        public IEnumerable<EventTicketDetail> GetByBOTicketCategoryIdAndeventDetailIds(long ticketCategoryId, IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetail.TicketCategoryId):C} = @TicketCategoryId AND  {nameof(EventTicketDetail.EventDetailId):C} IN @EventDetailIds AND ISNULL(IsBOEnabled,0)=1")
                .WithParameters(new { TicketCategoryId = ticketCategoryId, EventDetailIds = eventDetailIds })
            );
        }

        public EventTicketDetail GetByTicketCategoryIdAndEventTicketDetailId(long ticketCategoryId, long eventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetail.TicketCategoryId):C} = @TicketCategoryId AND  {nameof(EventTicketDetail.Id):C} = @EventTicketDetailId AND IsEnabled = 1")
                .WithParameters(new { TicketCategoryId = ticketCategoryId, EventTicketDetailId = eventTicketDetailId })
            ).FirstOrDefault();
        }

        public EventTicketDetail GetBySingleEventDetailId(long ids)
        {
            var eventTicketDetailList = GetAll(statement => statement
                 .Where($"{nameof(EventTicketDetail.EventDetailId):C} = @EventDetailId")
                 .WithParameters(new { EventDetailId = ids }));
            return eventTicketDetailList.FirstOrDefault();
        }

        public IEnumerable<EventTicketDetail> GetEventTicketDetailByTicketIdAndcitySightSeeingTicketTypeDetailId(string ticketId, int citySightSeeingTicketTypeDetailId)
        {
            return GetCurrentConnection().Query<EventTicketDetail>("Select D.*From CitySightSeeingTicketTypeDetails B With(NoLock) " +
                                                       "INNER JOIN CitySightSeeingEventTicketDetailMappings C With(NoLock) ON B.Id = C.CitySightSeeingTicketTypeDetailId " +
                                                       "Left Outer Join EventTicketDetails D With(NoLock) On c.EventTicketDetailId = D.Id" +
                                                       "Where B.TicketId = @TicketId AND B.Id = CitySightSeeingTicketTypeDetailId", new
                                                       {
                                                           TicketId = ticketId,
                                                           CitySightSeeingTicketTypeDetailId = citySightSeeingTicketTypeDetailId
                                                       });
        }

        public EventTicketDetail GetByTicketCategoryIdAndEventDetailId(long ticketCategoryId, long eventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetail.TicketCategoryId):C} = @TicketCategoryId AND  {nameof(EventTicketDetail.EventDetailId):C}  = @EventDetailId AND IsEnabled = 1")
                .WithParameters(new { TicketCategoryId = ticketCategoryId, EventDetailId = eventDetailId })
            ).FirstOrDefault();
        }

        public EventTicketDetail GetAllByTicketCategoryIdAndEventDetailId(long ticketCategoryId, long eventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetail.TicketCategoryId):C} = @TicketCategoryId AND  {nameof(EventTicketDetail.EventDetailId):C} = @EventDetailId ")
                .WithParameters(new { TicketCategoryId = ticketCategoryId, EventDetailId = eventDetailId })
            ).FirstOrDefault();
        }

        public IEnumerable<FIL.Contracts.Models.TMS.SponsorRequestTicketCategory> GetSponsorRequestTicketCategories(long eventDetailId)
        {
            return GetCurrentConnection().Query<Contracts.Models.TMS.SponsorRequestTicketCategory>("SELECT TC.Name AS TicketCategoryName, ETA.LocalPrice,CT.Code AS CurrencyName,ETA.RemainingTicketForSale, " +
                "ETA.Id AS EventTicketAttributeId " +
                "FROM EventTicketDetails ETD  WITH(NOLOCK) " +
                "INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON ETA.EventTicketDetailId = ETD.Id " +
                "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id " +
                "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON ETA.LocalCurrencyId = CT.Id " +
                "WHERE EventDetailId = @EventDetailId", new { EventDetailId = eventDetailId });
        }
    }
}