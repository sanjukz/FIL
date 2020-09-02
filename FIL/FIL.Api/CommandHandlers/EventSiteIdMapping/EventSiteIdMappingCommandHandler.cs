using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventSiteIdMapping;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventSiteIdMapping
{
    public class EventSiteIdMappingCommandHandler : BaseCommandHandler<EventSiteIdMappingCommand>
    {
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IMediator _mediator;

        public EventSiteIdMappingCommandHandler(IEventSiteIdMappingRepository eventSiteIdMappingRepository, IMediator mediator) : base(mediator)
        {
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(EventSiteIdMappingCommand command)
        {
            var entries = _eventSiteIdMappingRepository.GetAllByEventId(command.EventId);
            foreach (var entry in entries)
            {
                _eventSiteIdMappingRepository.Delete(entry);
            }

            var last = _eventSiteIdMappingRepository.GetAll().OrderByDescending(p => p.CreatedUtc).FirstOrDefault();
            if (last != null)
            {
                var eventsiteidmapping = new FIL.Contracts.DataModels.EventSiteIdMapping
                {
                    Id = command.Id,
                    EventId = command.EventId,
                    SiteId = command.SiteId,
                    SortOrder = Convert.ToInt16(last.SortOrder + 1),
                    IsEnabled = true,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    ModifiedBy = command.ModifiedBy,
                    UpdatedUtc = DateTime.UtcNow
                };
                _eventSiteIdMappingRepository.Save(eventsiteidmapping);
            }
        }
    }
}