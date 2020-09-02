using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Redemption
{
    public interface IGuideFinanceMappingsRepository : IOrmRepository<Redemption_GuideFinanceMappings, Redemption_GuideFinanceMappings>
    {
        Redemption_GuideFinanceMappings Get(int Id);

        IEnumerable<Redemption_GuideFinanceMappings> GetAllByGuideId(long GuideId);
    }

    public class GuideFinanceMappingsRepository : BaseLongOrmRepository<Redemption_GuideFinanceMappings>, IGuideFinanceMappingsRepository
    {
        public IEnumerable<Redemption_GuideFinanceMappings> GetAllByGuideId(long GuideId)
        {
            return GetAll(s => s.Where($"{nameof(Redemption_GuideFinanceMappings.GuideId):C} = @GuideId").WithParameters(new { GuideId = GuideId }));
        }

        public GuideFinanceMappingsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Redemption_GuideFinanceMappings Get(int Id)
        {
            return Get(new Redemption_GuideFinanceMappings { Id = Id });
        }

        public IEnumerable<Redemption_GuideFinanceMappings> GetAll()
        {
            return GetAll(null);
        }
    }
}