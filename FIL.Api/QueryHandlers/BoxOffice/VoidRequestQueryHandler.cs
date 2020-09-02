using FIL.Api.Repositories;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.VoidRequestSuccessTransaction;

namespace FIL.Api.QueryHandlers.VoidRequest
{
    public class VoidRequestQueryHandler : IQueryHandler<VoidRequestSearchSucessTransaction, VoidRequestSuccessTransaction>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IVoidRequestDetailRepository _voidRequestDetailRepository;

        public VoidRequestQueryHandler(ITransactionRepository transactionRepository, IVoidRequestDetailRepository voidRequestDetailRepository)
        {
            _transactionRepository = transactionRepository;
            _voidRequestDetailRepository = voidRequestDetailRepository;
        }

        public VoidRequestSuccessTransaction Handle(VoidRequestSearchSucessTransaction query)
        {
            var response = new VoidRequestSuccessTransaction();
            var user = _transactionRepository.GetSuccessfulBOTransactionDetails(query.TransactionId);
            var voidRequestDetails = _voidRequestDetailRepository.GetByTransId(query.TransactionId);
            if (user != null && voidRequestDetails == null)
            {
                response.IsExisting = true;
            }
            else
            {
                if (voidRequestDetails != null)
                {
                    response.IsRequestSent = true;
                }
                response.IsExisting = false;
            }
            return response;
        }
    }
}