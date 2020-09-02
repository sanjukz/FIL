using FIL.Api.Repositories;
using FIL.Contracts.Queries.Creator;
using FIL.Contracts.QueryResults;
using FIL.Api.Providers;
using System;
using System.Globalization;
using System.Linq;

namespace FIL.Api.QueryHandlers.Creator
{
    public class FILTransactionLocatorQueryHandler : IQueryHandler<FILTransactionLocatorQuery, FILTransactionQueryResult>
    {
        private readonly FIL.Logging.ILogger _logger;
        private readonly ITransactionsDataRepository _transactionsDataRepository;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;

        public FILTransactionLocatorQueryHandler(
            ITransactionsDataRepository transactionsDataRepository,
            ILocalTimeZoneConvertProvider localTimeZoneConvertProvider,
            FIL.Logging.ILogger logger)
        {
            _transactionsDataRepository = transactionsDataRepository;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
            _logger = logger;
        }

        public FILTransactionQueryResult Handle(FILTransactionLocatorQuery query)
        {
            try
            {
                var filTransactionLocator = _transactionsDataRepository.GetFILTransactionLocator(query.TransactionId,
                    query.FirstName,
                    query.LastName,
                    query.EmailId,
                    query.UserMobileNo);
                filTransactionLocator.TransactionData = filTransactionLocator.TransactionData.GroupBy(c => new
                {
                    c.TransactionId,
                    c.TicketCategoryName
                }).Select(x => x.First()).ToList();
                foreach (var item in filTransactionLocator.TransactionData)
                {
                    var localStartDate = _localTimeZoneConvertProvider.ConvertToLocal(item.EventStartDate, item.TimeZone);
                    var localEndDate = _localTimeZoneConvertProvider.ConvertToLocal(item.EventEndDate, item.TimeZone);
                    item.LocalStartDateString = localStartDate.DayOfWeek + ", " + localStartDate.ToString(@"MMM dd, yyyy", new CultureInfo("en-US"));
                    item.LocalEndDateString = localEndDate.DayOfWeek + ", " + localEndDate.ToString(@"MMM dd, yyyy", new CultureInfo("en-US"));
                }
                return new FILTransactionQueryResult
                {
                    Success = true,
                    FILTransactionLocator = filTransactionLocator
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new FILTransactionQueryResult { };
            }
        }
    }
}