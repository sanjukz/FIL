using FIL.Api.Repositories;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventTokenQueryHandler : IQueryHandler<EventTokenQuery, EventTokenQueryResult>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IEventTokenMappingRepository _eventTokenMappingRepository;
        private readonly IEventRepository _eventRepository;
        private readonly FIL.Logging.ILogger _logger;

        public EventTokenQueryHandler(ITokenRepository tokenRepository, IEventTokenMappingRepository eventTokenMappingRepository, IEventRepository eventRepository, FIL.Logging.ILogger logger)
        {
            _tokenRepository = tokenRepository;
            _eventRepository = eventRepository;
            _eventTokenMappingRepository = eventTokenMappingRepository;
            _logger = logger;
        }

        public EventTokenQueryResult Handle(EventTokenQuery query)
        {
            try
            {
                var token = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.Token>(_tokenRepository.GetByTokenId(query.AccessToken));
                if (token != null)
                {
                    var eventMapping = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.EventTokenMapping>(_eventTokenMappingRepository.GetByTokenId(token.Id));
                    if (eventMapping != null)
                    {
                        var events = _eventRepository.Get(eventMapping.EventId);
                        if (events != null)
                        {
                            return new EventTokenQueryResult
                            {
                                IsValid = true,
                                EventAltId = events.AltId
                            };
                        }
                        else
                        {
                            return new EventTokenQueryResult
                            {
                                IsValid = false,
                            };
                        }
                    }
                    else
                    {
                        return new EventTokenQueryResult
                        {
                            IsValid = false
                        };
                    }
                }
                else
                {
                    return new EventTokenQueryResult
                    {
                        IsValid = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new EventTokenQueryResult
                {
                    IsValid = false
                };
            }
        }
    }
}