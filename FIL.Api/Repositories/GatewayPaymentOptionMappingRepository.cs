using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IGatewayPaymentOptionMappingRepository : IOrmRepository<GatewayPaymentOptionMapping, GatewayPaymentOptionMapping>
    {
        GatewayPaymentOptionMapping Get(int id);
    }

    public class GatewayPaymentOptionMappingRepository : BaseOrmRepository<GatewayPaymentOptionMapping>, IGatewayPaymentOptionMappingRepository
    {
        public GatewayPaymentOptionMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public GatewayPaymentOptionMapping Get(int id)
        {
            return Get(new GatewayPaymentOptionMapping { Id = id });
        }

        public IEnumerable<GatewayPaymentOptionMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteGatewayPaymentOptionMapping(GatewayPaymentOptionMapping gatewayPaymentOptionMapping)
        {
            Delete(gatewayPaymentOptionMapping);
        }

        public GatewayPaymentOptionMapping SaveGatewayPaymentOptionMapping(GatewayPaymentOptionMapping gatewayPaymentOptionMapping)
        {
            return Save(gatewayPaymentOptionMapping);
        }
    }
}