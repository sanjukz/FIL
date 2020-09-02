using FIL.Api.Repositories;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventsFeelQueryHandler : IQueryHandler<EventsFeelQuery, EventsFeelQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventGalleryImageRepository _eventGalleryImageRepository;

        public EventsFeelQueryHandler(IEventRepository eventRepository,
            IEventGalleryImageRepository eventGalleryImageRepository)
        {
            _eventRepository = eventRepository;
            _eventGalleryImageRepository = eventGalleryImageRepository;
        }

        public EventsFeelQueryResult Handle(EventsFeelQuery query)
        {
            List<FIL.Contracts.DataModels.Event> eventResult = new List<FIL.Contracts.DataModels.Event>();
            if (query.RoleId == 10)
            {
                eventResult = _eventRepository.GetAllFeelEvents(query.IsFeel).ToList();
            }

            if (query.RoleId != 10)
            {
                eventResult = _eventRepository.GetAllFeelEventsByCreatedBy(query.UserAltId).ToList();
            }
            return new EventsFeelQueryResult()
            {
                Events = eventResult.Select(p => new Event
                {
                    Id = p.Id,
                    AltId = p.AltId,
                    EventCategoryId = p.EventCategoryId,
                    EventTypeId = p.EventTypeId,
                    Name = p.Name,
                    Description = p.Description,
                    IsEnabled = p.IsEnabled,
                    IsFeel = p.IsFeel,
                    EventSourceId = p.EventSourceId,
                    IsPublishedOnSite = p.IsPublishedOnSite,
                    PublishedDateTime = p.PublishedDateTime,
                    ImagePath = GetImagePath(p.Id)
                }).ToList()
            };
        }

        private string GetImagePath(long id)
        {
            var img = _eventGalleryImageRepository.GetByEventId(Convert.ToInt32(id));
            if (img != null && img.Count() > 0)
            {
                return img.FirstOrDefault().AltId.ToString();
            }
            return "";
        }
    }
}