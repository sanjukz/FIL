using Dapper.FastCrud;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetEventTicketDetailMappingRepository : IOrmRepository<TiqetEventTicketDetailMapping, TiqetEventTicketDetailMapping>
    {
        TiqetEventTicketDetailMapping Get(long id);

        IEnumerable<TiqetEventTicketDetailMapping> GetAll();

        TiqetEventTicketDetailMapping GetByTiqetVariantId(long variantId);

        TiqetEventTicketDetailMapping GetByEventTicketDetailId(long eventTicketDetailId);

        void DisableAllVariants(string productId);

        IEnumerable<TiqetEventTicketDetailMapping> GetAllDisabled(string productId);
    }

    public class TiqetEventTicketDetailMappingRepository : BaseLongOrmRepository<TiqetEventTicketDetailMapping>, ITiqetEventTicketDetailMappingRepository
    {
        public TiqetEventTicketDetailMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TiqetEventTicketDetailMapping Get(long id)
        {
            return Get(new TiqetEventTicketDetailMapping { Id = id });
        }

        public IEnumerable<TiqetEventTicketDetailMapping> GetAll()
        {
            return GetAll(null);
        }

        public TiqetEventTicketDetailMapping GetByTiqetVariantId(long variantId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetEventTicketDetailMapping.TiqetVariantDetailId):C} = @Id")
                 .WithParameters(new { Id = variantId })
             ).FirstOrDefault();
        }

        public TiqetEventTicketDetailMapping GetByEventTicketDetailId(long eventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetEventTicketDetailMapping.EventTicketDetailId):C} = @Id")
                 .WithParameters(new { Id = eventTicketDetailId })
             ).FirstOrDefault();
        }

        public IEnumerable<TiqetEventTicketDetailMapping> GetAllDisabled(string productId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(TiqetEventTicketDetailMapping.ProductId):C}=@Ids AND IsEnabled=0")
                .WithParameters(new { Ids = productId }));
        }

        public void DisableAllVariants(string productId)
        {
            var partialUpdateMapping = OrmConfiguration
      .GetDefaultEntityMapping<TiqetEventTicketDetailMapping>()
      .Clone() // clone it if you don't want to modify the default
      .UpdatePropertiesExcluding(prop => prop.IsExcludedFromUpdates = true, nameof(TiqetVariantDetail.IsEnabled));

            GetCurrentConnection().BulkUpdate(
                new TiqetEventTicketDetailMapping
                {
                    IsEnabled = false,
                },
                statement => statement.WithEntityMappingOverride(partialUpdateMapping)
                .Where($"{nameof(TiqetEventTicketDetailMapping.ProductId):C}=@ProductId")
                .WithParameters(new { ProductId = productId }).AttachToTransaction(GetCurrentTransaction()));
        }
    }
}