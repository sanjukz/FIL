using FIL.Api.Providers.EventManagement;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class EventRecurranceQueryHandler : IQueryHandler<EventRecurranceScheduleQuery, EventRecurranceScheduleQueryResult>
    {
        private readonly IGetScheduleDetailProvider _getScheduleDetailProvider;

        public EventRecurranceQueryHandler(IGetScheduleDetailProvider getScheduleDetailProvider)
        {
            _getScheduleDetailProvider = getScheduleDetailProvider;
        }

        public EventRecurranceScheduleQueryResult Handle(EventRecurranceScheduleQuery query)
        {
            try
            {
                var eventSchedules = _getScheduleDetailProvider.GetScheduleDetails(query.EventId, query.StartDate, query.EndDate);
                return new EventRecurranceScheduleQueryResult
                {
                    Success = true,
                    IsDraft = false,
                    IsValidLink = true,
                    EventRecurranceScheduleModel = eventSchedules
                };
            }
            catch (Exception e)
            {
                return new EventRecurranceScheduleQueryResult
                {
                    IsValidLink = true,
                    IsDraft = false
                };
            }
        }
    }
}