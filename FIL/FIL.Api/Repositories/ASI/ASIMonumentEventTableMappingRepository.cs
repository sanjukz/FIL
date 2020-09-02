using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASIMonumentEventTableMappingRepository : IOrmRepository<ASIMonumentEventTableMapping, ASIMonumentEventTableMapping>
    {
        ASIMonumentEventTableMapping Get(long id);

        ASIMonumentEventTableMapping GetByMonumentId(long id);

        ASIMonumentEventTableMapping GetByEventId(long id);
    }

    public class ASIMonumentEventTableMappingRepository : BaseLongOrmRepository<ASIMonumentEventTableMapping>, IASIMonumentEventTableMappingRepository
    {
        public ASIMonumentEventTableMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIMonumentEventTableMapping Get(long id)
        {
            return Get(new ASIMonumentEventTableMapping { Id = id });
        }

        public ASIMonumentEventTableMapping GetByMonumentId(long id)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentEventTableMapping.ASIMonumentId):C} = @MonumentId")
                .WithParameters(new { MonumentId = id })
            ).FirstOrDefault();
        }

        public ASIMonumentEventTableMapping GetByEventId(long id)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentEventTableMapping.EventId):C} = @EventId")
                .WithParameters(new { EventId = id })
            ).FirstOrDefault();
        }
    }
}