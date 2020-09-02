using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetProductCheckoutDetailRepository : IOrmRepository<TiqetProductCheckoutDetail, TiqetProductCheckoutDetail>
    {
        TiqetProductCheckoutDetail Get(long id);

        TiqetProductCheckoutDetail GetByProductId(string productId);
    }

    public class TiqetProductCheckoutDetailRepository : BaseLongOrmRepository<TiqetProductCheckoutDetail>, ITiqetProductCheckoutDetailRepository
    {
        public TiqetProductCheckoutDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TiqetProductCheckoutDetail Get(long id)
        {
            return Get(new TiqetProductCheckoutDetail { Id = id });
        }

        public IEnumerable<TiqetProductCheckoutDetail> GetAll()
        {
            return GetAll(null);
        }

        public TiqetProductCheckoutDetail GetByProductId(string productId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetProductCheckoutDetail.ProductId):C} = @ProductId")
                .WithParameters(new { ProductId = productId })
            ).LastOrDefault();
        }
    }
}