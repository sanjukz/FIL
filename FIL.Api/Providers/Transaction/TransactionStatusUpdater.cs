using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers.Transaction
{
    public interface ITransactionStatusUpdater
    {
        FIL.Contracts.DataModels.Transaction UpdateTranscationStatus(long transactionId);
    }

    public class TransactionStatusUpdater : ITransactionStatusUpdater
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITransactionReleaseLogRepository _transactionReleaseLogRepository;
        private readonly ILogger _logger;

        public TransactionStatusUpdater(ILogger logger,
            ITransactionRepository transactionRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            ITransactionReleaseLogRepository transactionReleaseLogRepository)
        {
            _transactionRepository = transactionRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _transactionReleaseLogRepository = transactionReleaseLogRepository;
            _logger = logger;
        }

        public FIL.Contracts.DataModels.Transaction UpdateTranscationStatus(long transactionId)
        {
            FIL.Contracts.DataModels.Transaction transactionResult = new Contracts.DataModels.Transaction();
            FIL.Contracts.DataModels.TransactionReleaseLog transactionReleaseLog = _transactionReleaseLogRepository.GetByTransactionId(transactionId);
            if (transactionReleaseLog == null)
            {
                var transaction = _transactionRepository.Get(Convert.ToInt64(transactionId));
                transaction.TransactionStatusId = TransactionStatus.Success;
                transactionResult = _transactionRepository.Save(transaction);

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
            }
            return transactionResult;
        }
    }
}