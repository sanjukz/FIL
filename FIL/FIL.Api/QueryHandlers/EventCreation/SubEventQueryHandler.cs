using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.QueryResults.EventCreation;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.EventCreation
{
    public class SubEventQueryHandler : IQueryHandler<SubEventDetailQuery, SubEventDetailQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;

        public SubEventQueryHandler(IEventDetailRepository eventDetailRepository)
        {
            _eventDetailRepository = eventDetailRepository;
        }

        public SubEventDetailQueryResult Handle(SubEventDetailQuery query)
        {
            var eventDataModel = _eventDetailRepository.GetSubEventByEventId(query.EventId);
            var eventDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventDetail>>(eventDataModel);
            return new SubEventDetailQueryResult
            {
                EventDetail = eventDetailModel
            };
        }
    }
}