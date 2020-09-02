using FIL.Api.CommandHandlers;
using FIL.Api.Providers;
using FIL.Api.Providers.Algolia;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.ApproveModeratePlace;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.ApproveModeratePlace
{
    public class ApproveModeratePlaceCommandHandler : BaseCommandHandlerWithResult<ApproveModeratePlaceCommand, ApproveModeratePlaceCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository _eventRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IAlgoliaAddEventProvider _algoliaAddEventProvider;
        private readonly IAlgoliaClientProvider _algoliaClientProvider;
        private readonly IStepProvider _stepProvider;

        public ApproveModeratePlaceCommandHandler(IMediator mediator,
             IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            IAlgoliaAddEventProvider algoliaAddEventProvider,
             IEventCategoryRepository eventCategoryRepository,
             IUserRepository userRepository,
             IEventCategoryMappingRepository eventCategoryMappingRepository,
             IStepProvider stepProvider,
            IEventRepository eventRepository, ILogger logger, IAlgoliaClientProvider algoliaClientProvider) : base(mediator)
        {
            _mediator = mediator;
            _eventRepository = eventRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _userRepository = userRepository;
            _logger = logger;
            _algoliaAddEventProvider = algoliaAddEventProvider;
            _algoliaClientProvider = algoliaClientProvider;
            _stepProvider = stepProvider;
        }

        protected override async Task<ICommandResult> Handle(ApproveModeratePlaceCommand command)
        {
            ApproveModeratePlaceCommandResult approveModeratePlaceCommandResult = new ApproveModeratePlaceCommandResult();
            try
            {
                if (command.EventStatus != 0 && command.EventId != 0)
                {
                    var currentEvent = _eventRepository.Get(command.EventId);
                    currentEvent.EventStatusId = command.EventStatus;
                    currentEvent.IsEnabled = command.EventStatus == Contracts.Enums.EventStatus.Draft ?
                        false : command.EventStatus == Contracts.Enums.EventStatus.Published ?
                        true : currentEvent.IsEnabled;
                    _eventRepository.Save(currentEvent);
                    if (currentEvent.EventStatusId == Contracts.Enums.EventStatus.WaitingForApproval)
                    {
                        var eventStepDetails = _stepProvider.SaveEventStepDetails(currentEvent.Id, 12, true, command.ModifiedBy);
                    }
                    return new ApproveModeratePlaceCommandResult
                    {
                        Success = true,
                        IsTokanize = currentEvent.IsTokenize,
                        PlaceAltId = currentEvent.AltId
                    };
                }
                var eventData = _eventRepository.GetByAltId(command.PlaceAltId);
                eventData.IsEnabled = !command.IsDeactivateFeels ? true : false;
                eventData.EventStatusId = !command.IsDeactivateFeels ? FIL.Contracts.Enums.EventStatus.Published : FIL.Contracts.Enums.EventStatus.Draft;
                _eventRepository.Save(eventData);
                var eveventSiteIds = _eventSiteIdMappingRepository.GetAllByEventId(eventData.Id).ToList();
                var eventCategoryMappings = _eventCategoryMappingRepository.GetByEventId(eventData.Id).FirstOrDefault();
                var subCategory = _eventCategoryRepository.Get(eventCategoryMappings.EventCategoryId);
                var parentCategory = _eventCategoryRepository.Get(subCategory.EventCategoryId);
                var user = _userRepository.GetByAltId(eventData.CreatedBy);
                if (eveventSiteIds != null)
                {
                    foreach (FIL.Contracts.DataModels.EventSiteIdMapping eventSiteIdmappings in eveventSiteIds)
                    {
                        var eventSiteIdMap = _eventSiteIdMappingRepository.Get(eventSiteIdmappings.Id);
                        if (eventSiteIdMap != null)
                        {
                            eventSiteIdMap.IsEnabled = !command.IsDeactivateFeels ? true : false;
                            _eventSiteIdMappingRepository.Save(eventSiteIdMap);
                        }
                    }
                    approveModeratePlaceCommandResult.Slug = eventData.Slug;
                    approveModeratePlaceCommandResult.ParentCategorySlug = parentCategory.Slug;
                    approveModeratePlaceCommandResult.SubCategorySlug = subCategory.Slug;
                    approveModeratePlaceCommandResult.Email = user.Email;
                    approveModeratePlaceCommandResult.ParentCategoryId = subCategory.EventCategoryId;
                    approveModeratePlaceCommandResult.Success = true;
                    approveModeratePlaceCommandResult.MasterEventTypeId = eventData.MasterEventTypeId;
                    approveModeratePlaceCommandResult.PlaceAltId = eventData.AltId;
                    approveModeratePlaceCommandResult.IsTokanize = eventData.IsTokenize;
                }
                try
                {
                    if (!command.IsDeactivateFeels && command.UpdateAlgolia && !eventData.IsTokenize)
                    {
                        _algoliaAddEventProvider.AddEventToAlgolia(eventData.Id);
                    }
                    if (command.IsDeactivateFeels)
                    {
                        _algoliaClientProvider.DeleteObject(eventData.Id.ToString());
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, e);
                }
                return approveModeratePlaceCommandResult;
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new ApproveModeratePlaceCommandResult
                {
                    Success = false
                };
            }
        }
    }
}