using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class DeleteEventHostCommandHandler : BaseCommandHandlerWithResult<DeleteHostCommand, DeleteHostCommandResult>
    {
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly IZoomUserRepository _zoomUserRepository;
        private readonly IStepProvider _stepProvider;
        private readonly ILogger _logger;

        public DeleteEventHostCommandHandler(
            IEventHostMappingRepository eventHostMappingRepository,
            IZoomUserRepository zoomUserRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _eventHostMappingRepository = eventHostMappingRepository;
            _zoomUserRepository = zoomUserRepository;
            _stepProvider = stepProvider;
            _logger = logger;
        }

        protected override async Task<ICommandResult> Handle(DeleteHostCommand command)
        {
            try
            {
                var eventHost = _eventHostMappingRepository.GetByAltId(command.EventHostAltId);
                var zoomUser = _zoomUserRepository.GetByHostUserId(eventHost.Id);
                if (zoomUser == null)
                {
                    _eventHostMappingRepository.Delete(eventHost);
                    var eventStepDetails = _stepProvider.SaveEventStepDetails(eventHost.EventId, command.CurrentStep, command.TicketLength == 1 ? false : true, command.ModifiedBy);
                    return new DeleteHostCommandResult
                    {
                        Success = true,
                        CompletedStep = eventStepDetails.CompletedStep,
                        CurrentStep = eventStepDetails.CurrentStep
                    };
                }
                else
                {
                    return new DeleteHostCommandResult
                    {
                        IsHostStreamLinkCreated = true,
                        Success = false
                    };
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new DeleteHostCommandResult { };
            }
        }
    }
}