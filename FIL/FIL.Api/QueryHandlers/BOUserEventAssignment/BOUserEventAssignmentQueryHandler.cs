using FIL.Api.Repositories;
using FIL.Contracts.Queries.BOUserEventAssignment;
using FIL.Contracts.QueryResults.BOUserEventAssignment;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.BOUserEventAssignment
{
    public class BOUserEventAssignmentQueryHandler : IQueryHandler<BOUserEventAssignmentQuery, BOUserEventAssignmentQueryResult>
    {
        private readonly IEventRepository _eventRepository;

        public BOUserEventAssignmentQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public BOUserEventAssignmentQueryResult Handle(BOUserEventAssignmentQuery query)
        {
            var eventDataModel = _eventRepository.GetAllZEvents();
            var eventModel = AutoMapper.Mapper.Map<List<Contracts.Models.Event>>(eventDataModel);

            return new BOUserEventAssignmentQueryResult
            {
                Events = eventModel,
            };
        }
    }
}