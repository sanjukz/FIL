using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneEventCategoryRepository : IOrmRepository<POneEventCategory, POneEventCategory>
    {
        POneEventCategory Get(int id);

        POneEventCategory GetByName(string name);
    }

    public class POneEventCategoryRepository : BaseOrmRepository<POneEventCategory>, IPOneEventCategoryRepository
    {
        public POneEventCategoryRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneEventCategory Get(int id)
        {
            return Get(new POneEventCategory { Id = id });
        }

        public POneEventCategory GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(POneEventCategory.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }
    }
}