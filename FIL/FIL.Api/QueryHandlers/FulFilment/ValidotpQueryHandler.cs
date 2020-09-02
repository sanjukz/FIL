using FIL.Api.Repositories;
using FIL.Contracts.Queries.FulFilment;
using FIL.Contracts.QueryResults.FulFilment;

namespace FIL.Api.QueryHandlers.FulFilment
{
    public class ValidotpQueryHandler : IQueryHandler<ValidotpQuery, ValidOtpQueryResult>
    {
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;

        public ValidotpQueryHandler(ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository)
        {
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
        }

        public ValidOtpQueryResult Handle(ValidotpQuery query)
        {
            var response = new ValidOtpQueryResult();
            var transactionDeliveryDetail = _transactionDeliveryDetailRepository.GetByTransactionDetailId(query.TransactionDetailId);
            if (transactionDeliveryDetail.PickupOTP == query.PickupOTP)
            {
                response.IsValid = true;
            }
            else
            {
                response.IsValid = false;
            }
            return response;
        }
    }
}