using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCategoryMapping;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCategoryMapping
{
    public class EventCategoryMappingCommandHandler : BaseCommandHandler<EventCategoryMappingCommand>
    {
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IMediator _mediator;

        public EventCategoryMappingCommandHandler(IEventCategoryMappingRepository eventCategoryMappingRepository, IMediator mediator) : base(mediator)
        {
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(EventCategoryMappingCommand command)
        {
            //delete old and add new mappings agains particular eventid
            var entries = _eventCategoryMappingRepository.GetByEventId(command.Eventid);
            foreach (var entry in entries)
            {
                _eventCategoryMappingRepository.Delete(entry);
            }

            var eventcatmapping = new FIL.Contracts.DataModels.EventCategoryMapping
            {
                Id = command.Id,
                EventId = command.Eventid,
                EventCategoryId = command.Categoryid,
                IsEnabled = command.Isenabled,
                CreatedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                ModifiedBy = command.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow
            };
            _eventCategoryMappingRepository.Save(eventcatmapping);
        }
    }
}