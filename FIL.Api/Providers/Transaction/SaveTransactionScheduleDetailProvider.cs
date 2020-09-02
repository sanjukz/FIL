using FIL.Logging;
using FIL.Configuration;
using FIL.Api.Repositories;
using System;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Providers.Transaction
{
    public interface ISaveTransactionScheduleDetailProvider
    {
        void SaveTransactionScheduleDetail(long TransactionDetailId, long ScheduleDetailId);
    }

    public class SaveTransactionScheduleDetailProvider : ISaveTransactionScheduleDetailProvider
    {
        private readonly ITransactionScheduleDetail _transactionScheduleDetail;
        private readonly IScheduleDetailRepository _scheduleDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public SaveTransactionScheduleDetailProvider(ILogger logger,
            ISettings settings,
            ITransactionScheduleDetail transactionScheduleDetail,
                 IScheduleDetailRepository scheduleDetailRepository
            )
        {
            _scheduleDetailRepository = scheduleDetailRepository;
            _transactionScheduleDetail = transactionScheduleDetail;
            _logger = logger;
        }

        public void SaveTransactionScheduleDetail(long TransactionDetailId, long ScheduleDetailId)
        {
            try
            {
                var scheduleDetail = _scheduleDetailRepository.Get(ScheduleDetailId);
                var transactionScheduleDetail = new TransactionScheduleDetail
                {
                    TransactionDetailId = TransactionDetailId,
                    ScheduleDetailId = ScheduleDetailId,
                    StartDateTime = scheduleDetail.StartDateTime,
                    EndDateTime = scheduleDetail.EndDateTime,
                    IsEnabled = true,
                    CreatedBy = Guid.NewGuid(),
                    CreatedUtc = DateTime.UtcNow
                };
                _transactionScheduleDetail.Save(transactionScheduleDetail);
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
            }
        }
    }
}
