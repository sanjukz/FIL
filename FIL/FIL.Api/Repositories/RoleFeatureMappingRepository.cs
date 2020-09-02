using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IRoleFeatureMappingRepository : IOrmRepository<RoleFeatureMapping, RoleFeatureMapping>
    {
        RoleFeatureMapping Get(int id);

        IEnumerable<RoleFeatureMapping> GetByRoleId(int id);
    }

    public class RoleFeatureMappingRepository : BaseOrmRepository<RoleFeatureMapping>, IRoleFeatureMappingRepository
    {
        public RoleFeatureMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public RoleFeatureMapping Get(int id)
        {
            return Get(new RoleFeatureMapping { Id = id });
        }

        public IEnumerable<RoleFeatureMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteRoleFeatureMapping(RoleFeatureMapping roleFeatureMapping)
        {
            Delete(roleFeatureMapping);
        }

        public RoleFeatureMapping SaveRoleFeatureMapping(RoleFeatureMapping roleFeatureMapping)
        {
            return Save(roleFeatureMapping);
        }

        public IEnumerable<RoleFeatureMapping> GetByRoleId(int id)
        {
            return GetAll(s => s.Where($"{nameof(RoleFeatureMapping.RoleId):C} = @Id  AND IsEnabled=1")
                .WithParameters(new { Id = id })
            );
        }
    }
}