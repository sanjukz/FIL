using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.QueryResults.EventCreation;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.EventCreation
{
    public class GetEventsQueryHandler : IQueryHandler<GetEventQuery, GetEventQueryResult>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventsQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public GetEventQueryResult Handle(GetEventQuery query)
        {
            var user = _eventRepository.GetByAltIds(query.UserId);
            if (user == null)
            {
                return new GetEventQueryResult();
            }
            else
            {
                var eventModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Event>>(user).OrderByDescending(s => s.Id).ToList();
                return new GetEventQueryResult
                {
                    Event = eventModel,
                };
            }
        }
    }
}