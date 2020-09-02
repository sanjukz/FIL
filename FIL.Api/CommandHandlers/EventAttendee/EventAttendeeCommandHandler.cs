using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventAttendee;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventAttendee
{
    public class EventAttendeeCommandHandler : BaseCommandHandler<EventAttendeeCommand>
    {
        private readonly IEventAttendeeDetailRepository _eventAttendeeDetailRepository;

        public EventAttendeeCommandHandler(IEventAttendeeDetailRepository eventAttendeeDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _eventAttendeeDetailRepository = eventAttendeeDetailRepository;
        }

        protected override async Task Handle(EventAttendeeCommand command)
        {
            var eventAttendee = new EventAttendeeDetail
            {
                TransactionId = command.TransactionId,
                TransactionDetailId = command.TransactionDetailId,
                EventFormFieldId = command.EventFormFieldId,
                Value = command.Value,
                AttendeeNumber = command.AttendeeNumber,
                CreatedBy = command.CreatedBy,
                ModifiedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
            };

            _eventAttendeeDetailRepository.Save(eventAttendee);
        }
    }
}