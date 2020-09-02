using FIL.Api.Repositories;
using FIL.Contracts.Queries.PublishEvent;
using FIL.Contracts.QueryResults.PublishEvent;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.Events
{
    public class PublishEventQueryHandler : IQueryHandler<PublishEventQuery, PublishEventQueryResult>
    {
        private readonly IEventRepository _eventRepository;

        public PublishEventQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public PublishEventQueryResult Handle(PublishEventQuery query)
        {
            var eventDataModel = _eventRepository.GetAllByDateEvents(DateTime.UtcNow.AddDays(-60));
            var eventModel = AutoMapper.Mapper.Map<List<Contracts.Models.Event>>(eventDataModel);
            return new PublishEventQueryResult
            {
                Events = eventModel,
            };
        }
    }
}