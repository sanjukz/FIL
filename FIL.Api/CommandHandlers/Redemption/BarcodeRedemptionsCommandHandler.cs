using FIL.Api.Repositories;
using FIL.Contracts.Commands.Redemption;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Redemption
{
    public class BarcodeRedemptionsCommandHandler : BaseCommandHandlerWithResult<BarcodeRedemptionCommand, BarcodeRedemptionCommandResult>
    {
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IOfflineBarcodeDetailRepository _offlineBarcodeDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public BarcodeRedemptionsCommandHandler(IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            IOfflineBarcodeDetailRepository offlineBarcodeDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            FIL.Logging.ILogger logger,
            IMediator mediator)
           : base(mediator)
        {
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _offlineBarcodeDetailRepository = offlineBarcodeDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _logger = logger;
        }

        protected override async Task<ICommandResult> Handle(BarcodeRedemptionCommand command)
        {
            BarcodeRedemptionCommandResult barcodeRedemptionCommandResult = new BarcodeRedemptionCommandResult();
            try
            {
                var eventTicketDetails = _eventTicketDetailRepository.GetRASVRideEventTicketDetails()
                    .ToDictionary(k => k.Id);
                var matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByBarcodeNumberAndEventTicketDetailIds(command.BarcodeNumber, eventTicketDetails.Keys);

                if (matchSeatTicketDetail != null)
                {
                    if (!matchSeatTicketDetail.IsConsumed)
                    {
                        matchSeatTicketDetail.IsConsumed = true;
                        matchSeatTicketDetail.ConsumedDateTime = DateTime.UtcNow;
                        FIL.Contracts.DataModels.MatchSeatTicketDetail RedemptionResult = _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                        barcodeRedemptionCommandResult.Id = RedemptionResult.Id;
                        barcodeRedemptionCommandResult.AltId = (System.Guid)RedemptionResult.AltId;
                        barcodeRedemptionCommandResult.BarcodeNumber = RedemptionResult.BarcodeNumber;
                        barcodeRedemptionCommandResult.IsConsumed = RedemptionResult.IsConsumed;
                        barcodeRedemptionCommandResult.ConsumedDateTime = RedemptionResult.ConsumedDateTime;
                        barcodeRedemptionCommandResult.Success = true;
                    }
                    else
                    {
                        barcodeRedemptionCommandResult.Id = matchSeatTicketDetail.Id;
                        barcodeRedemptionCommandResult.AltId = (System.Guid)matchSeatTicketDetail.AltId;
                        barcodeRedemptionCommandResult.BarcodeNumber = matchSeatTicketDetail.BarcodeNumber;
                        barcodeRedemptionCommandResult.IsConsumed = matchSeatTicketDetail.IsConsumed;
                        barcodeRedemptionCommandResult.ConsumedDateTime = matchSeatTicketDetail.ConsumedDateTime;
                        barcodeRedemptionCommandResult.Success = false;
                    }
                }
                else
                {
                    var offlineBarcodeDetail = _offlineBarcodeDetailRepository.GetByBarcodeNumberAndEventTicketDetailIds(command.BarcodeNumber, eventTicketDetails.Keys);
                    if (offlineBarcodeDetail != null)
                    {
                        if (!offlineBarcodeDetail.IsConsumed)
                        {
                            offlineBarcodeDetail.IsConsumed = true;
                            offlineBarcodeDetail.ConsumedDateTime = DateTime.UtcNow;
                            _offlineBarcodeDetailRepository.Save(offlineBarcodeDetail);
                            FIL.Contracts.DataModels.OfflineBarcodeDetail RedemptionResult = _offlineBarcodeDetailRepository.Save(offlineBarcodeDetail);
                            barcodeRedemptionCommandResult.Id = RedemptionResult.Id;
                            barcodeRedemptionCommandResult.AltId = RedemptionResult.AltId;
                            barcodeRedemptionCommandResult.BarcodeNumber = RedemptionResult.BarcodeNumber;
                            barcodeRedemptionCommandResult.IsConsumed = RedemptionResult.IsConsumed;
                            barcodeRedemptionCommandResult.ConsumedDateTime = RedemptionResult.ConsumedDateTime;
                            barcodeRedemptionCommandResult.Success = true;
                        }
                        else
                        {
                            barcodeRedemptionCommandResult.Id = offlineBarcodeDetail.Id;
                            barcodeRedemptionCommandResult.AltId = offlineBarcodeDetail.AltId;
                            barcodeRedemptionCommandResult.BarcodeNumber = offlineBarcodeDetail.BarcodeNumber;
                            barcodeRedemptionCommandResult.IsConsumed = offlineBarcodeDetail.IsConsumed;
                            barcodeRedemptionCommandResult.ConsumedDateTime = offlineBarcodeDetail.ConsumedDateTime;
                            barcodeRedemptionCommandResult.Success = false;
                        }
                    }
                    else
                    {
                        barcodeRedemptionCommandResult.Id = -1;
                        barcodeRedemptionCommandResult.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                barcodeRedemptionCommandResult.Id = -1;
                barcodeRedemptionCommandResult.Success = false;
            }
            return barcodeRedemptionCommandResult;
        }
    }
}