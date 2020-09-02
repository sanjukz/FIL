using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface IRefundPolicyRepository : IOrmRepository<RefundPolicies, RefundPolicies>
    {
        RefundPolicies Get(long id);
    }

    public class RefundPolicyRepository : BaseLongOrmRepository<RefundPolicies>, IRefundPolicyRepository
    {
        public RefundPolicyRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public RefundPolicies Get(long id)
        {
            return Get(new RefundPolicies { Id = id });
        }
    }
}