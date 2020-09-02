using FIL.Api.Repositories;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class TransactionLocatorQueryHandler : IQueryHandler<TransactionLocatorQuery, TransactionLocatorQueryResult>
    {
        private readonly FIL.Logging.ILogger _logger;
        private readonly ITransactionsDataRepository _transactionsDataRepository;

        public TransactionLocatorQueryHandler(ITransactionsDataRepository transactionsDataRepository, FIL.Logging.ILogger logger)
        {
            _transactionsDataRepository = transactionsDataRepository;
            _logger = logger;
        }

        public TransactionLocatorQueryResult Handle(TransactionLocatorQuery query)
        {
            try
            {
                Contracts.DataModels.TransactionsData transactionsData = AutoMapper.Mapper.Map<Contracts.DataModels.TransactionsData>(_transactionsDataRepository.GetCustomerInfo(query.TransactionId, query.FirstName, query.LastName, query.EmailId, query.UserMobileNo, query.BarcodeNumber, query.IsFulfilment));
                return new TransactionLocatorQueryResult
                {
                    TransactionInfos = transactionsData.TransactionInfos
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return new TransactionLocatorQueryResult
            {
                TransactionInfos = null
            };
        }
    }
}