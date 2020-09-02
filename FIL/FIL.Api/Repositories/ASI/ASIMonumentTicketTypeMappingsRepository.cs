using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IASIMonumentTicketTypeMappingsRepository : IOrmRepository<ASIMonumentTicketTypeMapping, ASIMonumentTicketTypeMapping>
    {
        ASIMonumentTicketTypeMapping Get(long id);

        IEnumerable<ASIMonumentTicketTypeMapping> GetByASIDetailId(long asiDetailId);
    }

    public class ASIMonumentTicketTypeMappingsRepository : BaseLongOrmRepository<ASIMonumentTicketTypeMapping>, IASIMonumentTicketTypeMappingsRepository
    {
        public ASIMonumentTicketTypeMappingsRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIMonumentTicketTypeMapping Get(long id)
        {
            return Get(new ASIMonumentTicketTypeMapping { Id = id });
        }

        public IEnumerable<ASIMonumentTicketTypeMapping> GetByASIDetailId(long asiDetailId)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentTicketTypeMapping.ASIMonumentDetailId):C} = @DetailId")
                .WithParameters(new { DetailId = asiDetailId })
            );
        }
    }
}