using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneTicketCategoryRepository : IOrmRepository<POneTicketCategory, POneTicketCategory>
    {
        POneTicketCategory Get(int id);

        POneTicketCategory GetByName(string name);

        POneTicketCategory GetByPOneId(int pOneId);
    }

    public class POneTicketCategoryRepository : BaseOrmRepository<POneTicketCategory>, IPOneTicketCategoryRepository
    {
        public POneTicketCategoryRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneTicketCategory Get(int id)
        {
            return Get(new POneTicketCategory { Id = id });
        }

        public POneTicketCategory GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(POneTicketCategory.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public POneTicketCategory GetByPOneId(int pOneId)
        {
            return GetAll(s => s.Where($"{nameof(POneTicketCategory.POneId):C} = @POneId")
                .WithParameters(new { POneId = pOneId })
            ).FirstOrDefault();
        }
    }
}