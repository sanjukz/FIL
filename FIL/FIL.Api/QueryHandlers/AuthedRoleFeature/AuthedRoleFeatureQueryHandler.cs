using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.AuthedRoleFeature;
using FIL.Contracts.QueryResults.AuthedRoleFeature;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TransactionReport
{
    public class AuthedRoleFeatureQueryHandler : IQueryHandler<AuthedRoleFeatureQuery, AuthedRoleFeatureQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleFeatureMappingRepository _roleFeatureMappingRepository;
        private readonly IFeatureRepository _featureRepository;

        public AuthedRoleFeatureQueryHandler(IUserRepository userRepository, IRoleFeatureMappingRepository roleFeatureMappingRepository, IFeatureRepository featureRepository)
        {
            _userRepository = userRepository;
            _roleFeatureMappingRepository = roleFeatureMappingRepository;
            _featureRepository = featureRepository;
        }

        public AuthedRoleFeatureQueryResult Handle(AuthedRoleFeatureQuery query)
        {
            List<User> users = new List<User>();
            try
            {
                var user = _userRepository.GetByAltId(query.UserAltId);
                var featureId = user.RolesId == 1 ? _featureRepository.GetAll().Where(w => w.ModuleId == FIL.Contracts.Enums.Modules.KzSuite).Select(s => s.Id)
                    : _roleFeatureMappingRepository.GetByRoleId(user.RolesId).Select(s => s.FeatureId).Distinct();
                IEnumerable<FIL.Contracts.DataModels.Feature> featureList = _featureRepository.GetByIds(featureId);
                return new AuthedRoleFeatureQueryResult
                {
                    Feature = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.Feature>>(featureList)
                };
            }
            catch (System.Exception ex)
            {
                return new AuthedRoleFeatureQueryResult { };
            }
        }
    }
}