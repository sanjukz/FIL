using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Commands.Redemption;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Redemption
{
    public class GuideConfirmCommandHandler : BaseCommandHandler<GuideConfirmCommand>
    {
        private readonly IGuideDetailsRepository _GuideDetailsRepository;
        private readonly FIL.Logging.ILogger _logger;

        public GuideConfirmCommandHandler(IGuideDetailsRepository GuideDetailsRepository,
            FIL.Logging.ILogger logger,
            IMediator mediator)
           : base(mediator)
        {
            _GuideDetailsRepository = GuideDetailsRepository;
            _logger = logger;
        }

        protected override async Task Handle(GuideConfirmCommand command)
        {
            GuideDetailsCommandResult guideDetailsCommandResult = new GuideDetailsCommandResult();
            try
            {
                FIL.Contracts.DataModels.Redemption.Redemption_GuideDetails GuideDetails = new Contracts.DataModels.Redemption.Redemption_GuideDetails();
                GuideDetails = _GuideDetailsRepository.Get(command.Id);
                GuideDetails.IsEnabled = command.ApproveStatus == Contracts.Enums.ApproveStatus.Approved ? true : command.IsEnabled;
                GuideDetails.ApproveStatusId = command.ApproveStatus;
                GuideDetails.UpdatedUtc = DateTime.UtcNow;
                GuideDetails.ApprovedUtc = DateTime.UtcNow;
                GuideDetails.UpdatedBy = command.ModifiedBy;
                GuideDetails.ApprovedBy = command.ModifiedBy;
                GuideDetails = _GuideDetailsRepository.Save(GuideDetails);
                guideDetailsCommandResult.Id = GuideDetails.Id;
                guideDetailsCommandResult.UserId = GuideDetails.UserId;
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
        }
    }
}