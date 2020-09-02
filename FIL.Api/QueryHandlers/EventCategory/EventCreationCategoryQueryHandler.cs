using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.QueryResults.EventCreation;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.EventCategory
{
    public class EventCreationCategoryQueryHandler : IQueryHandler<EventCreationCategoryQuery, EventCreationCategoryQueryResult>
    {
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public EventCreationCategoryQueryHandler(IEventCategoryRepository eventCategoryRepository)
        {
            _eventCategoryRepository = eventCategoryRepository;
        }

        public EventCreationCategoryQueryResult Handle(EventCreationCategoryQuery query)
        {
            var eventResult = _eventCategoryRepository.GetActiveEventCategory().OrderBy(o => o.Order);
            return new EventCreationCategoryQueryResult
            {
                EventCategories = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventCategory>>(eventResult)
            };
        }
    }
}