using FIL.Api.Repositories;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Contracts.QueryResults.TransactionReport;
using System;

namespace FIL.Api.QueryHandlers.TransactionReport
{
    public class FAPTransactionReportQueryHandler : IQueryHandler<FAPTransactionReportQuery, TransactionReportQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;
        private readonly IEventRepository _eventRepository;

        public FAPTransactionReportQueryHandler(
            IReportingRepository reportingRepository,
            IEventRepository eventRepository
            )
        {
            _reportingRepository = reportingRepository;
            _eventRepository = eventRepository;
        }

        public TransactionReportQueryResult Handle(FAPTransactionReportQuery query)
        {
            FIL.Contracts.DataModels.TransactionReport transactionReport = new Contracts.DataModels.TransactionReport();

            try
            {
                query.FromDate = new DateTime(query.FromDate.Year, query.FromDate.Month, query.FromDate.Day).ToUniversalTime();
                query.ToDate = new DateTime(query.ToDate.Year, query.ToDate.Month, query.ToDate.Day).ToUniversalTime();
                transactionReport = _reportingRepository.GetFAPTransactionReport(query.EventAltId, query.FromDate, query.ToDate, query.CurrencyTypes);
                return new TransactionReportQueryResult
                {
                    TransactionReportData = transactionReport
                };
            }
            catch (Exception e)
            {
                return new TransactionReportQueryResult
                {
                    TransactionReportData = transactionReport
                };
            }
        }
    }
}