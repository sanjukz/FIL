using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzRegionRepository : IOrmRepository<ExOzRegion, ExOzRegion>
    {
        ExOzRegion Get(int id);

        IEnumerable<ExOzRegion> GetAll();

        ExOzRegion GetByName(string name);

        void DeleteRegion(ExOzRegion exOzRegion);

        ExOzRegion SaveRegion(ExOzRegion exOzRegion);

        List<ExOzRegion> DisableAllExOzRegions();

        ExOzRegion GetByUrlSegment(string urlSegment);

        IEnumerable<ExOzRegion> GetByNames(List<string> names);
    }

    public class ExOzRegionRepository : BaseOrmRepository<ExOzRegion>, IExOzRegionRepository
    {
        public ExOzRegionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzRegion Get(int id)
        {
            return Get(new ExOzRegion { Id = id });
        }

        public IEnumerable<ExOzRegion> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteRegion(ExOzRegion exOzRegion)
        {
            Delete(exOzRegion);
        }

        public ExOzRegion SaveRegion(ExOzRegion exOzRegion)
        {
            return Save(exOzRegion);
        }

        public ExOzRegion GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ExOzRegion.Name):C}=@Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public List<ExOzRegion> DisableAllExOzRegions()
        {
            List<ExOzRegion> allExOzRegions = this.GetAll().ToList();
            foreach (var region in allExOzRegions)
            {
                region.IsEnabled = false;
                Save(region);
            }
            return allExOzRegions;
        }

        public ExOzRegion GetByUrlSegment(string urlSegment)
        {
            return GetAll(s => s.Where($"{nameof(ExOzRegion.UrlSegment):C}=@UrlSegment")
                .WithParameters(new { UrlSegment = urlSegment })
            ).FirstOrDefault();
        }

        public IEnumerable<ExOzRegion> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(ExOzRegion.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }
    }
}