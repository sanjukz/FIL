using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation.Zsuite;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation.Zsuite
{
    public class ZsuiteEventCommandHandler : BaseCommandHandlerWithResult<ZEventCommand, ZEventCommandResult>
    {
        private readonly IEventRepository _eventRepository;

        public ZsuiteEventCommandHandler(IEventRepository eventRepository, IMediator mediator)
            : base(mediator)
        {
            _eventRepository = eventRepository;
        }

        protected override Task<ICommandResult> Handle(ZEventCommand command)
        {
            if (!command.IsEditEvent)
            {
                try
                {
                    ZEventCommandResult ZEventDataResult = new ZEventCommandResult();
                    var eventData = new Event
                    {
                        AltId = Guid.NewGuid(),
                        Name = command.Name,
                        TermsAndConditions = command.TermsAndConditions == null ? String.Empty : command.TermsAndConditions,
                        ClientPointOfContactId = 1,
                        IsPublishedOnSite = true,
                        Description = command.Description,
                        EventCategoryId = command.EventCategoryId,
                        EventTypeId = command.EventType,
                        MetaDetails = command.MetaDetails,
                        ModifiedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        EventSourceId = EventSource.None,
                        IsFeel = false,
                        IsEnabled = false,
                        Slug = command.Name.ToLower().Replace(' ', '-'),
                        IsCreatedFromFeelAdmin = false,
                        IsDelete = false
                    };
                    FIL.Contracts.DataModels.Event result = _eventRepository.Save(eventData);
                    ZEventDataResult.Id = result.Id;
                    ZEventDataResult.AltId = result.AltId;
                    return Task.FromResult<ICommandResult>(ZEventDataResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    var place = _eventRepository.GetByEventId((long)command.Id);
                    ZEventCommandResult ZEventDataResult = new ZEventCommandResult();
                    place.Name = command.Name;
                    place.TermsAndConditions = command.TermsAndConditions;
                    place.Description = command.Description;
                    place.EventCategoryId = command.EventCategoryId;
                    place.EventTypeId = command.EventType;
                    place.MetaDetails = command.MetaDetails;
                    place.ModifiedBy = command.ModifiedBy;
                    place.Slug = command.Name.ToLower().Replace(' ', '-');

                    FIL.Contracts.DataModels.Event result = _eventRepository.Save(place);
                    ZEventDataResult.Id = result.Id;
                    ZEventDataResult.AltId = result.AltId;
                    return Task.FromResult<ICommandResult>(ZEventDataResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}