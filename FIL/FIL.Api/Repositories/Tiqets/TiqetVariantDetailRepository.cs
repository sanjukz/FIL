using Dapper.FastCrud;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetVariantDetailRepository : IOrmRepository<TiqetVariantDetail, TiqetVariantDetail>
    {
        TiqetVariantDetail Get(long id);

        IEnumerable<TiqetVariantDetail> GetAllByProductId(string productId);

        IEnumerable<TiqetVariantDetail> GetAll();

        TiqetVariantDetail GetByVariantIdAndProductId(int varinatId, string productId);

        void DisableAllVariants(string productId);
    }

    public class TiqetVariantDetailRepository : BaseLongOrmRepository<TiqetVariantDetail>, ITiqetVariantDetailRepository
    {
        public TiqetVariantDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TiqetVariantDetail Get(long id)
        {
            return Get(new TiqetVariantDetail { Id = id });
        }

        public TiqetVariantDetail GetByVariantIdAndProductId(int variantId, string productId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetVariantDetail.VariantId):C} = @VariantId AND {nameof(TiqetVariantDetail.ProductId)} = @ProductId")
                .WithParameters(new { VariantId = variantId, ProductId = productId })
            ).FirstOrDefault();
        }

        public IEnumerable<TiqetVariantDetail> GetAllByProductId(string productId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(TiqetVariantDetail.ProductId):C}=@Ids AND IsEnabled=1")
                .WithParameters(new { Ids = productId }));
        }

        public IEnumerable<TiqetVariantDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DisableAllVariants(string productId)
        {
            var partialUpdateMapping = OrmConfiguration
      .GetDefaultEntityMapping<TiqetVariantDetail>()
      .Clone() // clone it if you don't want to modify the default
      .UpdatePropertiesExcluding(prop => prop.IsExcludedFromUpdates = true, nameof(TiqetVariantDetail.IsEnabled));

            GetCurrentConnection().BulkUpdate(
                new TiqetVariantDetail
                {
                    IsEnabled = false,
                }, statement => statement.WithEntityMappingOverride(partialUpdateMapping).Where($"{nameof(TiqetVariantDetail.ProductId):C}=@ProductId").WithParameters(new { ProductId = productId }).AttachToTransaction(GetCurrentTransaction()));
        }
    }
}