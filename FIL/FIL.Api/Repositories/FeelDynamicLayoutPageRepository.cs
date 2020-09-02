using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFeelDynamicLayoutPageRepository : IOrmRepository<PageView, PageView>
    {
        PageView Get(int id);

        PageView GetByName(string name);
    }

    public class FeelDynamicLayoutPageRepository : BaseOrmRepository<PageView>, IFeelDynamicLayoutPageRepository
    {
        public FeelDynamicLayoutPageRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public PageView Get(int id)
        {
            return Get(new PageView { Id = id });
        }

        public IEnumerable<PageView> GetAll()
        {
            return GetAll(null);
        }

        public PageView GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(PageView.PageName):C} = @PageName")
                .WithParameters(new { PageName = name })
            ).FirstOrDefault();
        }
    }
}