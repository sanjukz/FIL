using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetProductImageRepository : IOrmRepository<TiqetProductImage, TiqetProductImage>
    {
        IEnumerable<TiqetProductImage> GetByProductId(string productId);
    }

    public class TiqetProductImageRepository : BaseLongOrmRepository<TiqetProductImage>, ITiqetProductImageRepository
    {
        public TiqetProductImageRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public IEnumerable<TiqetProductImage> GetByProductId(string productId)
        {
            var imagesList = (GetAll(s => s.Where($"{nameof(TiqetProductImage.ProductId):C} =@ProductId")
                   .WithParameters(new { ProductId = productId })
            ));
            return imagesList;
        }
    }
}