using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneEventSubCategoryRepository : IOrmRepository<POneEventSubCategory, POneEventSubCategory>
    {
        POneEventSubCategory Get(int id);

        POneEventSubCategory GetByNameAndPOneEventCategoryId(string name, int pOneEventCategoryId);

        POneEventSubCategory GetByPOneEventCategoryId(int pOneEventCategoryId);

        POneEventSubCategory GetByPOneEventSubCategoryId(int pOneEventSubCategoryId);
    }

    public class POneEventSubCategoryRepository : BaseOrmRepository<POneEventSubCategory>, IPOneEventSubCategoryRepository
    {
        public POneEventSubCategoryRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneEventSubCategory Get(int id)
        {
            return Get(new POneEventSubCategory { Id = id });
        }

        public POneEventSubCategory GetByNameAndPOneEventCategoryId(string name, int pOneEventCategoryId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventSubCategory.Name):C} = @Name AND {nameof(POneEventSubCategory.POneEventCategoryId):C} = @EventCategoryId")
                .WithParameters(new { Name = name, EventCategoryId = pOneEventCategoryId })
            ).FirstOrDefault();
        }

        public POneEventSubCategory GetByPOneEventCategoryId(int pOneEventCategoryId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventSubCategory.POneEventCategoryId):C} = @Id")
                .WithParameters(new { Id = pOneEventCategoryId })
            ).FirstOrDefault();
        }

        public POneEventSubCategory GetByPOneEventSubCategoryId(int pOneEventSubCategoryId)
        {
            return GetAll(s => s.Where($"{nameof(POneEventSubCategory.Id):C} = @Id")
                .WithParameters(new { Id = pOneEventSubCategoryId })
            ).FirstOrDefault();
        }
    }
}