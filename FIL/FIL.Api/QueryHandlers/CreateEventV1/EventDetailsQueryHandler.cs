using FIL.Api.Repositories;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class EventDetailsQueryHandler : IQueryHandler<EventDetailsQuery, EventDetailsQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventCategoryRepository _eventCategory;

        public EventDetailsQueryHandler(IEventRepository eventRepository,
            IEventCategoryRepository eventCategory,
              IEventCategoryMappingRepository eventCategoryMappingRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventCategory = eventCategory;
        }

        public EventDetailsQueryResult Handle(EventDetailsQuery query)
        {
            try
            {
                var currentEvent = _eventRepository.Get(query.EventId);
                if (currentEvent == null)
                {
                    return new EventDetailsQueryResult { Success = true };
                }
                currentEvent.MetaDetails = currentEvent.MetaDetails == "NA" ? null : currentEvent.MetaDetails;
                var eventCategoryMappings = _eventCategoryMappingRepository.GetByEvent(currentEvent.Id);
                var eventCategory = _eventCategory.Get(eventCategoryMappings.EventCategoryId);
                var parentCategory = _eventCategory.Get(eventCategory.EventCategoryId);
                FIL.Contracts.Models.CreateEventV1.EventDetailModel eventDetailModel = new FIL.Contracts.Models.CreateEventV1.EventDetailModel();
                eventDetailModel.EventId = currentEvent.Id;
                eventDetailModel.EventCategories = eventCategoryMappings.EventCategoryId.ToString();
                eventDetailModel.Slug = currentEvent.Slug;
                eventDetailModel.Name = currentEvent.Name;
                eventDetailModel.IsEnabled = currentEvent.IsEnabled;
                eventDetailModel.AltId = currentEvent.AltId;
                eventDetailModel.Description = currentEvent.Description;
                eventDetailModel.DefaultCategory = eventCategory.DisplayName;
                eventDetailModel.ItemsForViewer = !String.IsNullOrEmpty(currentEvent.MetaDetails) ? currentEvent.MetaDetails.Split("|").ToList() : new List<string>();
                eventDetailModel.ParentCategory = parentCategory.DisplayName;
                eventDetailModel.ParentCategoryId = parentCategory.Id;
                eventDetailModel.IsPrivate = currentEvent.IsTokenize;
                return new EventDetailsQueryResult
                {
                    Success = true,
                    IsValidLink = true,
                    IsDraft = false,
                    EventDetail = eventDetailModel
                };
            }
            catch (Exception e)
            {
                return new EventDetailsQueryResult { };
            }
        }
    }
}