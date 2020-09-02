using FIL.Api.CommandHandlers;
using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class SaveEventHostCommandHandler : BaseCommandHandlerWithResult<EventHostCommand, EventHostCommandResult>
    {
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly IStepProvider _stepProvider;
        private readonly FIL.Logging.ILogger _logger;

        public SaveEventHostCommandHandler(
            IEventHostMappingRepository eventHostMappingRepository,
            IStepProvider stepProvider,
            FIL.Logging.ILogger logger,
            IMediator mediator) : base(mediator)
        {
            _eventHostMappingRepository = eventHostMappingRepository;
            _stepProvider = stepProvider;
            _logger = logger;
        }

        protected FIL.Contracts.DataModels.EventHostMapping SaveEventHost(EventHostCommand command,
            EventHostMapping newEventHostMapping
            )
        {
            var eventHostData = new EventHostMapping
            {
                Id = newEventHostMapping.Id,
                AltId = newEventHostMapping.Id == 0 ? Guid.NewGuid() : newEventHostMapping.AltId,
                Description = newEventHostMapping.Description,
                Email = newEventHostMapping.Email,
                EventId = command.EventId,
                FirstName = newEventHostMapping.FirstName,
                LastName = newEventHostMapping.LastName,
                IsEnabled = newEventHostMapping.IsEnabled,
                ImportantInformation = newEventHostMapping.ImportantInformation,
                CreatedUtc = newEventHostMapping.Id != 0 ? newEventHostMapping.CreatedUtc : DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                CreatedBy = newEventHostMapping.Id != 0 ? newEventHostMapping.CreatedBy : command.ModifiedBy,
                UpdatedBy = command.ModifiedBy,
                ModifiedBy = command.ModifiedBy
            };
            return _eventHostMappingRepository.Save(eventHostData);
        }

        protected override async Task<ICommandResult> Handle(EventHostCommand command)
        {
            try
            {
                var eventHostList = _eventHostMappingRepository.GetAllByEventId(command.EventId).ToList();
                List<FIL.Contracts.DataModels.EventHostMapping> EventHostMappings = new List<EventHostMapping>();
                foreach (FIL.Contracts.DataModels.EventHostMapping eventHostMapping in command.EventHostMappings)
                {
                    if (eventHostList.Where(s => s.Id == eventHostMapping.Id).Any())
                    {
                        eventHostMapping.Id = eventHostList.Where(s => s.Id == eventHostMapping.Id).FirstOrDefault().Id;
                        eventHostMapping.AltId = eventHostList.Where(s => s.Id == eventHostMapping.Id).FirstOrDefault().AltId;
                        eventHostMapping.CreatedUtc = eventHostList.Where(s => s.Id == eventHostMapping.Id).FirstOrDefault().CreatedUtc;
                        eventHostMapping.CreatedBy = eventHostList.Where(s => s.Id == eventHostMapping.Id).FirstOrDefault().CreatedBy;
                        eventHostMapping.ModifiedBy = eventHostList.Where(s => s.Id == eventHostMapping.Id).FirstOrDefault().ModifiedBy;
                    }
                    var currentHost = SaveEventHost(command, eventHostMapping);
                    EventHostMappings.Add(currentHost);
                }
                var eventStepDetail = _stepProvider.SaveEventStepDetails(command.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new EventHostCommandResult
                {
                    EventHostMappings = EventHostMappings,
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = eventStepDetail.CurrentStep,
                    Success = true
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Fail to update the Event Host", e));
                return new EventHostCommandResult { };
            }
        }
    }
}