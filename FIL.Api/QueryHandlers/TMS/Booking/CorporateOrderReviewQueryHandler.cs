using FIL.Api.Repositories;
using FIL.Contracts.Models.TMS;
using FIL.Contracts.Queries.TMS.Booking;
using FIL.Contracts.QueryResults.TMS.Booking;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TMS.Booking
{
    public class CorporateOrderReviewQueryHandler : IQueryHandler<CorporateOrderReviewQuery, CorporateOrderReviewQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;

        public CorporateOrderReviewQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public CorporateOrderReviewQueryResult Handle(CorporateOrderReviewQuery query)
        {
            var corporateConfirmationDetails = AutoMapper.Mapper.Map<List<CorporateOrderDetails>>(_transactionRepository.GetCorporateOrderDetails(query.TransactionId));
            return new CorporateOrderReviewQueryResult
            {
                corporateOrderDetails = corporateConfirmationDetails
            };
        }
    }
}