using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventCategoryMappingRepository : IOrmRepository<EventCategoryMapping, EventCategoryMapping>
    {
        EventCategoryMapping Get(int id);

        IEnumerable<EventCategoryMapping> GetByEventId(long eventId);

        IEnumerable<EventCategoryMapping> GetByEventIds(IEnumerable<long> eventIds);

        IEnumerable<EventCategoryMapping> GetByEventCategoryIds(IEnumerable<long> eventCatIds);

        IEnumerable<EventCategoryMapping> GetByEventCategoryId(int eventCategoryId, string search, bool isSearch);

        IEnumerable<EventCategoryMapping> GetByParentEventCategoryId(int eventCategoryId, string search, bool isSearch,
            List<int> eventCategories = null, bool? isAllOnline = false);

        EventCategoryMapping GetByEvent(long eventId);

        EventCategoryMapping GetByEventIdAndEventCategoryId(long eventId, int eventCategoryId);
    }

    public class EventCategoryMappingRepository : BaseOrmRepository<EventCategoryMapping>, IEventCategoryMappingRepository
    {
        public EventCategoryMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventCategoryMapping Get(int id)
        {
            return Get(new EventCategoryMapping { Id = id });
        }

        public IEnumerable<EventCategoryMapping> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<EventCategoryMapping> GetByEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventCategoryMapping.EventId):C} = @EventId")
                  .WithParameters(new { EventId = eventId }));
        }

        public IEnumerable<EventCategoryMapping> GetByEventIds(IEnumerable<long> eventIds)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventCategoryMapping.EventId):C} IN @Ids")
                    .WithParameters(new { Ids = eventIds }));
        }

        public EventCategoryMapping GetByEventIdAndEventCategoryId(long eventId, int eventCategoryId)
        {
            return GetAll(s => s.Where($"{nameof(EventCategoryMapping.EventId):C} = @EventId AND {nameof(EventCategoryMapping.EventCategoryId)} = @EventCategoryId")
                .WithParameters(new { EventId = eventId, EventCategoryId = eventCategoryId })
            ).FirstOrDefault();
        }

        public IEnumerable<EventCategoryMapping> GetByEventCategoryIds(IEnumerable<long> eventCatIds)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventCategoryMapping.EventCategoryId):C} IN @Ids")
                    .WithParameters(new { Ids = eventCatIds }));
        }

        public IEnumerable<EventCategoryMapping> GetByEventCategoryId(int eventCategoryId, string search, bool isSearch)
        {
            if (!isSearch)
            {
                return GetAll(s => s.Where($"{nameof(EventCategoryMapping.EventCategoryId):C} = @EventCategoryId")
                      .WithParameters(new { EventCategoryId = eventCategoryId }));
            }
            else
            {
                {
                    return GetCurrentConnection().Query<EventCategoryMapping>("SELECT DISTINCT ecm.*, C.Name, S.Name, cou.Name FROM EventCategoryMappings ecm WITH(NOLOCK) " +
                                                                                "INNER JOIN EventDetails ed WITH(NOLOCK) ON ed.EventId = ecm.EventId " +
                                                                                "INNER JOIN Venues v WITH(NOLOCK) ON ed.VenueId = v.Id " +
                                                                                "INNER JOIN Cities c WITH(NOLOCK) ON v.CityId = c.Id " +
                                                                                "INNER JOIN States s WITH(NOLOCK) ON c.StateId = s.Id " +
                                                                                "INNER JOIN Countries cou WITH(NOLOCK) ON s.CountryId = cou.Id " +
                                                                                "WHERE ecm.EventCategoryId = @EventCategoryId AND ecm.IsEnabled = 1 " +
                                                                                "AND( " +
                                                                                "c.Name IN(SELECT KeyWord FROM SplitString(@SearchText, ',')) " +
                                                                                "OR " +
                                                                                "s.Name IN(SELECT KeyWord FROM SplitString(@SearchText, ',')) " +
                                                                                "OR " +
                                                                                "cou.Name IN(SELECT KeyWord FROM SplitString(@SearchText, ',')) " +
                                                                                ") ", new
                                                                                {
                                                                                    EventCategoryId = eventCategoryId,
                                                                                    SearchText = search
                                                                                });
                }
            }
        }

        public String GetScript(bool? isAllOnline = false)
        {
            return "SELECT DISTINCT ecm.*, C.Name, S.Name, cou.Name FROM EventCategoryMappings ecm WITH(NOLOCK) " +
                                                                                "INNER JOIN EventCategories ec WITH(NOLOCK) ON ecm.EventCategoryId = ec.Id " +
                                                                                "INNER JOIN EventDetails ed WITH(NOLOCK) ON ed.EventId = ecm.EventId " +
                                                                                "INNER JOIN Venues v WITH(NOLOCK) ON ed.VenueId = v.Id " +
                                                                                "INNER JOIN Cities c WITH(NOLOCK) ON v.CityId = c.Id " +
                                                                                "INNER JOIN States s WITH(NOLOCK) ON c.StateId = s.Id " +
                                                                                "INNER JOIN Countries cou WITH(NOLOCK) ON s.CountryId = cou.Id " +
                                                                                "WHERE ec.EventCategoryId" + (isAllOnline != null && (bool)isAllOnline ? "IN @EventCategoryId" : " =@EventCategoryId") + " AND ecm.IsEnabled = 1 " +
                                                                                "AND( " +
                                                                                "c.Name IN(SELECT KeyWord FROM SplitString(@SearchText, ',')) " +
                                                                                "OR " +
                                                                                "s.Name IN(SELECT KeyWord FROM SplitString(@SearchText, ',')) " +
                                                                                "OR " +
                                                                                "cou.Name IN(SELECT KeyWord FROM SplitString(@SearchText, ',')) ";
        }

        public IEnumerable<EventCategoryMapping> GetByParentEventCategoryId(
            int eventCategoryId,
            string search,
            bool isSearch,
            List<int> eventCategories = null,
            bool? isAllOnline = false)
        {
            //return GetAll(s => s.Where($"{nameof(EventCategoryMapping.EventCategoryId):C} IN @EventCategoryId")
            //      .WithParameters(new { EventCategoryId = eventCategoryId }));
            if (isAllOnline != null && (bool)isAllOnline)
            {
                return GetCurrentConnection().Query<EventCategoryMapping>("SELECT ecm.* FROM EventCategoryMappings ecm WITH (NOLOCK) " +
                                                                           "INNER JOIN EventCategories ec WITH (NOLOCK) ON ecm.EventCategoryId = ec.Id " +
                                                                           "WHERE ec.EventCategoryId IN @EventCategoryId", new
                                                                           {
                                                                               EventCategoryId = eventCategories.ToArray(),
                                                                               SearchText = search
                                                                           });
            }
            else if (!isSearch)
            {
                return GetCurrentConnection().Query<EventCategoryMapping>("SELECT ecm.* FROM EventCategoryMappings ecm WITH (NOLOCK) " +
                                                                           "INNER JOIN EventCategories ec WITH (NOLOCK) ON ecm.EventCategoryId = ec.Id " +
                                                                           "WHERE ec.EventCategoryId = @EventCategoryId", new
                                                                           {
                                                                               EventCategoryId = eventCategoryId
                                                                           });
            }
            else
            {
                return GetCurrentConnection().Query<EventCategoryMapping>(GetScript() +
                                                                            ") ", new
                                                                            {
                                                                                EventCategoryId = eventCategoryId,
                                                                                SearchText = search
                                                                            });
            }
        }

        public EventCategoryMapping GetByEvent(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventCategoryMapping.EventId):C} = @EventId")
                .WithParameters(new { EventId = eventId })
            ).FirstOrDefault();
        }
    }
}