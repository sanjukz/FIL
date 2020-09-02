using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Redemption
{
    public interface IGuidePlaceMappingsRepository : IOrmRepository<Redemption_GuidePlaceMappings, Redemption_GuidePlaceMappings>
    {
        Redemption_GuidePlaceMappings Get(int Id);

        IEnumerable<Redemption_GuidePlaceMappings> GetAllByGuideId(long GuideId);
    }

    public class GuidePlaceMappingsRepository : BaseLongOrmRepository<Redemption_GuidePlaceMappings>, IGuidePlaceMappingsRepository
    {
        public GuidePlaceMappingsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public IEnumerable<Redemption_GuidePlaceMappings> GetAllByGuideId(long GuideId)
        {
            return GetAll(s => s.Where($"{nameof(Redemption_GuidePlaceMappings.GuideId):C} = @GuideId").WithParameters(new { GuideId = GuideId }));
        }

        public Redemption_GuidePlaceMappings Get(int Id)
        {
            return Get(new Redemption_GuidePlaceMappings { Id = Id });
        }

        public IEnumerable<Redemption_GuidePlaceMappings> GetAll()
        {
            return GetAll(null);
        }
    }
}