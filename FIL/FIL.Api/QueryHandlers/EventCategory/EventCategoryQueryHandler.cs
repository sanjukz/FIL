using FIL.Api.Repositories;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.EventCategory
{
    public class EventCategoryQueryHandler : IQueryHandler<EventCategoryQuery, EventCategoryQueryResult>
    {
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public EventCategoryQueryHandler(IEventCategoryRepository eventCategoryRepository)
        {
            _eventCategoryRepository = eventCategoryRepository;
        }

        public EventCategoryQueryResult Handle(EventCategoryQuery query)
        {
            if (query.Id == 0)
            {
                if (!string.IsNullOrWhiteSpace(query.Slug))
                {
                    var eventResult = _eventCategoryRepository.GetBySlug(query.Slug);
                    List<FIL.Contracts.Models.EventCategory> EventCategories = new List<Contracts.Models.EventCategory>();
                    EventCategories.Add(AutoMapper.Mapper.Map<FIL.Contracts.Models.EventCategory>(eventResult));
                    return new EventCategoryQueryResult
                    {
                        EventCategories = EventCategories
                    };
                }
                else
                {
                    var eventResult = _eventCategoryRepository.GetActiveEventCategory().OrderBy(o => o.Order);
                    return new EventCategoryQueryResult
                    {
                        EventCategories = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventCategory>>(eventResult)
                    };
                }
            }
            else
            {
                var eventResult = _eventCategoryRepository.Get(query.Id);
                List<FIL.Contracts.Models.EventCategory> EventCategories = new List<Contracts.Models.EventCategory>();
                EventCategories.Add(AutoMapper.Mapper.Map<FIL.Contracts.Models.EventCategory>(eventResult));
                return new EventCategoryQueryResult
                {
                    EventCategories = EventCategories
                };
            }
        }
    }
}