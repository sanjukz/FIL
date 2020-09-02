using FIL.Api.Repositories;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class RevertTransactionQueryHandler : IQueryHandler<RevertTransactionQuery, RevertTransactionQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;

        public RevertTransactionQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public RevertTransactionQueryResult Handle(RevertTransactionQuery query)
        {
            var response = new RevertTransactionQueryResult();
            var transaction = _transactionRepository.GetSuccessfulBOTransactionDetails(query.TransactionId);
            if (transaction != null)
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;
        }
    }
}