using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneEventDetailMappingRepository : IOrmRepository<POneEventDetailMapping, POneEventDetailMapping>
    {
        POneEventDetailMapping Get(int id);

        POneEventDetailMapping GetPOneMappedEventDetails(int pOneEventDetailId);

        IEnumerable<POneEventDetailMapping> GetByEventDetailIds(List<long> eventDetailIds);

        POneEventDetailMapping GetByEventDetailId(long eventDetailId);

        IEnumerable<POneEventDetailMapping> GetByEventDetailId(List<long> eventDetailIds);
    }

    public class POneEventDetailMappingRepository : BaseOrmRepository<POneEventDetailMapping>, IPOneEventDetailMappingRepository
    {
        public POneEventDetailMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneEventDetailMapping Get(int id)
        {
            return Get(new POneEventDetailMapping { Id = id });
        }

        public POneEventDetailMapping GetPOneMappedEventDetails(int pOneEventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventDetailMapping.POneEventDetailId):C} = @POneEventDetailId")
                .WithParameters(new { POneEventDetailId = pOneEventDetailId })
            ).FirstOrDefault();
        }

        public IEnumerable<POneEventDetailMapping> GetByEventDetailIds(List<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(POneEventDetailMapping.ZoongaEventDetailId):C} IN @POneEventDetailIds")
                .WithParameters(new { POneEventDetailIds = eventDetailIds })
            );
        }

        public POneEventDetailMapping GetByEventDetailId(long eventDetailId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventDetailMapping.ZoongaEventDetailId):C} = @EventDetailId")
                .WithParameters(new { EventDetailId = eventDetailId })
            ).FirstOrDefault();
        }

        public IEnumerable<POneEventDetailMapping> GetByEventDetailId(List<long> eventDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(POneEventDetailMapping.ZoongaEventDetailId):C} IN @EventDetailIds")
                .WithParameters(new { EventDetailIds = eventDetailIds })
            );
        }
    }
}