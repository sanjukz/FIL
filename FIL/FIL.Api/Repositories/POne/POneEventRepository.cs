using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneEventRepository : IOrmRepository<POneEvent, POneEvent>
    {
        POneEvent Get(int id);

        POneEvent GetByName(string name);

        POneEvent GetByPOneId(int pOneId);
    }

    public class POneEventRepository : BaseOrmRepository<POneEvent>, IPOneEventRepository
    {
        public POneEventRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneEvent Get(int id)
        {
            return Get(new POneEvent { Id = id });
        }

        public POneEvent GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(POneEvent.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public POneEvent GetByPOneId(int pOneId)
        {
            return GetAll(s => s.Where($"{nameof(POneEvent.POneId):C} = @POneId")
                .WithParameters(new { POneId = pOneId })
            ).FirstOrDefault();
        }
    }
}