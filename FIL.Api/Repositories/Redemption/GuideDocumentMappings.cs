using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Redemption
{
    public interface IGuideDocumentMappingsRepository : IOrmRepository<Redemption_GuideDocumentMappings, Redemption_GuideDocumentMappings>
    {
        Redemption_GuideDocumentMappings Get(int Id);

        IEnumerable<Redemption_GuideDocumentMappings> GetAllByGuideId(long GuideId);
    }

    public class GuideDocumentMappingsRepository : BaseLongOrmRepository<Redemption_GuideDocumentMappings>, IGuideDocumentMappingsRepository
    {
        public GuideDocumentMappingsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public IEnumerable<Redemption_GuideDocumentMappings> GetAllByGuideId(long GuideId)
        {
            return GetAll(s => s.Where($"{nameof(Redemption_GuideDocumentMappings.GuideId):C} = @GuideId").WithParameters(new { GuideId = GuideId }));
        }

        public Redemption_GuideDocumentMappings Get(int Id)
        {
            return Get(new Redemption_GuideDocumentMappings { Id = Id });
        }

        public IEnumerable<Redemption_GuideDocumentMappings> GetAll()
        {
            return GetAll(null);
        }
    }
}