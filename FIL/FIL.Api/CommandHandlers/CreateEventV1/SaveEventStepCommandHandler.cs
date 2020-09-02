using FIL.Api.Providers;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class SaveEventStepCommandHandler : BaseCommandHandlerWithResult<EventStepCommand, EventStepCommandResult>
    {
        private readonly IStepProvider _stepProvider;
        private readonly ILogger _logger;

        public SaveEventStepCommandHandler(
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _stepProvider = stepProvider;
            _logger = logger;
        }

        protected override async Task<ICommandResult> Handle(EventStepCommand command)
        {
            try
            {
                var eventStepDetail = _stepProvider.SaveEventStepDetails(command.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new EventStepCommandResult
                {
                    CurrentStep = eventStepDetail.CurrentStep,
                    CompletedStep = eventStepDetail.CompletedStep,
                    Success = true
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new EventStepCommandResult { };
            }
        }
    }
}