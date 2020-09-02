using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventsCategoryFeelQueryHandler : IQueryHandler<EventsCategoryFeelQuery, EventsCategoryFeelQueryResult>
    {
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public EventsCategoryFeelQueryHandler(IEventCategoryRepository eventCategoryRepository)
        {
            _eventCategoryRepository = eventCategoryRepository;
        }

        public EventsCategoryFeelQueryResult Handle(EventsCategoryFeelQuery query)
        {
            var eventResult = _eventCategoryRepository.GetAll();
            List<EventCategoryData> categorydataList = new List<EventCategoryData>();
            foreach (var item in eventResult)
            {
                var description = new EventCategoryData();
                description.CategoryId = item.EventCategoryId;
                description.DisplayName = item.DisplayName;
                description.IsFeel = item.IsFeel;
                description.IsHomePage = item.IsHomePage;
                description.Order = item.Order;
                description.Slug = item.Slug;
                description.Value = item.Id;
                description.MasterEventTypeId = item.MasterEventTypeId;
                if (description.IsFeel)
                {
                    categorydataList.Add(description);
                }
            }
            return new EventsCategoryFeelQueryResult() { EventCategoryFeel = categorydataList };
        }
    }
}