using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASIMonumentTimeSlotMappingRepository : IOrmRepository<ASIMonumentTimeSlotMapping, ASIMonumentTimeSlotMapping>
    {
        ASIMonumentTimeSlotMapping Get(long id);

        IEnumerable<ASIMonumentTimeSlotMapping> GetByASIMonumentId(long asiMonumentId);

        ASIMonumentTimeSlotMapping GetByASIMonumentIdAndTimeId(long asiMonumentId, long timeId);
    }

    public class ASIMonumentTimeSlotMappingRepository : BaseLongOrmRepository<ASIMonumentTimeSlotMapping>, IASIMonumentTimeSlotMappingRepository
    {
        public ASIMonumentTimeSlotMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIMonumentTimeSlotMapping Get(long id)
        {
            return Get(new ASIMonumentTimeSlotMapping { Id = id });
        }

        public IEnumerable<ASIMonumentTimeSlotMapping> GetByASIMonumentId(long asiMonumentId)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentTimeSlotMapping.ASIMonumentId):C} = @MonumentId")
                .WithParameters(new { MonumentId = asiMonumentId })
            );
        }

        public ASIMonumentTimeSlotMapping GetByASIMonumentIdAndTimeId(long asiMonumentId, long timeId)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentTimeSlotMapping.ASIMonumentId):C} = @MonumentId AND {nameof(ASIMonumentTimeSlotMapping.TimeSlotId)} = @TimeId")
               .WithParameters(new { MonumentId = asiMonumentId, TimeId = timeId })
           ).FirstOrDefault();
        }
    }
}