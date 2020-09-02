using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzProductRepository : IOrmRepository<ExOzProduct, ExOzProduct>
    {
        ExOzProduct Get(long id);

        ExOzProduct GetByName(string name);

        ExOzProduct GetByUrlSegment(string urlSegment);

        List<ExOzProduct> DisableAllExOzProducts();

        IEnumerable<ExOzProduct> GetByNames(List<string> names);

        ExOzProduct GetByProductId(long productId);
    }

    public class ExOzProductRepository : BaseLongOrmRepository<ExOzProduct>, IExOzProductRepository
    {
        public ExOzProductRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzProduct Get(long id)
        {
            return Get(new ExOzProduct { Id = id });
        }

        public IEnumerable<ExOzProduct> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteProduct(ExOzProduct exOzProduct)
        {
            Delete(exOzProduct);
        }

        public ExOzProduct SaveProduct(ExOzProduct exOzProduct)
        {
            return Save(exOzProduct);
        }

        public ExOzProduct GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProduct.Name):C}=@Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public ExOzProduct GetByUrlSegment(string urlSegment)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProduct.UrlSegment):C}=@UrlSegment")
                .WithParameters(new { UrlSegment = urlSegment })
            ).FirstOrDefault();
        }

        public List<ExOzProduct> DisableAllExOzProducts()
        {
            List<ExOzProduct> allExOzProducts = this.GetAll().ToList();
            foreach (var op in allExOzProducts)
            {
                op.IsEnabled = false;
                Save(op);
            }
            return allExOzProducts;
        }

        public IEnumerable<ExOzProduct> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProduct.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }

        public ExOzProduct GetByProductId(long productId)
        {
            return GetAll(s => s.Where($"{nameof(ExOzProduct.ProductId):C} = @ProductId")
                .WithParameters(new { ProductId = productId })
            ).FirstOrDefault();
        }
    }
}