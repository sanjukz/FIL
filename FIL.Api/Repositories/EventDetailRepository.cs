using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.DynamicContent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventDetailRepository : IOrmRepository<EventDetail, EventDetail>
    {
        EventDetail Get(long id);

        IEnumerable<EventDetail> GetSubEventByEventId(long eventId);

        IEnumerable<EventDetail> GetByEventIds(IEnumerable<long> eventIds);

        EventDetail GetByEventId(long eventId);

        IEnumerable<EventDetail> GetSubeventByEventId(long eventId);

        IEnumerable<EventDetail> GetByEventIdAndEventDetailId(long eventId, IEnumerable<long> eventDetailIds);

        EventDetail GetByEventIdAndEventDetailIds(long eventId, long eventDetailIds);

        EventDetail GetById(long eventId);

        IEnumerable<EventDetail> GetByEventDetailIds(IEnumerable<long> eventDetailIds);

        EventDetail GetBySingleVenueId(long venueId);

        IEnumerable<EventDetail> GetByEvent(long eventId);

        IEnumerable<EventDetail> GetByVenueId(long venueId);

        EventDetail GetByEventAltId(Guid eventAltId);

        IEnumerable<EventDetail> GetByNames(List<string> names);

        EventDetail GetByName(string name);

        IEnumerable<EventDetail> GetByEventIdAndVenueId(long eventId, int venueId);

        IEnumerable<EventDetail> GetByPastEventIdAndVenueId(int eventId, int venueId);

        IEnumerable<EventDetail> GetBOByEventIdAndVenueId(long eventId, int venueId);

        IEnumerable<EventDetail> GetEventById(long eventId);

        IEnumerable<EventDetail> GetByVenueds(IEnumerable<int> venueIds);

        IEnumerable<EventDetail> GetAllByEventIdAndVenueIdsBo(long eventid, IEnumerable<int> uservenues);

        IEnumerable<EventDetail> GetSeasonEventDetails(long eventId, int venueId);

        IEnumerable<EventDetail> GetByVenueId(int venueId);

        EventDetail GetBySingleVenueId(int venueId);

        IEnumerable<EventDetail> GetAllByEventId(long eventId);

        EventDetail GetByAltId(Guid altId);

        IEnumerable<EventDetail> GetByEventIdAndVenueIds(long eventId, IEnumerable<int> venueIds);

        IEnumerable<EventDetail> GetAllByEventIdAndVenueIds(long eventId, IEnumerable<int> venueIds);

        IEnumerable<EventDetail> GetByIds(IEnumerable<long> ids);

        EventDetail GetByEventIdAndName(long eventId, string name);

        EventDetail GetByNameAndVenueId(string name, int venueId);

        IEnumerable<EventDetail> GetEventDetailForTMS(long eventId, int venueId);

        IEnumerable<EventDetail> GetEventDetailByAltId(Guid altId);

        IEnumerable<GetTicketDetailResponseModel> GetAllTicketDetails(long eventDetailId);
    }

    public class EventDetailRepository : BaseLongOrmRepository<EventDetail>, IEventDetailRepository
    {
        public EventDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventDetail Get(long id)
        {
            return Get(new EventDetail { Id = id });
        }

        public IEnumerable<EventDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEventDetail(EventDetail eventDetail)
        {
            Delete(eventDetail);
        }

        public EventDetail SaveEventDetail(EventDetail eventDetail)
        {
            return Save(eventDetail);
        }

        public IEnumerable<EventDetail> GetByEventIds(IEnumerable<long> eventIds)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventDetail.EventId):C} IN @Ids")
                    .WithParameters(new { Ids = eventIds }));
        }

        public EventDetail GetByEventId(long eventId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventDetail.EventId):C} = @Id")
                    .WithParameters(new { id = eventId })).FirstOrDefault();
        }

        public IEnumerable<EventDetail> GetSubeventByEventId(long eventId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventDetail.EventId):C} = @Id")
                    .WithParameters(new { id = eventId }));
        }

        public IEnumerable<EventDetail> GetByEventDetailIds(IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.Id):C} IN @EventDetailIds")
            .WithParameters(new { EventDetailIds = eventDetailIds }));
        }

        public IEnumerable<EventDetail> GetByVenueds(IEnumerable<int> venueIds)
        {
            List<EventDetail> result = new List<EventDetail>();
            while (venueIds.Any())
            {
                var ids2Query = venueIds.Take(1000);
                venueIds = venueIds.Skip(1000).ToList();
                var tempResult = GetAll(s => s.Where($"{nameof(EventDetail.VenueId):C} IN @VenueIds")
            .WithParameters(new { VenueIds = ids2Query })).ToList();
                result.AddRange(tempResult);
            }
            return result;
        }

        public EventDetail GetById(long eventId)
        {
            var eventDetailList = GetAll(statement => statement
                .Where($"{nameof(EventDetail.Id):C}=@Id")
                .WithParameters(new { Id = eventId }));
            return eventDetailList.FirstOrDefault();
        }

        public IEnumerable<EventDetail> GetSubEventByEventId(long eventId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventDetail.EventId):C}=@Ids AND IsEnabled=1")
                .WithParameters(new { Ids = eventId }));
        }

        public EventDetail GetByEventIdAndEventDetailIds(long eventId, long eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@Id AND {nameof(EventDetail.Id):C}= @EventDetailId ")
            .WithParameters(new { Id = eventId, EventDetailId = eventDetailIds })
            ).FirstOrDefault();
        }

        public IEnumerable<EventDetail> GetByEventIdAndEventDetailId(long eventId, IEnumerable<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@Id AND {nameof(EventDetail.Id):C} IN @EventDetailId ")
            .WithParameters(new { Id = eventId, EventDetailId = eventDetailIds })
            );
        }

        public IEnumerable<EventDetail> GetByEvent(long eventId)
        {
            var eventDetailList = GetAll(statement => statement
                .Where($"{nameof(EventDetail.EventId):C}=@Ids")
                .WithParameters(new { Ids = eventId }));
            return eventDetailList;
        }

        public IEnumerable<EventDetail> GetByVenueId(long venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.VenueId):C}=@VenueId ")
            .WithParameters(new { VenueId = venueId }));
        }

        public EventDetail GetBySingleVenueId(int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.VenueId):C}=@VenueId ")
            .WithParameters(new { VenueId = venueId })).FirstOrDefault(); ;
        }

        public EventDetail GetByEventAltId(Guid eventAltId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.AltId):C}=@EventAltId ")
            .WithParameters(new { EventAltId = eventAltId })).FirstOrDefault();
        }

        public IEnumerable<EventDetail> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }

        public EventDetail GetByNameAndVenueId(string name, int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.Name):C} = @Name AND {nameof(EventDetail.VenueId)} = @VenueId")
                .WithParameters(new { Name = name, VenueId = venueId })
            ).FirstOrDefault();
        }

        public EventDetail GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.Name):C}=@Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public IEnumerable<EventDetail> GetBOByEventIdAndVenueId(long eventId, int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.VenueId):C} = @VenueId AND ISNULL(IsBOEnabled,0)=1")
            .WithParameters(new { EventId = eventId, VenueId = venueId })
            );
        }

        public IEnumerable<EventDetail> GetByEventIdAndVenueId(long eventId, int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.VenueId):C} = @VenueId AND IsEnabled = 1")
            .WithParameters(new { EventId = eventId, VenueId = venueId })
            );
        }

        public IEnumerable<EventDetail> GetByPastEventIdAndVenueId(int eventId, int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.VenueId):C} = @VenueId")
            .WithParameters(new { EventId = eventId, VenueId = venueId })
            );
        }

        public IEnumerable<EventDetail> GetSeasonEventDetails(long eventId, int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.VenueId):C} = @VenueId ")
            .WithParameters(new { EventId = eventId, VenueId = venueId })
            );
        }

        public EventDetail GetByEventIdAndName(long eventId, string name)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.Name):C} = @Name ")
            .WithParameters(new { EventId = eventId, Name = name })).FirstOrDefault();
        }

        public IEnumerable<EventDetail> GetEventById(long eventId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventDetail.Id):C} = @Id")
                    .WithParameters(new { Id = eventId }));
        }

        public IEnumerable<EventDetail> GetByEventIdAndVenueIds(long eventId, IEnumerable<int> venueIds)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.VenueId):C} IN @VenueId AND IsEnabled = 1")
            .WithParameters(new { EventId = eventId, VenueId = venueIds })
            );
        }

        public IEnumerable<EventDetail> GetAllByEventIdAndVenueIds(long eventId, IEnumerable<int> venueIds)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.VenueId):C} IN @VenueId")
            .WithParameters(new { EventId = eventId, VenueId = venueIds })
            );
        }

        public IEnumerable<EventDetail> GetAllByEventIdAndVenueIdsBo(long eventId, IEnumerable<int> venueIds)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.VenueId):C} IN @VenueId AND IsBOEnabled = 1")
            .WithParameters(new { EventId = eventId, VenueId = venueIds })
            );
        }

        public IEnumerable<EventDetail> GetByVenueId(int venueId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventDetail.VenueId):C}=@VenueId")
                .WithParameters(new { VenueId = venueId }));
        }

        public IEnumerable<EventDetail> GetAllByEventId(long eventId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(EventDetail.EventId):C}=@Ids")
                .WithParameters(new { Ids = eventId }));
        }

        public EventDetail GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.AltId):C}=@AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<EventDetail> GetByIds(IEnumerable<long> ids)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventDetail.Id):C} IN @Ids")
                    .WithParameters(new { Ids = ids }));
        }

        public EventDetail GetBySingleVenueId(long venueId)
        {
            var eventDetailList = GetAll().OrderByDescending(p => p.Id).FirstOrDefault(p => p.VenueId == venueId);
            return eventDetailList;
        }

        public IEnumerable<GetTicketDetailResponseModel> GetAllTicketDetails(long eventDetailId)
        {
            return GetCurrentConnection().Query<GetTicketDetailResponseModel>("select Distinct(Tc.Name),eta.Price,ct.code as Currency,eta.SalesStartDateTime,eta.SalesEndDatetime,eta.AvailableTicketForSale from eventdetails ed with (NOLOCK) inner join eventticketdetails etd with (NOLOCK) on etd.EventDetailId=ed.Id inner join ticketcategories tc with (NOLOCK) on tc.id=etd.ticketcategoryid inner join eventticketattributes eta with (NOLOCK) on eta.EventTicketDetailId=etd.Id inner join CurrencyTypes ct with (NOLOCK) on ct.id=eta.currencyid where etd.isenabled=1 and eta.isenabled=1 and ed.id=" + eventDetailId.ToString(), null, GetCurrentTransaction());
        }

        public IEnumerable<EventDetail> GetEventDetailForTMS(long eventId, int venueId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.EventId):C}=@EventId AND {nameof(EventDetail.VenueId):C} = @VenueId ")
            .WithParameters(new { EventId = eventId, VenueId = venueId })
            );
        }

        public IEnumerable<EventDetail> GetEventDetailByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(EventDetail.AltId):C}=@AltId")
                .WithParameters(new { AltId = altId })
            );
        }
    }
}