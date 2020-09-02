using FIL.Api.Repositories;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Contracts.QueryResults.TransactionReport;
using System;

namespace FIL.Api.QueryHandlers.TransactionReport.Graph
{
    public class TransactionDataQueryHandler : IQueryHandler<TransactionDataQuery, TransactionDataQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;

        public TransactionDataQueryHandler(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public TransactionDataQueryResult Handle(TransactionDataQuery query)
        {
            FIL.Contracts.DataModels.TransactionReport transactionReport = new Contracts.DataModels.TransactionReport();

            try
            {
                transactionReport = _reportingRepository.GetTransactionData(
                    query.EventAltId,
                    query.UserAltId,
                    query.VenueId,
                    query.EventDetailId,
                    query.TicketCategoryId,
                    query.ChannelId,
                    query.CurrencyTypes,
                    query.TicketTypes,
                    query.TransactionTypes,
                    query.PaymentGatewayes,
                    query.FromDate,
                    query.ToDate
                  );
                return new TransactionDataQueryResult
                {
                    TransactionReportData = transactionReport
                };
            }
            catch (Exception e)
            {
                return new TransactionDataQueryResult
                {
                    TransactionReportData = transactionReport
                };
            }
        }
    }
}