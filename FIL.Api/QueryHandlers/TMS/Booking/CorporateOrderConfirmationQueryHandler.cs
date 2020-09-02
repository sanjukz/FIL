using FIL.Api.Repositories;
using FIL.Contracts.Models.TMS;
using FIL.Contracts.Queries.TMS.Booking;
using FIL.Contracts.QueryResults.TMS.Booking;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TMS.Booking
{
    public class CorporateOrderConfirmationQueryHandler : IQueryHandler<CorporateOrderConfirmationQuery, CorporateOrderConfirmationQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;

        public CorporateOrderConfirmationQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public CorporateOrderConfirmationQueryResult Handle(CorporateOrderConfirmationQuery query)
        {
            var corporateConfirmationDetails = AutoMapper.Mapper.Map<List<CorporateOrderDetails>>(_transactionRepository.GetCorporateConfirmationDetails(query.TransactionId));
            return new CorporateOrderConfirmationQueryResult
            {
                corporateOrderDetails = corporateConfirmationDetails
            };
        }
    }
}