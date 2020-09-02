using Dapper;
using Dapper.FastCrud;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Contracts.Models.Zoom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventRepository : IOrmRepository<Event, Event>
    {
        Event Get(long id);

        Event GetById(IEnumerable<long> id);

        Event GetByAltId(Guid altId);

        IEnumerable<Event> GetByAltIds(List<Guid> altIds);
        IEnumerable<Event> GetAllByCreatorAltIds(List<Guid> altIds);

        Event GetBySlug(string slug);

        IEnumerable<Event> GetByAltIds(Guid createdby);

        IEnumerable<Event> GetAllFeelEvents(bool isFeel);

        IEnumerable<Event> GetAllFeelEvents(bool isFeel, string searchString);

        IEnumerable<Event> GetEventsByAltIds(List<Guid> altIds);

        IEnumerable<Event> GetByEventIds(IEnumerable<long> eventIds);

        IEnumerable<Event> GetByAllTypeEventIds(IEnumerable<long> eventIds);

        IEnumerable<Event> GetByAllEventIds(IEnumerable<long> eventIds);

        IEnumerable<Event> GetByName(string eventName);

        Event GetByEventName(string eventName);

        Event GetByEventId(long eventIds);

        IEnumerable<Event> GetByCategoryId(int eventCategoryId);

        IEnumerable<Event> GetByNames(List<string> names);

        IEnumerable<Event> GetAllByName(string eventName);

        IEnumerable<Event> GetEventsByProduct(bool isFeel);

        IEnumerable<Contracts.Models.Export.FeelExportContainer> GetEventsForIA(bool isFeel);

        IEnumerable<Event> GetSEOEventsByProduct(bool isFeel);

        IEnumerable<Event> GetByNameAndEventId(string eventName, IEnumerable<long> eventIds);

        IEnumerable<Event> GetFeelEventsByName(string eventName, Site siteId);

        IEnumerable<Event> GetFeelEventsBySearchString(string eventName, Site siteId);

        IEnumerable<Event> GetFeelEventsBySearch(string eventName, string cityName, string countryName, string category, string subCategory, Site siteId);

        IEnumerable<Event> GetFeelEventsBySite(Site siteId);

        IEnumerable<Event> GetEvents();

        IEnumerable<Event> GetAllZoongaEvents(bool isFeel);

        IEnumerable<Event> GetAllFeelAdminEvents();

        //Event SaveEvent(Event eventObj);
        IEnumerable<Event> GetAllByUserAltId(Guid altId);

        IEnumerable<Event> GetAllZEvents();

        IEnumerable<Event> GetAllPlaceByCountry(string countryName);

        IEnumerable<Event> GetAllPlaceCity(string cityName);

        IEnumerable<Event> GetAllTournamentEvents();

        IEnumerable<Event> GetAllFeelEventsByCreatedBy(Guid altId);

        IEnumerable<Event> GetAllFeelEventsByCreatedBy(bool isFeel, string searchString, Guid guid);

        Event GetByFeelEventName(string names);

        IEnumerable<Event> GetBySourceId(EventSource eventSource);

        IEnumerable<Event> GetBOEventByEventIds(IEnumerable<long> eventIds);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCategoryPage(List<int> subcategoryIds);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCountry(List<int> subcategoryIds, int countryId);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByState(List<int> subcategoryIds, int stateId);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCity(List<int> subcategoryIds, int cityId);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCountry(int CountryId);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByState(int StateId);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCity(int CityId);

        IEnumerable<Event> GetTMSEventByEventIds(IEnumerable<long> eventIds);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlacesForAlgolia(bool isFeel);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetPlaceForAlgolia(long eventId);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllBySlug(string slug);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllItineraryPlaces(List<int> cityIds, List<long> categotyIds);

        IEnumerable<FIL.Contracts.Models.PlaceDetail> GetItinerarySearchPlaces(int eventId);

        IEnumerable<Event> GetAllByDateEvents(DateTime fromDate);

        IEnumerable<Event> GetByEventNameAndSourceId(string eventName, EventSource eventSource);

        IEnumerable<StripeConnectMaster> EventStripeConnectMaster(long transactionId);

        IEnumerable<Event> GetAllFeels(bool isEnabled);

        IEnumerable<LiveOnlineTransactionDetailResponseModel> GetMyFeelDetails(List<long> eventIds);

        IEnumerable<FIL.Contracts.Models.Creator.MyFeel> GetMyFeels(Guid AltId);

        IEnumerable<FIL.Contracts.Models.Creator.MyFeel> GetApproveModerateFeels(bool isEnabled);
    }

    public class EventRepository : BaseLongOrmRepository<Event>, IEventRepository
    {
        public EventRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Event Get(long id)
        {
            return Get(new Event { Id = id });
        }

        public IEnumerable<Event> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<Event> GetAllTournamentEvents()
        {
            return GetAll(null).Where(p => p.EventTypeId == EventType.Tournament);
        }

        public IEnumerable<Event> GetAllFeelEvents(bool isFeel)
        {
            return GetAll(null).Where(p => p.IsFeel == isFeel).OrderByDescending(p => p.UpdatedUtc).Take(100);
        }

        public IEnumerable<Event> GetAllFeelEventsByCreatedBy(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(Event.CreatedBy):C} = @AltId AND IsFeel=1")
               .WithParameters(new { AltId = altId })
           );
        }

        public IEnumerable<Event> GetAllFeelAdminEvents()
        {
            return GetAll(null).Where(p => p.IsFeel == true && p.IsCreatedFromFeelAdmin == true && p.IsEnabled == false);
        }

        public IEnumerable<Event> GetAllFeels(bool isEnabled)
        {
            return GetAll(s => s.Where($"{nameof(Event.IsEnabled):C} = @IsEnabled AND IsFeel=1")
               .WithParameters(new { IsEnabled = isEnabled })
           );
        }

        public IEnumerable<Event> GetAllFeelEvents(bool isFeel, string searchString)
        {
            var data = searchString.Split("_");
            string search = data[0];
            var category = data[1].Split(",");
            List<int> obj = new List<int>();
            for (int i = 0; i < category.Length; i++)
            {
                if (category[i] != "")
                {
                    obj.Add(Convert.ToInt32(category[i]));
                }
            }

            if (obj.Count() > 0 && search.Length > 2)
            {
                return GetAll(null).Where(p => p.IsFeel == isFeel && p.Name.ToLower().Contains(search.ToLower()) && obj.Contains((int)p.EventCategoryId)).OrderByDescending(p => p.UpdatedUtc).Take(100);
            }
            else if (obj.Count() > 0 && search.Length <= 2)
            {
                return GetAll(null).Where(p => p.IsFeel == isFeel && obj.Contains((int)p.EventCategoryId)).OrderByDescending(p => p.UpdatedUtc).Take(100);
            }
            else if (search.Length > 2 && obj.Count == 0)
            {
                return GetAll(null).Where(p => p.IsFeel == isFeel && p.Name.ToLower().Contains(search.ToLower())).OrderByDescending(p => p.UpdatedUtc).Take(100);
            }
            else
            {
                return GetAll(null).Where(p => p.IsFeel == isFeel).OrderByDescending(p => p.UpdatedUtc).Take(100);
            }
        }

        public IEnumerable<Event> GetAllFeelEventsByCreatedBy(bool isFeel, string searchString, Guid guid)
        {
            var data = searchString.Split("_");
            string search = data[0];
            var category = data[1].Split(",");
            List<int> obj = new List<int>();
            for (int i = 0; i < category.Length; i++)
            {
                if (category[i] != "")
                {
                    obj.Add(Convert.ToInt32(category[i]));
                }
            }

            if (obj.Count() > 0 && search.Length > 2)
            {
                return GetAll(null).Where(p => p.IsFeel == isFeel && p.CreatedBy == guid && p.Name.ToLower().Contains(search.ToLower()) && obj.Contains((int)p.EventCategoryId)).OrderByDescending(p => p.UpdatedUtc).Take(100);
            }
            else if (obj.Count() > 0 && search.Length <= 2)
            {
                return GetAll(null).Where(p => p.IsFeel == isFeel && p.CreatedBy == guid && obj.Contains((int)p.EventCategoryId)).OrderByDescending(p => p.UpdatedUtc).Take(100);
            }
            else if (search.Length > 2 && obj.Count == 0)
            {
                return GetAll(null).Where(p => p.IsFeel == isFeel && p.CreatedBy == guid && p.Name.ToLower().Contains(search.ToLower())).OrderByDescending(p => p.UpdatedUtc).Take(100);
            }
            else
            {
                return GetAll(null).Where(p => p.IsFeel == isFeel && p.CreatedBy == guid).OrderByDescending(p => p.UpdatedUtc).Take(100);
            }
        }

        public IEnumerable<Event> GetEventsByAltIds(List<Guid> altIds)
        {
            List<string> strAltIds = new List<string>();
            foreach (var value in altIds)
            {
                strAltIds.Add("'" + value + "'");
            }

            string concatAltIds = string.Join(" Or e.AltId = ", strAltIds);
            string strWhere = " (e.AltId = " + concatAltIds + ") ";

            string strQuery = "SELECT DISTINCT e.* FROM Events e WITH(NOLOCK) " +
                "INNER JOIN EventSiteIdMappings esm WITH(NOLOCK) ON e.Id = esm.EventId " +
                "INNER JOIN EventCategoryMappings ecm WITH(NOLOCK) ON e.Id = ecm.EventId " +
                "INNER JOIN EventCategories ec WITH(NOLOCK) ON ec.Id = ecm.EventCategoryId " +
                "INNER JOIN EventDetails ED WITH(NOLOCK) ON e.Id = ED.EventId " +
                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.EventDetailId = ED.Id " +
                "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
                "WHERE " + strWhere + " AND e.IsEnabled = 1 AND ED.IsEnabled = 1";

            return GetCurrentConnection().Query<Event>("SELECT DISTINCT e.* FROM Events e WITH(NOLOCK) " +
                "INNER JOIN EventSiteIdMappings esm WITH(NOLOCK) ON e.Id = esm.EventId " +
                "INNER JOIN EventCategoryMappings ecm WITH(NOLOCK) ON e.Id = ecm.EventId " +
                "INNER JOIN EventCategories ec WITH(NOLOCK) ON ec.Id = ecm.EventCategoryId " +
                "INNER JOIN EventDetails ED WITH(NOLOCK) ON e.Id = ED.EventId " +
                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.EventDetailId = ED.Id " +
                "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
                "WHERE " + strWhere + " AND e.IsEnabled = 1 AND ED.IsEnabled = 1 ");
        }

        public void DeleteEvent(Event objEvent)
        {
            Delete(objEvent);
        }

        public Event GetById(IEnumerable<long> id)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(Event.Id):C} = @Id")
                    .WithParameters(new { Id = id })).FirstOrDefault();
        }

        public Event GetByEventId(long eventIds)
        {
            var eventList = GetAll(statement => statement
                           .Where($"{nameof(Event.Id):C} = @Ids")
                           .WithParameters(new { Ids = eventIds }));
            return eventList.FirstOrDefault();
        }

        public IEnumerable<Event> GetByEventIds(IEnumerable<long> eventIds)
        {
            var eventList = GetAll(statement => statement
                           .Where($"{nameof(Event.Id):C} IN @Ids AND IsEnabled=1")
                           .WithParameters(new { Ids = eventIds }));
            return eventList;
        }

        public IEnumerable<Event> GetByAllTypeEventIds(IEnumerable<long> eventIds)
        {
            var eventList = GetAll(statement => statement
                           .Where($"{nameof(Event.Id):C} IN @Ids")
                           .WithParameters(new { Ids = eventIds }));
            return eventList;
        }

        public IEnumerable<Event> GetByAllEventIds(IEnumerable<long> eventIds)
        {
            var eventList = GetAll(statement => statement
                           .Where($"{nameof(Event.Id):C} IN @Ids")
                           .WithParameters(new { Ids = eventIds }));
            return eventList;
        }

        public Event GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(Event.AltId):C} = @AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<Event> GetAllByUserAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(Event.CreatedBy):C} = @AltId")
                .WithParameters(new { AltId = altId })
            );
        }

        public Event GetBySlug(string slug)
        {
            return GetAll(s => s.Where($"{nameof(Event.Slug):C} = @Slug")
                .WithParameters(new { Slug = slug })
            ).FirstOrDefault();
        }

        public IEnumerable<Event> GetBySourceId(EventSource eventSource)
        {
            return GetAll(s => s.Where($"{nameof(Event.EventSourceId):C} = @Source")
                .WithParameters(new { Source = eventSource })
            );
        }

        public IEnumerable<Event> GetByAltIds(Guid createdby)
        {
            var eventList = GetAll(s => s.Where($"{nameof(Event.CreatedBy):C} = @CreatedBy")
                .WithParameters(new { CreatedBy = createdby }));
            return eventList;
        }

        public IEnumerable<Event> GetByName(string eventName)
        {
            var eventList = (GetAll(s => s.Where($"{nameof(Event.Name):C} LIKE '%'+@Name+'%' AND IsEnabled=1")
                   .WithParameters(new { Name = eventName })
            ));
            return eventList;
        }

        public IEnumerable<Event> GetAllByName(string eventName)
        {
            var eventList = (GetAll(s => s.Where($"{nameof(Event.Name):C} LIKE '%'+@Name+'%'")
                   .WithParameters(new { Name = eventName })
            ));
            return eventList;
        }

        public IEnumerable<Event> GetByNameAndEventId(string eventName, IEnumerable<long> eventIds)
        {
            var eventList = (GetAll(s => s.Where($"{nameof(Event.Name):C} LIKE '%'+@Name+'%' AND IsEnabled=1 OR {nameof(Event.Id):C} IN @eventIds AND IsEnabled=1")
                   .WithParameters(new { Name = eventName, eventIds = eventIds })
            ));
            return eventList;
        }

        public IEnumerable<Event> GetByCategoryId(int eventCategoryId)
        {
            var categoryEventList = (GetAll(s => s.Where($"{nameof(Event.EventCategoryId):C} = @EventCategoryId")
                 .WithParameters(new { EventCategoryId = (int)eventCategoryId })
             ));
            return categoryEventList;
        }

        public IEnumerable<Event> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(Event.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }

        public Event GetByEventName(string names)
        {
            return GetAll(s => s.Where($"{nameof(Event.Name):C} = @Name")
                .WithParameters(new { Name = names })
            ).FirstOrDefault();
        }

        public Event GetByFeelEventName(string names)
        {
            return GetAll(s => s.Where($"{nameof(Event.Name):C} = @Name AND IsFeel=1")
                .WithParameters(new { Name = names })
            ).FirstOrDefault();
        }

        public IEnumerable<Event> GetByAltIds(List<Guid> altIds)
        {
            return GetAll(s => s.Where($"{nameof(Event.AltId):C} in @AltId")
                .WithParameters(new { AltId = altIds })
            );
        }

        public IEnumerable<Event> GetAllByCreatorAltIds(List<Guid> altIds)
        {
            return GetAll(s => s.Where($"{nameof(Event.CreatedBy):C} in @AltId")
                .WithParameters(new { AltId = altIds })
            );
        }

        public IEnumerable<Event> GetAllZEvents()
        {
            var eventList = GetAll(statement => statement
                           .Where($"{nameof(Event.IsFeel):C}=0 AND IsEnabled=1")
                           .WithParameters(new { }));
            return eventList;
        }

        public IEnumerable<Event> GetEventsByProduct(bool isFeel)
        {
            IEnumerable<Event> eventList;
            if (isFeel)
            {
                eventList = (GetAll(s => s.Where($"{nameof(Event.IsFeel):C} = @isFeel AND IsEnabled=1")
                    .WithParameters(new { IsFeel = isFeel })
                ));
            }
            else
            {
                eventList = (GetAll(s => s.Where($"({nameof(Event.IsFeel):C} IS NULL OR {nameof(Event.IsFeel):C} = @isFeel) AND IsEnabled=1")
                    .WithParameters(new { IsFeel = isFeel })
                ));
            }

            return eventList;
        }

        public IEnumerable<Contracts.Models.Export.FeelExportContainer> GetEventsForIA(bool isFeel)
        {
            return GetCurrentConnection().Query<Contracts.Models.Export.FeelExportContainer>("SELECT DISTINCT ETA.Id AS Id, " +
                                                        "ESIM.SiteId AS SiteId, ECM.EventCategoryId AS CategoryId, " +
                                                        "E.AltId AS ParentId, E.Name AS ParentName, E.Description AS ParentDescription, TC.Name AS Name, " +
                                                        "ETA.TicketCategoryDescription AS Description, ETA.Price AS Price, C.Id AS CityId, C.Name AS CityName, " +
                                                        "S.Id AS StateId, S.Name AS StateName, COU.Id AS CountryId, COU.Name AS CountryName, E.Slug  " +
                                                        "FROM " +
                                                        "Events  E WITH (NOLOCK) " +
                                                        "INNER JOIN EventDetails ED WITH(NOLOCK) ON E.Id = ED.EventId " +
                                                        "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ED.Id = ETD.EventDetailId " +
                                                        "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETD.Id = ETA.EventTicketDetailId " +
                                                        "INNER JOIN Venues V WITH(NOLOCK) ON V.Id = ED.VenueId " +
                                                        "INNER JOIN Cities C WITH(NOLOCK) ON C.Id = V.CityId " +
                                                        "INNER JOIN States S WITH(NOLOCK) ON S.Id = C.StateId " +
                                                        "INNER JOIN Countries COU WITH(NOLOCK) ON COU.Id = S.CountryId " +
                                                        "INNER JOIN TicketCategories TC WITH(NOLOCK) ON TC.Id = ETD.TicketCategoryId " +
                                                        "INNER JOIN EventSiteIdMappings ESIM WITH(NOLOCK) ON ESIM.EventId = E.Id " +
                                                        "INNER JOIN EventCategoryMappings ECM WITH (NOLOCK) ON ECM.EventId = E.Id " +
                                                        "WHERE e.IsEnabled=1 AND e.IsFeel = @IsFeel  ", new
                                                        {
                                                            IsFeel = isFeel
                                                        });
        }

        public IEnumerable<Event> GetSEOEventsByProduct(bool isFeel)
        {
            IEnumerable<Event> eventList;
            if (isFeel)
            {
                eventList = (GetAll(s => s.Where($"{nameof(Event.IsFeel):C} = @isFeel AND IsEnabled=1 AND ISNULL(Description, '') <> '' ")
                    .WithParameters(new { IsFeel = isFeel })
                ));
            }
            else
            {
                eventList = (GetAll(s => s.Where($"({nameof(Event.IsFeel):C} IS NULL OR {nameof(Event.IsFeel):C} = @isFeel) AND IsEnabled=1")
                    .WithParameters(new { IsFeel = isFeel })
                ));
            }

            return eventList;
        }

        public IEnumerable<Event> GetEvents()
        {
            return GetAll(s => s.Where($"{nameof(Event.IsEnabled):C}= @IsEnabled AND {nameof(Event.IsPublishedOnSite):C}= @IsPublishedOnSite ")
            .WithParameters(new { IsEnabled = 1, IsPublishedOnSite = 1 })
            );
        }

        public IEnumerable<Event> GetFeelEventsByName(string eventName, Site siteId)
        {
            return GetCurrentConnection().Query<Event>("SELECT e.* FROM Events e WITH (NOLOCK) " +
                                                       "INNER JOIN EventSiteIdMappings esm WITH (NOLOCK) ON e.Id=esm.EventId " +
                                                       "WHERE e.Name LIKE '%'+@EventName+'%' AND e.IsEnabled=1 AND e.IsFeel=1 " +
                                                       "AND esm.IsEnabled=1 AND esm.SiteId=@SiteId", new
                                                       {
                                                           SiteId = siteId,
                                                           EventName = eventName
                                                       });
        }

        public IEnumerable<Event> GetFeelEventsBySearchString(string searchString, Site siteId)
        {
            return GetCurrentConnection().Query<Event>("SELECT DISTINCT e.* FROM Events e WITH (NOLOCK) " +
                "INNER JOIN EventSiteIdMappings esm WITH(NOLOCK) ON e.Id = esm.EventId " +
                "INNER JOIN EventCategoryMappings ecm WITH(NOLOCK) ON e.Id = ecm.EventId " +
                "INNER JOIN EventCategories ec WITH(NOLOCK) ON ec.Id = ecm.EventCategoryId " +
                "INNER JOIN EventDetails ED WITH(NOLOCK) ON E.Id = ED.EventId " +
                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.EventDetailId = ED.Id " +
                "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
                "INNER JOIN Venues V WITH(NOLOCK) ON V.Id = ED.VenueId " +
                "INNER JOIN Cities C WITH(NOLOCK) ON C.Id = V.CityId " +
                "INNER JOIN States S WITH(NOLOCK) ON S.Id = C.StateId " +
                "INNER JOIN Countries COU WITH(NOLOCK) ON COU.Id = S.CountryId " +
                "WHERE ( " +
                "e.Name LIKE '%' + @SearchString + '%' " +
                "OR C.Name like '%' + @SearchString + '%' " +
                "OR COU.Name like '%' + @SearchString + '%' " +
                "OR ec.Category like '%' + @SearchString + '%' " +
                "OR ec.id IN(select Id from EventCategories where EventCategoryId in (select Id from EventCategories where Category = '' + @SearchString + ''))) " +
                "AND e.IsEnabled = 1 " +
                "AND e.IsFeel = 1 " +
                "AND esm.IsEnabled = 1 " +
                "AND esm.SiteId = @SiteId ", new
                {
                    SiteId = siteId,
                    SearchString = searchString
                });
        }

        public IEnumerable<Event> GetFeelEventsBySearch(string eventName, string cityName, string countryName, string category, string subCategory, Site siteId)
        {
            string strQuery = "SELECT DISTINCT e.*FROM Events e WITH(NOLOCK) " +
                "INNER JOIN EventSiteIdMappings esm WITH(NOLOCK) ON e.Id = esm.EventId " +
                "INNER JOIN EventCategoryMappings ecm WITH(NOLOCK) ON e.Id = ecm.EventId " +
                "INNER JOIN EventCategories ec WITH(NOLOCK) ON ec.Id = ecm.EventCategoryId " +
                "INNER JOIN EventDetails WITH(NOLOCK) ED ON E.Id = ED.EventId " +
                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.EventDetailId = ED.Id " +
                "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
                "INNER JOIN Venues V WITH(NOLOCK) ON V.Id = ED.VenueId " +
                "INNER JOIN Cities C WITH(NOLOCK) ON C.Id = V.CityId " +
                "INNER JOIN States S WITH(NOLOCK) ON S.Id = C.StateId " +
                "INNER JOIN Countries COU WITH(NOLOCK) ON COU.Id = S.CountryId " +
                "WHERE " +
                "e.Name like CASE ISNULL('" + eventName + "', '') WHEN '' THEN e.Name ELSE '%" + eventName + "%' END " +
                "AND C.Name like CASE ISNULL('" + cityName + "', '') WHEN '' THEN C.Name ELSE '%" + cityName + "%' END " +
                "AND COU.Name like CASE ISNULL('" + countryName + "', '') WHEN '' THEN COU.Name ELSE '%" + countryName + "%' END " +
                "AND ec.Category like CASE ISNULL('" + subCategory + "', '') WHEN '' THEN ec.DisplayName ELSE '" + subCategory + "' END " +
                "AND((ISNULL('" + category + "', '') = '' and ec.Id = ec.Id)  or(ISNULL('" + category + "', '') <> '' and ec.id IN(select Id from EventCategories where EventCategoryId in (select Id from EventCategories where Category = '" + category + "')))) " +
                "AND e.IsEnabled = 1 " +
                "AND e.IsFeel = 1 " +
                "AND esm.IsEnabled = 1 " +
                "AND esm.SiteId =  " + siteId + "";

            return GetCurrentConnection().Query<Event>("SELECT DISTINCT e.* FROM Events e WITH (NOLOCK) " +
                "INNER JOIN EventSiteIdMappings esm WITH(NOLOCK) ON e.Id = esm.EventId " +
                "INNER JOIN EventCategoryMappings ecm WITH(NOLOCK) ON e.Id = ecm.EventId " +
                "INNER JOIN EventCategories ec WITH(NOLOCK) ON ec.Id = ecm.EventCategoryId " +
                "INNER JOIN EventDetails ED WITH(NOLOCK) ON E.Id = ED.EventId " +
                "INNER JOIN Venues V WITH(NOLOCK) ON V.Id = ED.VenueId " +
                "INNER JOIN Cities C WITH(NOLOCK) ON C.Id = V.CityId " +
                "INNER JOIN States S WITH(NOLOCK) ON S.Id = C.StateId " +
                "INNER JOIN Countries COU WITH(NOLOCK) ON COU.Id = S.CountryId " +
                "WHERE " +
                "e.Name like CASE ISNULL(@EventName, '') WHEN '' THEN e.Name ELSE '%' + @EventName + '%' END " +
                "AND C.Name like CASE ISNULL(@CityName, '') WHEN '' THEN C.Name ELSE '%' + @CityName + '%' END " +
                "AND COU.Name like CASE ISNULL(@CountryName, '') WHEN '' THEN COU.Name ELSE '%' + @CountryName + '%' END " +
                "AND ec.Category like CASE ISNULL(@SubCategoryName, '') WHEN '' THEN ec.DisplayName ELSE '' + @SubCategoryName + '' END " +
                "AND((ISNULL(@CategoryName, '') = '' and ec.Id = ec.Id)  or(ISNULL(@CategoryName, '') <> '' and ec.id IN(select Id from EventCategories where EventCategoryId in (select Id from EventCategories where Category = @CategoryName)))) " +
                "AND e.IsEnabled = 1 " +
                "AND e.IsFeel = 1 " +
                "AND esm.IsEnabled = 1 " +
                "AND esm.SiteId = @SiteId ", new
                {
                    SiteId = siteId,
                    EventName = eventName,
                    CityName = cityName,
                    CountryName = countryName,
                    CategoryName = category,
                    SubCategoryName = subCategory,
                });
        }

        public IEnumerable<Event> GetFeelEventsBySite(Site siteId)
        {
            return GetCurrentConnection().Query<Event>("SELECT e.* FROM Events e WITH (NOLOCK) " +
                                                       "INNER JOIN EventSiteIdMappings esm WITH (NOLOCK) ON e.Id=esm.EventId " +
                                                       "WHERE e.IsEnabled=1 AND e.IsFeel=1 " +
                                                       "AND esm.IsEnabled=1 AND esm.SiteId=@SiteId", new
                                                       {
                                                           SiteId = siteId
                                                       });
        }

        public IEnumerable<Event> GetAllZoongaEvents(bool isFeel)
        {
            return GetCurrentConnection().Query<Event>("SELECT DISTINCT E.* FROM Events E WITH (NOLOCK) " +
                                                        "INNER JOIN EventDetails ED WITH (NOLOCK) ON E.Id = ED.EventId " +
                                                        "INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.EventDetailId = ED.Id " +
                                                        "INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
                                                        "INNER JOIN TicketCategories TC WITH (NOLOCK) ON TC.Id = ETD.TicketCategoryId " +
                                                        "INNER JOIN TransactionDetails TD WITH (NOLOCK) ON TD.EventTicketAttributeId = ETA.Id " +
                                                        "INNER JOIN Transactions T WITH (NOLOCK) ON T.ID = TD.TransactionId " +
                                                        "WHERE TransactionStatusId = 8 AND ISNULL(E.IsFeel, 0) = @IsFeel " +
                                                        "ORDER By E.Name", new
                                                        {
                                                            IsFeel = isFeel
                                                        });
        }

        public IEnumerable<Event> GetAllPlaceByCountry(string countryName)
        {
            return GetCurrentConnection().Query<Event>("select DISTINCT e.* from events e  WITH (NOLOCK) " +
                                                       " Inner Join eventdetails ed WITH (NOLOCK) on e.id = ed.eventId  " +
                                                       " Inner Join venues v WITH (NOLOCK) on ed.venueId = v.id  " +
                                                       " Inner Join cities ct WITH (NOLOCK) on ct.id = v.cityid  " +
                                                       " Inner Join states st WITH (NOLOCK) on st.id = ct.stateid  " +
                                                       " Inner Join countries country WITH (NOLOCK) on country.id = st.countryId " +
                                                       "WHERE country.name LIKE '%'+@CountryName+'%' AND e.IsEnabled=1 AND e.IsFeel=1 "
                                                       , new
                                                       {
                                                           CountryName = countryName,
                                                       });
        }

        public IEnumerable<Event> GetAllPlaceCity(string cityName)
        {
            return GetCurrentConnection().Query<Event>("select DISTINCT e.* from events e  WITH (NOLOCK) " +
                                                       " Inner Join eventdetails ed WITH (NOLOCK) on e.id = ed.eventId  " +
                                                       " Inner Join venues v WITH (NOLOCK) on ed.venueId = v.id  " +
                                                       " Inner Join cities ct WITH (NOLOCK) on ct.id = v.cityid  " +
                                                       " Inner Join states st WITH (NOLOCK) on st.id = ct.stateid  " +
                                                       " Inner Join countries country WITH (NOLOCK) on country.id = st.countryId " +
                                                       "WHERE ct.name LIKE '%'+@City+'%' AND e.IsEnabled=1 AND e.IsFeel=1 "
                                                       , new
                                                       {
                                                           City = cityName,
                                                       });
        }

        public string GetScript()
        {
            return "Select E.Id as Id, E.IsTokenize as IsTokenize, Ed.EventFrequencyType as EventFrequencyType, E.Name AS Name, E.MasterEventTypeId AS MasterEventTypeId,E.EventTypeId AS EventTypeId, E.altId as AltId, E.slug as Slug, E.EventSourceId as EventSource, Ed.Id as EventDetailId,Ed.StartDateTime as EventStartDateTime,Ed.EndDateTime as EventEndDateTime, Ec.Id as EventCategoryId, Ec.displayname as Category, Ec.slug as SubCategorySlug,  C.id as CityId, C.name as CityName, S.Id as StateId, S.Name as StateName, Co.Id as CountryId, Co.AltId as CountryAltId, co.name as CountryName, '/place/'+ (select  slug from EventCategories where Id In(select Distinct EventCategoryId from EventCategories where Id=EC.Id) ) +'/' + E.slug + '/' + Ec.slug as URL, " +
                                                       "(select  DisplayName from EventCategories With(NOLOCK) where Id In(select Distinct EventCategoryId from EventCategories where Id=EC.Id) ) as ParentCategory, " +
                                                       "(select  Id from EventCategories With(NOLOCK) where Id In(select Distinct EventCategoryId from EventCategories where Id=EC.Id) ) as ParentCategoryId,  " +
                                                       "(select  slug from EventCategories With(NOLOCK) where Id In(select Distinct EventCategoryId from EventCategories where Id=EC.Id) ) as ParentCategorySlug,  " +
                                                       "(Select TOP 1 Min(Price) From EventTicketAttributes ETA With(NOLOCK) INNER JOIN EventTicketDetails ETD With(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id Where ETA.IsEnabled = 1 AND ETD.IsEnabled = 1 AND Isnull(Price, 0) <> 0 AND ETD.EventDetailId = ED.Id) as MinPrice, " +
                                                       "(Select TOP 1 Max(Price) From EventTicketAttributes ETA  With(NOLOCK) INNER JOIN EventTicketDetails  ETD With(NOLOCK)  ON ETA.EventTicketDetailId = ETD.Id Where ETA.IsEnabled = 1 AND ETD.IsEnabled = 1 AND Isnull(Price, 0) <> 0 AND ETD.EventDetailId = ED.Id) as MaxPrice, " +
                                                       "ISNULL((Select TOP 1 AVG(Points) From Ratings R With(NOLOCK) Where R.EventId = E.Id),0) as Rating, " +
                                                       "ISNULL((Select TOP 1 CurrencyId From EventTicketAttributes ETA With(NOLOCK) INNER JOIN EventTicketDetails ETD With(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id Where ETA.IsEnabled = 1 AND ETD.IsEnabled = 1 AND  ETD.EventDetailId = ED.Id), 0) as CurrencyId, " +
                                                       "ISNULL((Select TOP 1 CT.Code From EventTicketAttributes ETA With(NOLOCK) INNER JOIN EventTicketDetails ETD With(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id INNER JOIN CurrencyTypes CT ON CT.Id = ETA.CurrencyId Where ETA.IsEnabled = 1 AND ETD.IsEnabled = 1 AND  ETD.EventDetailId = ED.Id),'None') as Currency, " +
                                                       " E.Description as EventDescription, V.Name as Venue, LED.EventStartDateTime as InteractivityStartDateTime, " +
                                                       " V.Latitude as Latitude, " +
                                                       " V.Longitude as Longitude, " +
                                                       " V.AddressLineOne as AddressLineOne, " +
                                                       " V.AddressLineTwo as AddressLineTwo " +
                                                       " FROM Eventcategorymappings A With(NOLOCK)  " +
                                                       " Left Outer Join Events E With(NOLOCK) ON A.EventId = E.Id  " +
                                                       " Inner Join EventSiteIdMappings ESD With(NoLOck) ON ESD.EventId=E.Id  " +
                                                       " Inner Join EventCategories EC With(NOLOCK) ON EC.Id = A.EventCategoryId  " +
                                                       " Inner Join EventSources ES With(NOLOCK) ON ES.Id = E.EventSourceId " +
                                                       " Inner Join Eventdetails ED With(NOLOCK) on E.id = ED.eventid " +
                                                       " Inner Join Venues V With(NOLOCK) on ED.venueid = V.id " +
                                                       " Inner Join Cities C With(NOLOCK) on V.cityid = C.id " +
                                                       " Inner Join States S With(NOLOCK) on C.stateid = S.id " +
                                                       " Left Outer Join LiveEventDetails LED With(NOLOCK) ON LED.EventId = E.Id  " +
                                                       " Inner Join Countries Co With(NOLOCK) on S.countryid = Co.id ";
        }

        public string GetDistinctEventScript()
        {
            return "Select DISTINCT E.Id as Id, (Select TOP 1 Name From Events where Id = E.Id) As Name,(Select TOP 1 AltId From Events where Id = E.Id) As AltId, (Select TOP 1 Description From Events where Id = E.Id) As EventDescription, (select TOP 1 Id from EventCategories With(NOLOCK) where Id In(Select TOP 1 EC.EventCategoryId FROM EventCategoryMappings ECM With(NOLOCK) Inner Join EventCategories EC With(NOLOCK) ON EC.Id = ECM.EventCategoryId WHERE ECM.EventId = E.ID)) as ParentCategoryId, (select TOP 1 DisplayName from EventCategories With(NOLOCK) where Id In(Select TOP 1 EC.EventCategoryId FROM EventCategoryMappings ECM With(NOLOCK) Inner Join EventCategories EC With(NOLOCK) ON EC.Id = ECM.EventCategoryId WHERE ECM.EventId = E.ID)) as ParentCategory,(Select TOP 1 EC.Id FROM EventCategoryMappings ECM With(NOLOCK) Inner Join EventCategories EC With(NOLOCK) ON EC.Id = ECM.EventCategoryId WHERE ECM.EventId = E.ID) as EventCategoryId, (Select TOP 1 EC.DisplayName FROM EventCategoryMappings ECM With(NOLOCK) Inner Join EventCategories EC With(NOLOCK) ON EC.Id = ECM.EventCategoryId WHERE ECM.EventId = E.ID) as Category, '/place/' + (select  slug from EventCategories where Id In(Select TOP 1 EC.EventCategoryId FROM EventCategoryMappings ECM With(NOLOCK) Inner Join EventCategories EC With(NOLOCK) ON EC.Id = ECM.EventCategoryId WHERE ECM.EventId = E.ID)) + '/' + E.slug + '/' + (Select TOP 1 EC.Slug FROM EventCategoryMappings ECM With(NOLOCK) Inner Join EventCategories EC With(NOLOCK) ON EC.Id = ECM.EventCategoryId WHERE ECM.EventId = E.ID) as Url, (Select TOP 1 Id From Cities With(NOLOCK) where Id= C.Id) As CityId, (Select TOP 1 Name From Cities With(NOLOCK) where Id = C.Id) As CityName, (Select TOP 1 Id From States With(NOLOCK) where Id = S.Id) As StateId,(Select TOP 1 Name From States With(NOLOCK) where Id = S.Id) As StateName,(Select TOP 1 Id From Countries With(NOLOCK) where Id = Co.Id) As CountryId,(Select TOP 1 Name From Countries With(NOLOCK) where Id = Co.Id) As CountryName FROM Events E With(NOLOCK)" +
              " Inner Join Eventdetails ED With(NOLOCK) on E.id = ED.eventid" +
              " Inner Join Venues V With(NOLOCK) on ED.venueid = V.id" +
              " Inner Join Cities C With(NOLOCK) on V.cityid = C.id" +
              " Inner Join States S With(NOLOCK) on C.stateid = S.id" +
              " Inner Join Countries Co With(NOLOCK) on S.countryid = Co.ID ";
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCategoryPage(List<int> subcategoryIds)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isfeel=1 AND E.isenabled=1 AND A.eventcategoryId in @Ids order by ESD.SortOrder "
                                                        , new
                                                        {
                                                            Ids = subcategoryIds.ToArray(),
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCountry(List<int> subcategoryIds, int countryId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isfeel=1 AND E.isenabled=1 AND A.eventcategoryId in @Ids AND Co.Id = @countryIds order by ESD.SortOrder "
                                                        , new
                                                        {
                                                            Ids = subcategoryIds.ToArray(),
                                                            countryIds = countryId
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByState(List<int> subcategoryIds, int stateId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isfeel=1 AND E.isenabled=1 AND A.eventcategoryId in @Ids AND S.Id = @stateIds order by ESD.SortOrder "
                                                        , new
                                                        {
                                                            Ids = subcategoryIds.ToArray(),
                                                            stateIds = stateId
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCity(List<int> subcategoryIds, int cityId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isfeel=1 AND E.isenabled=1 AND A.eventcategoryId in @Ids AND C.Id = @cityIds order by ESD.SortOrder "
                                                        , new
                                                        {
                                                            Ids = subcategoryIds.ToArray(),
                                                            cityIds = cityId
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCountry(int CountryId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isfeel=1 AND E.isenabled=1 AND Co.Id = @Id order by ESD.SortOrder "
                                                        , new
                                                        {
                                                            Id = CountryId
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByState(int StateId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isfeel=1 AND E.isenabled=1 AND S.Id = @Id order by ESD.SortOrder "
                                                        , new
                                                        {
                                                            Id = StateId
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlaceDetailsByCity(int CityId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isfeel=1 AND E.isenabled=1 AND C.Id = @Id order by ESD.SortOrder "
                                                        , new
                                                        {
                                                            Id = CityId
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllPlacesForAlgolia(bool isFeel)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetDistinctEventScript() +
                                                        "Where E.isenabled=1 AND e.IsFeel = @IsFeel AND Ed.Isenabled=1 order by E.Id ", new
                                                        {
                                                            IsFeel = isFeel
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetPlaceForAlgolia(long eventId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetDistinctEventScript() +
                                                        "Where E.isenabled=1 AND e.Id =" + eventId, null, GetCurrentTransaction());
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllBySlug(string slug)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.Slug=@Slug ", new
                                                        {
                                                            Slug = slug
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetAllItineraryPlaces(List<int> cityIds, List<long> categotyIds)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isenabled=1 AND C.Id IN @Ids AND E.IsFeel = 1 AND A.eventcategoryId IN @eventCategoryIds  ", new
                                                        {
                                                            Ids = cityIds,
                                                            eventCategoryIds = categotyIds
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.PlaceDetail> GetItinerarySearchPlaces(int eventId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.PlaceDetail>(GetScript() +
                                                        "Where E.isenabled=1 AND E.IsFeel = 1 AND E.Id = @Ids ", new
                                                        {
                                                            Ids = eventId
                                                        });
        }

        public IEnumerable<Event> GetBOEventByEventIds(IEnumerable<long> eventIds)
        {
            var eventList = GetAll(statement => statement
                           .Where($"{nameof(Event.Id):C} IN @Ids")
                           .WithParameters(new { Ids = eventIds }));
            return eventList;
        }

        public IEnumerable<Event> GetTMSEventByEventIds(IEnumerable<long> eventIds)
        {
            var eventList = GetAll(statement => statement
                           .Where($"{nameof(Event.Id):C} IN @Ids")
                           .WithParameters(new { Ids = eventIds }));
            return eventList;
        }

        public IEnumerable<Event> GetAllByDateEvents(DateTime fromDate)
        {
            var eventList = GetAll(s => s.Where($"{nameof(Event.CreatedUtc):C} > @CreatedUtc AND IsFeel=0")
                           .WithParameters(new { CreatedUtc = fromDate }));
            return eventList;
        }

        public IEnumerable<Event> GetByEventNameAndSourceId(string eventName, EventSource eventSource)
        {
            return GetAll(s => s.Where($"{nameof(Event.Name):C} = @EventName AND {nameof(Event.EventSourceId):C} = @Source AND IsEnabled=1")
                .WithParameters(new { EventName = eventName, Source = eventSource })
            );
        }

        public IEnumerable<StripeConnectMaster> EventStripeConnectMaster(long transactionId)
        {
            var stripeConnectMaster = GetCurrentConnection().Query<StripeConnectMaster>("select distinct t.Id as TransactionID, t.GrossTicketAmount, t.DeliveryCharges, t.ConvenienceCharges, t.ServiceCharge, e.Id as EventId, escm.IsEnabled, Escm.StripeConnectAccountID, td.TotalTickets,td.PricePerTicket,td.id as TransactionDetailId, Escm.ExtraCommisionFlat, Escm.ExtraCommisionPercentage, ed.StartDateTime, ed.EndDateTime, Escm.ChargebackHoldFlat, Escm.ChargebackHoldPercentage, t.CreatedBy, Escm.PayoutDaysOffset, Escm.ChargebackDaysOffset from Transactions t with (NOLOCK) inner join TransactionDetails td with (NOLOCK) on td.TransactionId=t.id inner join EventTicketAttributes eta with (NOLOCK) on td.EventTicketAttributeId = eta.Id inner join EventTicketDetails etd with (NOLOCK) on eta.EventTicketDetailId = etd.Id inner join EventDetails ed with (NOLOCK) on etd.EventDetailId = ed.Id inner join Events e with (NOLOCK) on ed.EventId = e.Id inner join EventStripeConnectMasters escm with (NOLOCK) on e.Id = escm.EventId where t.id=" + transactionId.ToString(), null, GetCurrentTransaction());
            return stripeConnectMaster;
        }

        public IEnumerable<LiveOnlineTransactionDetailResponseModel> GetMyFeelDetails(List<long> eventIds)
        {
            return GetCurrentConnection().Query<LiveOnlineTransactionDetailResponseModel>("" +
                "select distinct e.Id as EventId, ecm.EventcategoryId, e.Name, ec.displayName as SubCategoryDisplayName, " +
                " ed.StartDateTime," +
                " e.createdBy as CreatorAltId, " +
                "(select  DisplayName from EventCategories With(NOLOCK) where Id In(select Distinct EventCategoryId from EventCategories where Id=ec.Id) ) as parentCategoryName, " +
                "(select  Id from EventCategories With(NOLOCK) where Id In(select Distinct EventCategoryId from EventCategories where Id=ec.Id) ) as ParentCategoryId  " +
                " from events e with(NOLOCK) inner join eventdetails ed with(NOLOCK)" +
                " on ed.eventId = e.Id  " +
                " inner join eventcategorymappings ecm with(NOLOCK) on ecm.eventid = e.id " +
                " inner join eventcategories ec with(NOLOCK) on ec.Id = ecm.EventcategoryId" +
                " where e.id IN @Ids", new
                {
                    Ids = eventIds
                });
        }

        public string GetMyFeelsScript(bool isTicketSoldDetails)
        {
            return "" +
                "Select E.Id as Id, E.IsTokenize as IsTokenize, E.Name AS Name, E.altId as AltId, E.slug as Slug, E.IsEnabled as IsEnabled, E.EventStatusId, ESD.CompletedStep as CompletedStep, E.CreatedUtc as EventStartDateTime, Ed.StartDateTime as EventStartDateTime,Ed.EndDateTime as EventEndDateTime, EA.TimeZoneAbbreviation as TimeZoneAbbrivation,EA.TimeZone as TimeZoneOffset,  " +
                " E.MasterEventTypeId as MasterEventType, E.CreatedUtc as EventCreatedDateTime, usr.RolesId as RoleId, usr.Email as UserEmail, Ec.displayname as SubCategory, '/place/'+ (select  slug from EventCategories where Id In(select Distinct EventCategoryId from EventCategories where Id=EC.Id) ) +'/' + E.slug + '/' + Ec.slug as EventUrl " +
               (isTicketSoldDetails ? ", (select  DisplayName from EventCategories With(NOLOCK) where Id In(select Distinct EventCategoryId from EventCategories where Id=EC.Id) ) as ParentCategory, " +
               " ISNULL((Select TOP 1 SUM(AvailableTicketForSale) From EventTicketAttributes ETA  With(NOLOCK) INNER JOIN EventTicketDetails  ETD With(NOLOCK)  ON ETA.EventTicketDetailId = ETD.Id Where ETA.IsEnabled = 1 AND ETD.IsEnabled = 1 AND ETD.EventDetailId = ED.Id),0) as TicketForSale, " +
                " ISNULL((Select TOP 1 SUM(T.TotalTickets) From transactiondetails TD  With(NOLOCK) INNER JOIN EventTicketAttributes  ETA With(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id INNER JOIN EventTicketDetails  ETD With(NOLOCK)  ON ETA.EventTicketDetailId = ETD.Id  INNER JOIN Transactions  T With(NOLOCK)  ON T.Id = Td.TransactionId Where T.TransactionStatusId=8 AND ETA.IsEnabled = 1 AND ETD.IsEnabled = 1 AND ETD.EventDetailId = ED.Id),0) as SoldTicket " : " ") +
                " FROM Eventcategorymappings A With(NOLOCK) " +
                " Left Outer Join Events E With(NOLOCK) ON A.EventId = E.Id " +
                " Inner Join EventCategories EC With(NOLOCK) ON EC.Id = A.EventCategoryId  " +
                " left outer Join EventStepDetails ESD With(NOLOCK) ON ESD.EventId = E.Id " +
                " Left Outer Join Eventdetails ED With(NOLOCK) on E.id = ED.eventid " +
                " Left Outer Join Users usr With(NOLOCK) on usr.AltId = E.createdBy " +
                " Left Outer Join EventAttributes EA With(NOLOCK) ON EA.EventDetailId = ED.ID ";
        }

        public IEnumerable<FIL.Contracts.Models.Creator.MyFeel> GetMyFeels(Guid AltId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.Creator.MyFeel>(GetMyFeelsScript(true) +
                "Where E.createdBy = @CreatedBy order by  E.CreatedUtc desc", new
                {
                    CreatedBy = AltId.ToString()
                });
        }

        public IEnumerable<FIL.Contracts.Models.Creator.MyFeel> GetApproveModerateFeels(bool IsEnabled)
        {
            var feels = new List<FIL.Contracts.Models.Creator.MyFeel>();
            var feelData = GetCurrentConnection().QueryMultiple("FAP_Approve_Moderate", new { IsEnabled = IsEnabled }, commandType: CommandType.StoredProcedure);
            feels = feelData.Read<FIL.Contracts.Models.Creator.MyFeel>().ToList();
            return feels;
        }
    }
}