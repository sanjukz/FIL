using FIL.Api.Repositories;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Contracts.QueryResults.TransactionReport;
using System;

namespace FIL.Api.QueryHandlers.TicketAlert
{
    public class TransactionReportQueryHandler : IQueryHandler<TransactionReportQuery, TransactionReportQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;

        public TransactionReportQueryHandler(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public TransactionReportQueryResult Handle(TransactionReportQuery query)
        {
            FIL.Contracts.DataModels.TransactionReport transactionReport = new Contracts.DataModels.TransactionReport();

            try
            {
                transactionReport = _reportingRepository.GetTransactionReportDataAsSingleDataModel(query.EventAltId, query.UserAltId, query.EventDetailId, query.FromDate, query.ToDate, query.CurrencyTypes);
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