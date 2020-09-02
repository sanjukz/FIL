using FIL.Api.Repositories;
using FIL.Contracts.Queries.Invoice;
using FIL.Contracts.QueryResults.Invoice;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS.Invoice
{
    public class BankDetailsQueryHandler : IQueryHandler<BankDetailsQuery, BankDetailsQueryResult>
    {
        private readonly IBankDetailRepository _bankDetailRepository;

        public BankDetailsQueryHandler(IBankDetailRepository bankDetailRepository)
        {
            _bankDetailRepository = bankDetailRepository;
        }

        public BankDetailsQueryResult Handle(BankDetailsQuery query)
        {
            try
            {
                var bankDetails = _bankDetailRepository.GetAll().ToList();
                return new BankDetailsQueryResult
                {
                    BankDetails = bankDetails
                };
            }
            catch (Exception ex)
            {
                return new BankDetailsQueryResult
                {
                    BankDetails = null
                };
            }
        }
    }
}