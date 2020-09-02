using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class SaveEventReplayCommandHandler : BaseCommandHandlerWithResult<ReplayDetailCommand, ReplayDetailCommandResult>
    {
        private readonly IReplayDetailRepository _replayDetailRepository;
        private readonly IStepProvider _stepProvider;
        private readonly ILogger _logger;

        public SaveEventReplayCommandHandler(IReplayDetailRepository replayDetailRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _replayDetailRepository = replayDetailRepository;
            _stepProvider = stepProvider;
            _logger = logger;
        }

        public FIL.Contracts.DataModels.ReplayDetail SaveReplayDetail(
            FIL.Contracts.Models.CreateEventV1.ReplayDetailModel replayDetail,
            ReplayDetailCommand command
            )
        {
            var replayDetails = _replayDetailRepository.GetAllByEventId(command.EventId);
            var currentReplayDetail = replayDetails.Where(s => s.IsPaid == replayDetail.IsPaid).FirstOrDefault();
            var replayDetailDataModel = new ReplayDetail();
            replayDetailDataModel.AltId = currentReplayDetail != null ? currentReplayDetail.AltId : Guid.NewGuid();
            replayDetailDataModel.Id = currentReplayDetail != null ? currentReplayDetail.Id : 0;
            replayDetailDataModel.EventId = command.EventId;
            replayDetailDataModel.StartDate = replayDetail.IsEnabled ? replayDetail.StartDate : (DateTime?)null;
            replayDetailDataModel.EndDate = replayDetail.IsEnabled ? replayDetail.EndDate : (DateTime?)null;
            replayDetailDataModel.IsPaid = replayDetail.IsPaid;
            replayDetailDataModel.Price = replayDetail.IsEnabled ? replayDetail.Price : 0;
            replayDetailDataModel.IsEnabled = replayDetail.IsEnabled;
            replayDetailDataModel.CurrencyId = replayDetail.CurrencyId == 0 ? null : replayDetail.CurrencyId;
            replayDetailDataModel.CreatedBy = currentReplayDetail != null ? currentReplayDetail.CreatedBy : command.ModifiedBy;
            replayDetailDataModel.CreatedUtc = currentReplayDetail != null ? currentReplayDetail.CreatedUtc : DateTime.UtcNow;
            replayDetailDataModel.UpdatedBy = command.ModifiedBy;
            replayDetailDataModel.UpdatedUtc = DateTime.UtcNow;
            _replayDetailRepository.Save(replayDetailDataModel);
            return replayDetailDataModel;
        }

        protected override async Task<ICommandResult> Handle(ReplayDetailCommand command)
        {
            try
            {
                foreach (var replay in command.ReplayDetailModel)
                {
                    SaveReplayDetail(replay, command);
                }
                var eventStepDetail = _stepProvider.SaveEventStepDetails(command.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new ReplayDetailCommandResult
                {
                    Success = true,
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = command.CurrentStep,
                    ReplayDetailModel = command.ReplayDetailModel
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new ReplayDetailCommandResult { };
            }
        }
    }
}