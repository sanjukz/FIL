using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventsQueryHandler : IQueryHandler<EventsQuery, EventsQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public EventsQueryHandler(IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
        }

        public EventsQueryResult Handle(EventsQuery query)
        {
            if (query.Channel == Channels.Feel)
            {
                var eventResult = _eventRepository.GetSEOEventsByProduct(query.Channel == Channels.Feel);

                var siteMap = eventResult.Select(eId =>
                {
                    var eventCategoryDataModel = _eventCategoryRepository.Get(eId.EventCategoryId);
                    var eventCategoryModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryDataModel);

                    if (eventCategoryModel != null)
                    {
                        var eventCategoryParentDataModel = _eventCategoryRepository.Get(eventCategoryModel.EventCategoryId);
                        var eventCategoryParentModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryParentDataModel);

                        if (eId.AltId != null && eventCategoryParentModel != null)
                        {
                            return new EventsSiteMap
                            {
                                EventAltId = eId.AltId,
                                URL = eventCategoryParentModel.DisplayName.ToString().Replace(" ", "-").ToLower() + "/" + eId.Slug.ToString() + "/" + eventCategoryModel.DisplayName.ToString().Replace(" ", "-").ToLower(),
                                Description = eId.Description
                            };
                        }
                        else
                        {
                            return new EventsSiteMap
                            {
                            };
                        }
                    }
                    else
                    {
                        return new EventsSiteMap
                        {
                        };
                    }
                });

                return new EventsQueryResult
                {
                    EventsURLs = siteMap.ToList()
                };
            }
            else
            {
                var eventResult = _eventRepository.GetSEOEventsByProduct(query.Channel == Channels.Feel);

                var siteMap = eventResult.Select(eId =>
                {
                    if (eId.AltId != null)
                    {
                        return new EventsSiteMap
                        {
                            EventAltId = eId.AltId
                        };
                    }
                    else
                    {
                        return new EventsSiteMap
                        {
                        };
                    }
                });

                return new EventsQueryResult
                {
                    EventsURLs = siteMap.ToList()
                };
            }
        }
    }
}