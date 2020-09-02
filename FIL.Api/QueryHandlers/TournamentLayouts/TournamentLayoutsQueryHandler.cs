using FIL.Api.Repositories;
using FIL.Contracts.Queries.TournamentLayout;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TournamentLayouts
{
    public class TournamentLayoutsQueryHandler : IQueryHandler<TournamentEventQuery, TournamentLayoutEventQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly FIL.Logging.ILogger _logger;

        public TournamentLayoutsQueryHandler(IEventRepository eventRepository, FIL.Logging.ILogger logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public TournamentLayoutEventQueryResult Handle(TournamentEventQuery query)
        {
            try
            {
                var events = _eventRepository.GetAllTournamentEvents();

                return new TournamentLayoutEventQueryResult
                {
                    Events = AutoMapper.Mapper.Map<List<Contracts.Models.Event>>(events)
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new TournamentLayoutEventQueryResult
                {
                    Events = null
                };
            }
        }
    }
}