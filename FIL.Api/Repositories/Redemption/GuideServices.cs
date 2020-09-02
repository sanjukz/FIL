using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Redemption
{
    public interface IGuideServicesRepository : IOrmRepository<Redemption_GuideServices, Redemption_GuideServices>
    {
        Redemption_GuideServices Get(int Id);

        IEnumerable<Redemption_GuideServices> GetAllByGuideId(long GuideId);
    }

    public class GuideServicesRepository : BaseLongOrmRepository<Redemption_GuideServices>, IGuideServicesRepository
    {
        public GuideServicesRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public IEnumerable<Redemption_GuideServices> GetAllByGuideId(long GuideId)
        {
            return GetAll(s => s.Where($"{nameof(Redemption_GuideServices.GuideId):C} = @GuideId").WithParameters(new { GuideId = GuideId }));
        }

        public Redemption_GuideServices Get(int Id)
        {
            return Get(new Redemption_GuideServices { Id = Id });
        }

        public IEnumerable<Redemption_GuideServices> GetAll()
        {
            return GetAll(null);
        }
    }
}