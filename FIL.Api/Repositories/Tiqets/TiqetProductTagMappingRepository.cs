using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetProductTagMappingRepository : IOrmRepository<TiqetProductTagMapping, TiqetProductTagMapping>
    {
        IEnumerable<TiqetProductTagMapping> GetByProductId(string productId);
    }

    public class TiqetProductTagMappingRepository : BaseLongOrmRepository<TiqetProductTagMapping>, ITiqetProductTagMappingRepository
    {
        public TiqetProductTagMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public IEnumerable<TiqetProductTagMapping> GetByProductId(string productId)
        {
            var imagesList = (GetAll(s => s.Where($"{nameof(TiqetProductImage.ProductId):C} =@ProductId")
                   .WithParameters(new { ProductId = productId })
            ));
            return imagesList;
        }
    }
}