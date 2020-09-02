using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class SaveEventDetailCommandHandler : BaseCommandHandlerWithResult<EventDetailsCommand, EventDetailsCommandResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IStepProvider _stepProvider;
        private readonly ILogger _logger;

        public SaveEventDetailCommandHandler(
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
              IEventCategoryMappingRepository eventCategoryMappingRepository,
              IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _stepProvider = stepProvider;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        private void UpdateEventDetailName(EventDetailsCommand command)
        {
            var eventDetail = _eventDetailRepository.GetByEventId(command.EventDetail.EventId);
            if (eventDetail != null)
            {
                eventDetail.Name = command.EventDetail.Name;
                eventDetail.IsEnabled = true;
                eventDetail.CreatedUtc = eventDetail.Id != 0 ? eventDetail.CreatedUtc : DateTime.UtcNow;
                eventDetail.UpdatedUtc = eventDetail.Id != 0 ? DateTime.UtcNow : eventDetail.CreatedUtc;
                eventDetail.CreatedBy = eventDetail.Id != 0 ? eventDetail.CreatedBy : command.ModifiedBy;
                eventDetail.UpdatedBy = command.ModifiedBy;
                eventDetail.ModifiedBy = command.ModifiedBy;
                _eventDetailRepository.Save(eventDetail);
            }
        }

        private void SaveEventCategoryMapping(EventDetailsCommand command)
        {
            var entries = _eventCategoryMappingRepository.GetByEventId(command.EventDetail.EventId);
            foreach (var entry in entries)
            {
                _eventCategoryMappingRepository.Delete(entry);
            }
            var subcategories = command.EventDetail.EventCategories.Split(',');
            foreach (var subcat in subcategories)
            {
                var eventcatmapping = new FIL.Contracts.DataModels.EventCategoryMapping
                {
                    Id = 0,
                    EventId = command.EventDetail.EventId,
                    EventCategoryId = Convert.ToInt32(subcat),
                    IsEnabled = true,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    ModifiedBy = command.ModifiedBy,
                    UpdatedUtc = DateTime.UtcNow
                };
                _eventCategoryMappingRepository.Save(eventcatmapping);
            }
        }

        private void SaveEventSiteId(EventDetailsCommand command)
        {
            var eventSiteIds = _eventSiteIdMappingRepository.GetAllByEventId(command.EventDetail.EventId);
            if (!eventSiteIds.Any())
            {
                var last = _eventSiteIdMappingRepository.GetAll().OrderByDescending(p => p.CreatedUtc).FirstOrDefault();
                var eventsiteidmapping = new FIL.Contracts.DataModels.EventSiteIdMapping
                {
                    Id = 0,
                    EventId = command.EventDetail.EventId,
                    SiteId = Site.feelaplaceSite,
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

        private string GetItemsForViewer(EventDetailsCommand command)
        {
            var itemForViewer = "";
            foreach (var item in command.EventDetail.ItemsForViewer)
            {
                itemForViewer += item + "|";
            }
            return itemForViewer.Trim('|');
        }

        private FIL.Contracts.DataModels.Event SaveEvent(EventDetailsCommand command)
        {
            var eventData = new FIL.Contracts.DataModels.Event();
            eventData = command.EventDetail.EventId != 0 ? _eventRepository.Get(command.EventDetail.EventId) : eventData;
            var @event = _eventRepository.GetBySlug(Regex.Replace(command.EventDetail.Name, @"[ '&().,/]+", "-").Trim('-').ToLower());
            eventData.AltId = command.EventDetail.EventId != 0 ? eventData.AltId : Guid.NewGuid();
            eventData.Id = command.EventDetail.EventId != 0 ? eventData.Id : 0;
            eventData.Name = command.EventDetail.Name;
            eventData.TermsAndConditions = "NA";
            eventData.ClientPointOfContactId = 1;
            eventData.Description = command.EventDetail.Description;
            eventData.EventCategoryId = Convert.ToInt32(command.EventDetail.EventCategories.Split(',').FirstOrDefault());
            eventData.EventTypeId = eventData.Id != 0 ? eventData.EventTypeId : EventType.Regular;
            eventData.MetaDetails = GetItemsForViewer(command);
            eventData.EventSourceId = eventData.Id != 0 ? eventData.EventSourceId : EventSource.None;
            eventData.IsFeel = true;
            eventData.IsEnabled = command.EventDetail.IsEnabled;
            eventData.IsCreatedFromFeelAdmin = true;
            eventData.IsDelete = false;
            eventData.MasterEventTypeId = eventData.Id != 0 ? eventData.MasterEventTypeId : MasterEventType.Online;
            eventData.EventStatusId = eventData.Id != 0 ? eventData.EventStatusId : EventStatus.Draft;
            eventData.Slug = command.EventDetail.EventId != 0 ? eventData.Slug : Regex.Replace(command.EventDetail.Name, @"[ '&().,/]+", "-").Trim('-').ToLower();
            eventData.IsTokenize = command.EventDetail.IsPrivate;
            eventData.CreatedUtc = eventData.Id != 0 ? eventData.CreatedUtc : DateTime.UtcNow;
            eventData.UpdatedUtc = eventData.Id != 0 ? DateTime.UtcNow : eventData.CreatedUtc;
            eventData.CreatedBy = eventData.Id != 0 ? eventData.CreatedBy : command.ModifiedBy;
            eventData.UpdatedBy = command.ModifiedBy;
            eventData.ModifiedBy = command.ModifiedBy;
            var savedEvent = _eventRepository.Save(eventData);
            if (@event != null && command.EventDetail.EventId == 0)
            {
                eventData.Slug = Regex.Replace(command.EventDetail.Name + "-" + savedEvent.Id.ToString(), @"[ '&().,/]+", "-").Trim('-').ToLower();
                _eventRepository.Save(eventData);
            }
            return eventData;
        }

        protected override async Task<ICommandResult> Handle(EventDetailsCommand command)
        {
            try
            {
                var currentEvent = SaveEvent(command);
                command.EventDetail.EventId = currentEvent.Id;
                command.EventDetail.AltId = currentEvent.AltId;
                command.EventDetail.Slug = currentEvent.Slug;
                command.EventDetail.Description = currentEvent.Description;
                command.EventDetail.Name = currentEvent.Name;
                command.EventDetail.IsEnabled = currentEvent.IsEnabled;
                command.EventDetail.EventCategories = command.EventDetail.EventCategories;
                SaveEventCategoryMapping(command);
                SaveEventSiteId(command);
                UpdateEventDetailName(command);
                var eventStepDetail = _stepProvider.SaveEventStepDetails(currentEvent.Id, command.CurrentStep, true, command.ModifiedBy);
                return new EventDetailsCommandResult
                {
                    EventDetail = command.EventDetail,
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = eventStepDetail.CurrentStep,
                    Success = true
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new EventDetailsCommandResult { };
            }
        }
    }
}