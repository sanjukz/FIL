using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.QueryResults.EventCreation;

namespace FIL.Api.QueryHandlers.EventCreation
{
    public class EventQueryHandler : IQueryHandler<EventCreationQuery, EventCreationQueryResult>
    {
        private readonly IEventRepository _eventRepository;

        public EventQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public EventCreationQueryResult Handle(EventCreationQuery query)
        {
            var eventDataModel = _eventRepository.GetByAltId(query.EventAltId);
            var eventModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(eventDataModel);
            return new EventCreationQueryResult
            {
                Event = eventModel
            };
        }
    }
}