using FIL.Api.Repositories;
using FIL.Contracts.Queries.Invoice;
using FIL.Contracts.QueryResults.Invoice;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS.Invoice
{
    public class CompanyDetailsQueryHandler : IQueryHandler<CompanyDetailsQuery, CompanyDetailsQueryResult>
    {
        private readonly ICompanyDetailRepository _companyDetailRepository;

        public CompanyDetailsQueryHandler(ICompanyDetailRepository companyDetailRepository)
        {
            _companyDetailRepository = companyDetailRepository;
        }

        public CompanyDetailsQueryResult Handle(CompanyDetailsQuery query)
        {
            try
            {
                var companyDetails = _companyDetailRepository.GetAll().ToList();
                return new CompanyDetailsQueryResult
                {
                    CompanyDetails = companyDetails
                };
            }
            catch (Exception ex)
            {
                return new CompanyDetailsQueryResult
                {
                    CompanyDetails = null
                };
            }
        }
    }
}