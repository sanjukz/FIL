using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneEventTicketAttributeRepository : IOrmRepository<POneEventTicketAttribute, POneEventTicketAttribute>
    {
        POneEventTicketAttribute Get(int id);

        POneEventTicketAttribute GetByPOneEventTicketDetailId(int pOneEventTicketDetailId);

        POneEventTicketAttribute GetByPOneEventTicketAttributeId(long pOneEventTicketAttributeId);
    }

    public class POneEventTicketAttributeRepository : BaseLongOrmRepository<POneEventTicketAttribute>, IPOneEventTicketAttributeRepository
    {
        public POneEventTicketAttributeRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneEventTicketAttribute Get(int id)
        {
            return Get(new POneEventTicketAttribute { Id = id });
        }

        public POneEventTicketAttribute GetByPOneEventTicketDetailId(int pOneEventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventTicketAttribute.POneEventTicketDetailId):C} = @EventTicketDetailId")
                .WithParameters(new
                {
                    EventTicketDetailId = pOneEventTicketDetailId
                })
            ).FirstOrDefault();
        }

        public POneEventTicketAttribute GetByPOneEventTicketAttributeId(long pOneEventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventTicketAttribute.POneId):C} = @Id")
                .WithParameters(new
                {
                    Id = pOneEventTicketAttributeId
                })
            ).FirstOrDefault();
        }
    }
}