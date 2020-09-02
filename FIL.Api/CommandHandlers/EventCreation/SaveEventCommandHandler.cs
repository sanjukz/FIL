using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class SaveEventCommandHandler : BaseCommandHandlerWithResult<SaveEventCommand, SaveEventDataResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventAmenityRepository _eventAmenityRepository;
        private readonly IPlaceVisitDurationRepository _placeVisitDurationRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTagMappingRepository _eventTagMappingRepository;
        //private readonly IEventGalleryImageRepository _eventGalleryImageRepository;

        public SaveEventCommandHandler(IEventRepository eventRepository, IMediator mediator,
            IEventCategoryMappingRepository eventCategoryMappingRepository,
            IEventAmenityRepository eventAmenityRepository,
            IPlaceVisitDurationRepository placeVisitDurationRepository,
            IEventTagMappingRepository eventTagMappingRepository,
            IEventDetailRepository eventDetailRepository,
        IEventSiteIdMappingRepository eventSiteIdMappingRepository
        //IEventGalleryImageRepository eventGalleryImageRepository
        )
            : base(mediator)
        {
            _eventRepository = eventRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _placeVisitDurationRepository = placeVisitDurationRepository;
            _eventAmenityRepository = eventAmenityRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTagMappingRepository = eventTagMappingRepository;
            //_eventGalleryImageRepository = eventGalleryImageRepository;
        }

        protected override Task<ICommandResult> Handle(SaveEventCommand command)
        {
            var place = _eventRepository.GetByFeelEventName(command.Name);
            SaveEventDataResult saveEventDataResult = new SaveEventDataResult();
            var eventData = new Event();
            if (place != null && !command.IsEdit)
            {
                command.Id = place.Id;
            }
            int eventCategory = command.EventCategoryId;
            var createdByGuid = command.ModifiedBy;
            if (command.IsEdit)
            {
                var currentEvent = _eventRepository.Get(command.Id);
                command.AltId = currentEvent.AltId;
                eventCategory = currentEvent.EventCategoryId;
                createdByGuid = currentEvent.CreatedBy;
            }
            eventData.AltId = command.AltId;
            eventData.Id = command.Id;
            eventData.Name = command.Name;
            eventData.TermsAndConditions = command.TermsAndConditions;
            eventData.ClientPointOfContactId = command.ClientPointOfContactId;
            eventData.Description = command.Description;
            eventData.EventCategoryId = (command.IsEdit ? eventCategory : command.EventCategoryId);
            eventData.EventTypeId = EventType.Perennial;
            eventData.MetaDetails = command.MetaDetails;
            eventData.CreatedBy = (command.IsEdit ? createdByGuid : command.ModifiedBy);
            eventData.ModifiedBy = command.ModifiedBy;
            eventData.CreatedUtc = (place != null ? place.CreatedUtc : DateTime.Now);
            eventData.EventSourceId = EventSource.None;
            eventData.IsFeel = command.IsFeel;//--
            eventData.IsEnabled = false;
            eventData.Slug = command.Name.ToLower().Replace(' ', '-');//--
            eventData.IsCreatedFromFeelAdmin = true;
            eventData.IsDelete = false;

            try
            {
                FIL.Contracts.DataModels.Event result = _eventRepository.Save(eventData);

                if (result != null && result.Id > 0)
                {
                    //savecategorymapping
                    SaveEventCategoryMapping(command, result.Id);
                    //Amenity
                    SaveEventAmentity(command, result.Id);
                    //Save siteid mapping
                    SaveEventSiteId(command, result.Id);
                    //Save Time Duration of place
                    SavePlaceVisitDuration(command, result.Id);
                    //Save Event Tag Mappings
                    SaveEventTagMappings(command, result.Id);
                }
                saveEventDataResult.Id = result.Id;
                saveEventDataResult.AltId = result.AltId;
            }
            catch (Exception ex)
            {
            }
            return Task.FromResult<ICommandResult>(saveEventDataResult);
        }

        private void SaveEventAmentity(SaveEventCommand command, long id)
        {
            var entries = _eventAmenityRepository.GetByEventId(id);
            foreach (var entry in entries)
            {
                _eventAmenityRepository.Delete(entry);
            }
            if (command.AmenityIds != "")
            {
                var amenities = command.AmenityIds.Split(',');
                foreach (var amenity in amenities)
                {
                    var eventAmenity = new EventAmenity
                    {
                        AmenityId = Convert.ToInt32(amenity),
                        Description = command.Description ?? "",
                        EventId = id,
                        IsEnabled = command.IsEnabled,
                        UpdatedBy = command.ModifiedBy,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        ModifiedBy = command.ModifiedBy,
                        Id = 0
                    };
                    _eventAmenityRepository.Save(eventAmenity);
                }
            }
        }

        private void SaveEventTagMappings(SaveEventCommand command, long id)
        {
            var entries = _eventTagMappingRepository.GetByEventId(id);
            foreach (var entry in entries)
            {
                _eventTagMappingRepository.Delete(entry);
            }
            if (command.TagIds != "")
            {
                var tags = command.TagIds.Split(',');
                foreach (var tag in tags)
                {
                    var eventTag = new eventtagmappings
                    {
                        TagId = Convert.ToInt64(tag),
                        EventId = id,
                        AltId = Guid.NewGuid(),
                        SortOrder = 1,
                        IsEnabled = command.IsEnabled,
                        UpdatedBy = command.ModifiedBy,
                        UpdatedUtc = DateTime.UtcNow,
                        CreatedBy = command.ModifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        ModifiedBy = command.ModifiedBy,
                        Id = 0
                    };
                    _eventTagMappingRepository.Save(eventTag);
                }
            }
        }

        private void SavePlaceVisitDuration(SaveEventCommand command, long id)
        {
            var entries = _placeVisitDurationRepository.GetByEventId(id).ToList();

            foreach (FIL.Contracts.DataModels.PlaceVisitDuration currentPlaceVisitDuration in entries)
            {
                _placeVisitDurationRepository.Delete(currentPlaceVisitDuration);
            }
            PlaceVisitDuration placeVisitDuration = new PlaceVisitDuration();
            placeVisitDuration.AltId = Guid.NewGuid();
            placeVisitDuration.EventId = id;
            placeVisitDuration.TimeDuration = command.TimeDuration;
            placeVisitDuration.IsEnabled = true;
            placeVisitDuration.CreatedUtc = DateTime.UtcNow;
            placeVisitDuration.UpdatedUtc = DateTime.UtcNow;
            placeVisitDuration.CreatedBy = Guid.NewGuid();
            _placeVisitDurationRepository.Save(placeVisitDuration);
        }

        private void SaveEventSiteId(SaveEventCommand command, long id)
        {
            if (!command.IsEdit)
            {
                var entries = _eventSiteIdMappingRepository.GetAllByEventId(id);
                /* foreach (var entry in entries)
                 {
                     _eventSiteIdMappingRepository.Delete(entry);
                 }*/
                var last = _eventSiteIdMappingRepository.GetAll().OrderByDescending(p => p.CreatedUtc).FirstOrDefault();
                if (last != null && (entries == null || entries.Count() == 0))
                {
                    var eventsiteidmapping = new FIL.Contracts.DataModels.EventSiteIdMapping
                    {
                        Id = 0,
                        EventId = id,
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
        }

        private void SaveEventCategoryMapping(SaveEventCommand command, long id)
        {
            var entries = _eventCategoryMappingRepository.GetByEventId(id);
            foreach (var entry in entries)
            {
                _eventCategoryMappingRepository.Delete(entry);
            }

            var subcategories = command.SubCategoryIds.Split(',');
            foreach (var subcat in subcategories)
            {
                var eventcatmapping = new FIL.Contracts.DataModels.EventCategoryMapping
                {
                    Id = 0,
                    EventId = id,
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
    }
}