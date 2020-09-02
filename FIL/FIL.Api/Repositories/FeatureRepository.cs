using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IFeatureRepository : IOrmRepository<Feature, Feature>
    {
        Feature Get(int id);

        IEnumerable<Feature> GetById(IEnumerable<int> featureId);

        IEnumerable<Feature> GetByIds(IEnumerable<int> id);
    }

    public class FeatureRepository : BaseOrmRepository<Feature>, IFeatureRepository
    {
        public FeatureRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Feature Get(int id)
        {
            return Get(new Feature { Id = id });
        }

        public IEnumerable<Feature> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteFeature(Feature feature)
        {
            Delete(feature);
        }

        public Feature SaveFeature(Feature feature)
        {
            return Save(feature);
        }

        public IEnumerable<Feature> GetById(IEnumerable<int> featureId)
        {
            return GetAll(s => s.Where($"{nameof(Feature.Id):C} IN @FeatureId")
          .WithParameters(new { FeatureId = featureId }));
        }

        public IEnumerable<Feature> GetByIds(IEnumerable<int> id)
        {
            return GetAll(s => s.Where($"{nameof(Feature.Id):C} IN @Id AND IsEnabled=1")
                .WithParameters(new { Id = id })
            );
        }
    }
}