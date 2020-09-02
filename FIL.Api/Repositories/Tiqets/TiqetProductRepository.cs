using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetProductRepository : IOrmRepository<TiqetProduct, TiqetProduct>
    {
        TiqetProduct Get(long id);

        TiqetProduct GetByProductId(string productId);

        IEnumerable<TiqetProduct> GetAll();
    }

    public class TiqetProductRepository : BaseLongOrmRepository<TiqetProduct>, ITiqetProductRepository
    {
        public TiqetProductRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TiqetProduct Get(long id)
        {
            return Get(new TiqetProduct { Id = id });
        }

        public TiqetProduct GetByProductId(string productId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetProduct.ProductId):C} = @ProductId")
                 .WithParameters(new { ProductId = productId })
             ).FirstOrDefault();
        }

        public IEnumerable<TiqetProduct> GetAll()
        {
            return GetAll(null);
        }
    }
}