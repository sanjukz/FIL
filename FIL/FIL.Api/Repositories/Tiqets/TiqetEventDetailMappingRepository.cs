using Dapper.FastCrud;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetEventDetailMappingRepository : IOrmRepository<TiqetEventDetailMapping, TiqetEventDetailMapping>
    {
        TiqetEventDetailMapping Get(long id);

        TiqetEventDetailMapping GetByProductId(string productId);

        TiqetEventDetailMapping GetByEventDetailId(long eventDetailId);

        IEnumerable<TiqetEventDetailMapping> GetAll();

        void DisableAll();

        IEnumerable<TiqetEventDetailMapping> GetAllDisabled();
    }

    public class TiqetEventDetailMappingRepository : BaseLongOrmRepository<TiqetEventDetailMapping>, ITiqetEventDetailMappingRepository
    {
        public TiqetEventDetailMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TiqetEventDetailMapping Get(long id)
        {
            return Get(new TiqetEventDetailMapping { Id = id });
        }

        public TiqetEventDetailMapping GetByProductId(string productId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetEventDetailMapping.ProductId):C} = @ProductId")
                 .WithParameters(new { ProductId = productId })
             ).FirstOrDefault();
        }

        public TiqetEventDetailMapping GetByEventDetailId(long eventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetEventDetailMapping.EventDetailId):C} = @EventDetailId")
                 .WithParameters(new { EventDetailId = eventDetailId })
             ).FirstOrDefault();
        }

        public IEnumerable<TiqetEventDetailMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DisableAll()
        {
            var partialUpdateMapping = OrmConfiguration
      .GetDefaultEntityMapping<TiqetEventDetailMapping>()
      .Clone() // clone it if you don't want to modify the default
      .UpdatePropertiesExcluding(prop => prop.IsExcludedFromUpdates = true, nameof(TiqetEventDetailMapping.IsEnabled));

            GetCurrentConnection().BulkUpdate(
                new TiqetEventDetailMapping
                {
                    IsEnabled = false,
                },
                statement => statement.WithEntityMappingOverride(partialUpdateMapping)
                .AttachToTransaction(GetCurrentTransaction()));
        }

        public IEnumerable<TiqetEventDetailMapping> GetAllDisabled()
        {
            return GetAll(s => s.Where($"{nameof(TiqetEventDetailMapping.IsEnabled):C} = 0")
                   .WithParameters(new { }));
        }
    }
}