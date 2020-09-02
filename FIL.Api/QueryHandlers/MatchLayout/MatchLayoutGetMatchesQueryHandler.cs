using FIL.Api.Repositories;
using FIL.Contracts.Queries.MatchLayout;
using FIL.Contracts.QueryResult.MatchLayout;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.MatchLayout
{
    public class MatchLayoutGetMatchesQueryHandler : IQueryHandler<MatchLayoutGetMatchesQuery, MatchLayoutGetMatchesQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;

        public MatchLayoutGetMatchesQueryHandler(IEventDetailRepository eventDetailRepository)
        {
            _eventDetailRepository = eventDetailRepository;
        }

        public MatchLayoutGetMatchesQueryResult Handle(MatchLayoutGetMatchesQuery query)
        {
            var eventdetails = _eventDetailRepository.GetByPastEventIdAndVenueId(query.EventId, query.VenueId);

            return new MatchLayoutGetMatchesQueryResult
            {
                eventDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(eventdetails)
            };
        }
    }
}