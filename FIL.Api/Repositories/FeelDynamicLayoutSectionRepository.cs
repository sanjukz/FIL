using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFeelDynamicLayoutSectionRepository : IOrmRepository<SectionView, SectionView>
    {
        SectionView Get(int id);

        SectionView GetByName(string name);

        List<SectionView> GetAllSectionsByPageId(int pageId);
    }

    public class FeelDynamicLayoutSectionRepository : BaseOrmRepository<SectionView>, IFeelDynamicLayoutSectionRepository
    {
        public FeelDynamicLayoutSectionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public SectionView Get(int id)
        {
            return Get(new SectionView { Id = id });
        }

        public IEnumerable<SectionView> GetAll()
        {
            return GetAll(null);
        }

        public SectionView GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(SectionView.SectionName):C} = @PageName AND IsEnabled=1")
                .WithParameters(new { PageName = name })
            ).FirstOrDefault();
        }

        public List<SectionView> GetAllSectionsByPageId(int pageId)
        {
            return GetAll(s => s.Where($"{nameof(SectionView.PageViewId):C} = @PageViewId AND IsEnabled=1")
                .WithParameters(new { PageViewId = pageId })).ToList();
        }
    }
}