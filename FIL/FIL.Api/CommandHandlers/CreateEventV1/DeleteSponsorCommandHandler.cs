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
    public class DeleteSponsorCommandHandler : BaseCommandHandlerWithResult<DeleteSponsorCommand, DeleteSponsorCommandResult>
    {
        private readonly IFILSponsorDetailRepository _fILSponsorDetailRepository;
        private readonly IStepProvider _stepProvider;
        private readonly ILogger _logger;

        public DeleteSponsorCommandHandler(
           IFILSponsorDetailRepository fILSponsorDetailRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _fILSponsorDetailRepository = fILSponsorDetailRepository;
            _stepProvider = stepProvider;
            _logger = logger;
        }

        protected override async Task<ICommandResult> Handle(DeleteSponsorCommand command)
        {
            try
            {
                var sponsorDetail = _fILSponsorDetailRepository.GetByAltId(command.SponsorAltId);
                _fILSponsorDetailRepository.Delete(sponsorDetail);

                var eventStepDetails = _stepProvider.SaveEventStepDetails(sponsorDetail.EventId, command.CurrentStep, false, command.ModifiedBy);
                return new DeleteSponsorCommandResult
                {
                    Success = true,
                    CompletedStep = eventStepDetails.CompletedStep,
                    CurrentStep = eventStepDetails.CurrentStep,
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new DeleteHostCommandResult { };
            }
        }
    }
}