using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class EventAmenityCommandHandler : BaseCommandHandler<EventAmenityCommand>
    {
        private readonly IEventAmenityRepository _eventAmenityRepository;
        private readonly IMediator _mediator;

        public EventAmenityCommandHandler(IEventAmenityRepository eventAmenityRepository, IMediator mediator) : base(mediator)
        {
            _eventAmenityRepository = eventAmenityRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(EventAmenityCommand command)
        {
            var entries = _eventAmenityRepository.GetByEventId(command.EventId);
            foreach (var entry in entries)
            {
                _eventAmenityRepository.Delete(entry);
            }
            var eventAmenityresult = _eventAmenityRepository.Get(command.Id);
            var eventAmenity = new EventAmenity
            {
                AmenityId = command.AmenityId,
                Description = command.Description,
                EventId = command.EventId,
                IsEnabled = command.IsEnabled,
                UpdatedBy = command.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow,
                CreatedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                ModifiedBy = command.ModifiedBy,
                Id = command.Id
            };
            _eventAmenityRepository.Save(eventAmenity);
        }
    }
}