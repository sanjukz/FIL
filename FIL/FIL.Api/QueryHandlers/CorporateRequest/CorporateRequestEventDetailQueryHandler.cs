using FIL.Api.Repositories;
using FIL.Contracts.Queries.CorporateRequest;
using FIL.Contracts.QueryResults.CorporateRequest;
using System;

namespace FIL.Api.QueryHandlers.CorporateRequest
{
    public class CorporateRequestEventDetailQueryHandler : IQueryHandler<CorporateRequestEventDetailQuery, CorporateRequestEventDetailQueryResult>
    {
        private readonly ICorporateRequestProcRepository _corporateRequestProcRepository;

        public CorporateRequestEventDetailQueryHandler(ICorporateRequestProcRepository corporateRequestProcRepository)
        {
            _corporateRequestProcRepository = corporateRequestProcRepository;
        }

        public CorporateRequestEventDetailQueryResult Handle(CorporateRequestEventDetailQuery query)
        {
            FIL.Contracts.DataModels.CorporateRequestProcData corporateRequest = new FIL.Contracts.DataModels.CorporateRequestProcData();
            try
            {
                corporateRequest = _corporateRequestProcRepository.GetEventDetailData();
                return new CorporateRequestEventDetailQueryResult
                {
                    CorporateRequestData = corporateRequest
                };
            }
            catch (Exception e)
            {
                return new CorporateRequestEventDetailQueryResult
                {
                    CorporateRequestData = corporateRequest
                };
            }
        }
    }
}