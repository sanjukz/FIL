using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzProductImageRepository : IOrmRepository<ExOzProductImage, ExOzProductImage>
    {
        ExOzProductImage Get(long id);

        List<ExOzProductImage> DisableAllExOzProductImages();

        IEnumerable<ExOzProductImage> GetByProductId(long productId);

        IEnumerable<ExOzProductImage> GetByEventDetailIds(IEnumerable<long> eventDetailIds);
    }

    public class ExOzProductImageRepository : BaseLongOrmRepository<ExOzProductImage>, IExOzProductImageRepository
    {
        public ExOzProductImageRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzProductImage Get(long id)
        {
            return Get(new ExOzProductImage { Id = id });
        }

        public IEnumerable<ExOzProductImage> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteProductImage(ExOzProductImage exOzProductImage)
        {
            Delete(exOzProductImage);
        }

        public ExOzProductImage SaveProductImage(ExOzProductImage exOzProductImage)
        {
            return Save(exOzProductImage);
        }

        public List<ExOzProductImage> DisableAllExOzProductImages()
        {
            List<ExOzProductImage> allExOzProductImages = this.GetAll().ToList();
            foreach (var productImage in allExOzProductImages)
            {
                productImage.IsEnabled = false;
                Save(productImage);
            }
            return allExOzProductImages;
        }

        public IEnumerable<ExOzProductImage> GetByProductId(long productId)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProductImage.ProductId):C} = @ProductId")
                .WithParameters(new { ProductId = productId })
            );
        }

        public IEnumerable<ExOzProductImage> GetByEventDetailIds(IEnumerable<long> eventDetailIds)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(ExOzProductImage.EventDetailId):C} IN @Ids")
                    .WithParameters(new { Ids = eventDetailIds }));
        }
    }
}