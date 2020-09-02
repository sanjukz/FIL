using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventAttendee;
using FIL.Contracts.QueryResults.EventAttendee;

namespace FIL.Api.QueryHandlers.EventAttendee
{
    public class EventAttendeeQueryHandler : IQueryHandler<EventAttendeeQuery, EventAttendeeQueryResult>
    {
        private readonly IEventAttendeeDetailRepository _eventAttendeeRepository;

        public EventAttendeeQueryHandler(IEventAttendeeDetailRepository eventAttendeeRepository)
        {
            _eventAttendeeRepository = eventAttendeeRepository;
        }

        public EventAttendeeQueryResult Handle(EventAttendeeQuery query)
        {
            FIL.Contracts.DataModels.EventAttendeeDetail eventAttendeeDetail = new FIL.Contracts.DataModels.EventAttendeeDetail();
            if (query.TransactionId != null)
            {
                eventAttendeeDetail = _eventAttendeeRepository.Get((long)query.TransactionId);
            }

            return new EventAttendeeQueryResult
            {
                TransactionId = eventAttendeeDetail.TransactionId,
                TransactionDetailId = eventAttendeeDetail.TransactionDetailId,
                EventFormFieldId = eventAttendeeDetail.EventFormFieldId,
                Value = eventAttendeeDetail.Value,
                AttendeeNumber = eventAttendeeDetail.AttendeeNumber,
            };
        }
    }
}