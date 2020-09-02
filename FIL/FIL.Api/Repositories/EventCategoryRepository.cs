using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventCategoryRepository : IOrmRepository<EventCategory, EventCategory>
    {
        EventCategory Get(int id);

        EventCategory GetBySlug(string slug);

        long GetByName(string name);

        IEnumerable<EventCategory> GetActiveEventCategory();

        IEnumerable<EventCategory> GetByIds(IEnumerable<int> ids);

        EventCategory GetByNameAndNonFeel(string name, bool isFeel);

        EventCategory GetByName(string name, string over);

        IEnumerable<EventCategory> GetByEventCategoryId(int eventCategoryId);

        IEnumerable<EventCategory> GetCategoriesByLocation(List<int> cityIds);
    }

    public class EventCategoryRepository : BaseOrmRepository<EventCategory>, IEventCategoryRepository
    {
        public EventCategoryRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventCategory Get(int id)
        {
            return Get(new EventCategory { Id = id });
        }

        public EventCategory GetBySlug(string slug)
        {
            return GetAll(s => s.Where($"{nameof(EventCategory.Slug):C} = @Slug")
                .WithParameters(new { Slug = slug })
            ).FirstOrDefault();
        }

        public long GetByName(string name)
        {
            try
            {
                return GetAll(s => s.Where($"{nameof(EventCategory.DisplayName):C} = @DisplayName")
                 .WithParameters(new { DisplayName = name })
            ).FirstOrDefault().Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EventCategory GetByName(string name, string over)
        {
            try
            {
                return GetAll(s => s.Where($"{nameof(EventCategory.DisplayName):C} = @DisplayName")
                 .WithParameters(new { DisplayName = name })
            ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EventCategory GetByNameAndNonFeel(string name, bool isFeel)
        {
            return GetAll(s => s.Where($"{nameof(EventCategory.Category):C} = @Category AND {nameof(EventCategory.IsFeel)} = @IsFeel")
                .WithParameters(new { Category = name, IsFeel = isFeel })
            ).FirstOrDefault();
        }

        public IEnumerable<EventCategory> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<EventCategory> GetActiveEventCategory()
        {
            return GetAll(null).Where(p => p.IsEnabled == true);
        }

        public IEnumerable<EventCategory> GetByIds(IEnumerable<int> ids)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventCategory.Id):C} IN @Ids")
                    .WithParameters(new { Ids = ids }));
        }

        public IEnumerable<EventCategory> GetByEventCategoryId(int eventCategoryId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(EventCategory.EventCategoryId):C} = @Id")
                    .WithParameters(new { Id = eventCategoryId }));
        }

        public IEnumerable<EventCategory> GetCategoriesByLocation(List<int> cityIds)
        {
            var categoris = GetCurrentConnection().Query<int>("select distinct(ecm.EventCategoryId) from events e WITH(NOLOCK) " +
                "inner join EventCategoryMappings ecm WITH(NOLOCK) on ecm.EventId = e.id " +
                "inner join EventDetails ed WITH(NOLOCK) on ed.EventId = e.id " +
                "inner join Venues v WITH(NOLOCK) on v.Id = ed.VenueId " +
                "inner join Cities ct WITH(NOLOCK) on ct.Id = v.CityId " +
                                                        "Where E.isenabled=1 AND E.IsFeel = 1 AND Ct.Id IN @Ids ", new
                                                        {
                                                            Ids = cityIds.ToArray()
                                                        }).ToList();
            return GetAll(statement => statement
                   .Where($"{nameof(EventCategory.Id):C} IN @Ids")
                   .WithParameters(new { Ids = categoris }));
        }
    }
}