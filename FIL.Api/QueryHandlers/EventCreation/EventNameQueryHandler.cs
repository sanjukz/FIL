using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.QueryResults.EventCreation;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.EventCreation
{
    public class EventNameQueryHandler : IQueryHandler<EventNameQuery, EventNameQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly FIL.Logging.ILogger _logger;

        public EventNameQueryHandler(IEventRepository eventRepository, FIL.Logging.ILogger logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public EventNameQueryResult Handle(EventNameQuery query)
        {
            try
            {
                var events = _eventRepository.GetAllByDateEvents(DateTime.UtcNow.AddMonths(-3));

                return new EventNameQueryResult
                {
                    Events = AutoMapper.Mapper.Map<List<Contracts.Models.Event>>(events)
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new EventNameQueryResult
                {
                    Events = null
                };
            }
        }
    }
}