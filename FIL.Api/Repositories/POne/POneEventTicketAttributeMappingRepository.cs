using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneEventTicketAttributeMappingRepository : IOrmRepository<POneEventTicketAttributeMapping, POneEventTicketAttributeMapping>
    {
        POneEventTicketAttributeMapping Get(int id);

        POneEventTicketAttributeMapping GetPOneMappedEventAttribute(long eventTicketAttributeId);
    }

    public class POneEventTicketAttributeMappingRepository : BaseOrmRepository<POneEventTicketAttributeMapping>, IPOneEventTicketAttributeMappingRepository
    {
        public POneEventTicketAttributeMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneEventTicketAttributeMapping Get(int id)
        {
            return Get(new POneEventTicketAttributeMapping { Id = id });
        }

        public POneEventTicketAttributeMapping GetPOneMappedEventAttribute(long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventTicketAttributeMapping.ZoongaEventTicketAttributeId):C} = @EventTicketAttributeIdParam")
                .WithParameters(new { EventTicketAttributeIdParam = eventTicketAttributeId })
            ).FirstOrDefault();
        }

        public IEnumerable<POneEventTicketAttributeMapping> GetByEventTicketAttributeIds(IEnumerable<long> ids)
        {
            return GetAll(statement => statement
                        .Where($"{nameof(POneEventTicketAttributeMapping.ZoongaEventTicketAttributeId):C} IN @EventTicketAttributeIds")
                        .WithParameters(new { EventTicketAttributeIds = ids }));
        }
    }
}