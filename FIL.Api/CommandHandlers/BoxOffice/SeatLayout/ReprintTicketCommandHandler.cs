using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice.SeatLayout
{
    public class ReprintTicketCommandHandler : BaseCommandHandlerWithResult<ReprintTicketCommand, ReprintTicketCommandResult>
    {
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IReprintRequestDetailRepository _reprintRequestDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public ReprintTicketCommandHandler(

        IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
        FIL.Logging.ILogger logger,
        IReprintRequestDetailRepository reprintRequestDetailRepository,
        IMediator mediator) : base(mediator)
        {
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _logger = logger;
            _reprintRequestDetailRepository = reprintRequestDetailRepository;
        }

        protected override async Task<ICommandResult> Handle(ReprintTicketCommand command)
        {
            ReprintTicketCommandResult reprintTicketCommandResult = new ReprintTicketCommandResult();
            try
            {
                if (command.BarcodeNumbers.ToList().Any())
                {
                    var ticketDetails = _matchSeatTicketDetailRepository.GetByBarcodeList(command.BarcodeNumbers.ToList().Distinct())
                        .ToList();
                    if (ticketDetails.Any())
                    {
                        foreach (var item in ticketDetails)
                        {
                            ReprintRequestDetail reprintRequestDetail = AutoMapper.Mapper.Map<ReprintRequestDetail>(_reprintRequestDetailRepository.GetRequestDetailByBarcodeAndUserAltId(item.BarcodeNumber, command.ModifiedBy));
                            if (reprintRequestDetail.Id != -1)
                            {
                                reprintRequestDetail.IsRePrinted = true;
                                reprintRequestDetail.RePrintCount += 1;
                                reprintRequestDetail.UpdatedUtc = DateTime.UtcNow;
                                reprintRequestDetail.UpdatedBy = command.ModifiedBy;
                                _reprintRequestDetailRepository.Save(reprintRequestDetail);
                            }
                        }
                        reprintTicketCommandResult.Success = true;
                        reprintTicketCommandResult.TicketDetail = null;
                    }
                    else
                    {
                        reprintTicketCommandResult.Success = false;
                        reprintTicketCommandResult.TicketDetail = null;
                    }
                }
                else
                {
                    reprintTicketCommandResult.Success = false;
                    reprintTicketCommandResult.TicketDetail = null;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                reprintTicketCommandResult.Success = false;
                reprintTicketCommandResult.TicketDetail = null;
            }

            return reprintTicketCommandResult;
        }
    }
}