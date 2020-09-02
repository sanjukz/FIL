using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS.Booking;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS.Booking
{
    public class UpdateTransactionCommandHandler : BaseCommandHandlerWithResult<UpdateTransactionCommand, UpdateTransactionCommandResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionReleaseLogRepository _transactionReleaseLogRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;

        public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository,
            ITransactionReleaseLogRepository transactionReleaseLogRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            IMediator mediator)
            : base(mediator)
        {
            _transactionRepository = transactionRepository;
            _transactionReleaseLogRepository = transactionReleaseLogRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
        }

        protected override Task<ICommandResult> Handle(UpdateTransactionCommand command)
        {
            UpdateTransactionCommandResult updateTransactionCommandResult = new UpdateTransactionCommandResult();

            FIL.Contracts.DataModels.TransactionReleaseLog transactionReleaseLog = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.TransactionReleaseLog>(_transactionReleaseLogRepository.GetByTransactionId(command.TransactionId));
            if (transactionReleaseLog == null)
            {
                var transaction = _transactionRepository.Get(Convert.ToInt64(command.TransactionId));
                transaction.TransactionStatusId = TransactionStatus.Success;
                FIL.Contracts.DataModels.Transaction transactionResult = _transactionRepository.Save(transaction);

                /* final seat status update */
                if (transactionResult.Id != -1)
                {
                    List<FIL.Contracts.DataModels.MatchSeatTicketDetail> matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetbyTransactionId(transactionResult.Id).ToList();
                    if (matchSeatTicketDetails != null)
                    {
                        foreach (var matchSeatTicketDetail in matchSeatTicketDetails)
                        {
                            if (matchSeatTicketDetail.MatchLayoutSectionSeatId != null)
                            {
                                FIL.Contracts.DataModels.MatchLayoutSectionSeat matchLayoutSectionSeat = _matchLayoutSectionSeatRepository.Get((long)matchSeatTicketDetail.MatchLayoutSectionSeatId);
                                if (matchLayoutSectionSeat.SeatStatusId == SeatStatus.BlockedByCustomer)
                                {
                                    matchLayoutSectionSeat.SeatStatusId = SeatStatus.Sold;
                                    matchLayoutSectionSeat.UpdatedUtc = DateTime.UtcNow;
                                    _matchLayoutSectionSeatRepository.Save(matchLayoutSectionSeat);
                                }
                            }
                        }
                    }
                }
                updateTransactionCommandResult.Id = transactionResult.Id;
                updateTransactionCommandResult.Success = true;
            }
            return Task.FromResult<ICommandResult>(updateTransactionCommandResult);
        }
    }
}