using FIL.Api.Repositories;
using FIL.Contracts.Queries.RefundPolicy;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.RefundPolicies
{
    public class RefundPoliciesQueryHandler : IQueryHandler<RefundPolicyQuery, RefundPolicyQueryResult>
    {
        private readonly IRefundPolicyRepository _refundPolicyRepository;
        private readonly FIL.Logging.ILogger _logger;

        public RefundPoliciesQueryHandler(IRefundPolicyRepository refundPolicyRepository, FIL.Logging.ILogger logger)
        {
            _refundPolicyRepository = refundPolicyRepository;
            _logger = logger;
        }

        public RefundPolicyQueryResult Handle(RefundPolicyQuery query)
        {
            var refundPolicies = _refundPolicyRepository.GetAll();
            var refundPoliciesModel = AutoMapper.Mapper.Map<List<Contracts.Models.RefundPolicies>>(refundPolicies).ToList();

            return new RefundPolicyQueryResult
            {
                RefundPolicies = refundPoliciesModel
            };
        }
    }
}