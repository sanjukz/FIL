using FIL.Api.Repositories;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResults.TMS;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS
{
    public class ReprintBarcodeDetailQueryHandler : IQueryHandler<BarcodeDetailQuery, BarcodeDetailQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;

        public ReprintBarcodeDetailQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public BarcodeDetailQueryResult Handle(BarcodeDetailQuery query)
        {
            var barcodeDetails = _transactionRepository.GetBarcodeDetails(query.TransactionId).ToList();
            return new BarcodeDetailQueryResult
            {
                TicketDetailModel = barcodeDetails,
            };
        }
    }
}