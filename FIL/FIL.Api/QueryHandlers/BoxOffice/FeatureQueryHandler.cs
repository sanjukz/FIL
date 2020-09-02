using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class FeatureQueryHandler : IQueryHandler<FeatureQuery, FeatureQueryResult>
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly IRoleFeatureMappingRepository _roleFeatureMappingRepository;

        public FeatureQueryHandler(IFeatureRepository featureRepository, IRoleFeatureMappingRepository roleFeatureMappingRepository)
        {
            _featureRepository = featureRepository;
            _roleFeatureMappingRepository = roleFeatureMappingRepository;
        }

        public FeatureQueryResult Handle(FeatureQuery query)
        {
            var featureId = query.RoleId == 1 ?
                _featureRepository.GetAll().Where(w => w.ModuleId == FIL.Contracts.Enums.Modules.KzSuite).Select(s => s.Id)
                : _roleFeatureMappingRepository.GetByRoleId(query.RoleId).Select(s => s.FeatureId).Distinct();
            var featureList = _featureRepository.GetById(featureId);
            return new FeatureQueryResult
            {
                Features = AutoMapper.Mapper.Map<List<Feature>>(featureList)
            };
        }
    }
}