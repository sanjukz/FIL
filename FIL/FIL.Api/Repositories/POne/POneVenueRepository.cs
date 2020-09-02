using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPOneVenueRepository : IOrmRepository<POneVenue, POneVenue>
    {
        POneVenue Get(int id);

        POneVenue GetByName(string name);
    }

    public class POneVenueRepository : BaseOrmRepository<POneVenue>, IPOneVenueRepository
    {
        public POneVenueRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneVenue Get(int id)
        {
            return Get(new POneVenue { Id = id });
        }

        public POneVenue GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(POneVenue.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }
    }
}