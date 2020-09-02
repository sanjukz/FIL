using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class EventLearnMoreAttributeCommandHandler : BaseCommandHandler<EventLearnMoreAttributeCommand>
    {
        private readonly IEventLearnMoreAttributeRepository _eventLearnMoreAttributeRepository;
        private readonly IMediator _mediator;

        public EventLearnMoreAttributeCommandHandler(IEventLearnMoreAttributeRepository eventLearnMoreAttributeRepository, IMediator mediator) : base(mediator)
        {
            _eventLearnMoreAttributeRepository = eventLearnMoreAttributeRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(EventLearnMoreAttributeCommand command)
        {
            var eventLearnMoreAttributeresult = _eventLearnMoreAttributeRepository.Get(command.Id);
            var eventLearnMoreAttribute = new EventLearnMoreAttribute
            {
                Description = command.Description,
                EventId = command.EventId,
                IsEnabled = command.IsEnabled,
                UpdatedBy = command.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow,
                CreatedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                ModifiedBy = command.ModifiedBy,
                Id = command.Id,
                LearnMoreFeatureId = command.LearnMoreFeatureId,
                AltId = command.AltId,
                Image = "NA"
            };
            _eventLearnMoreAttributeRepository.Save(eventLearnMoreAttribute);
        }
    }
}