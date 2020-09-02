using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice.GetUserRolesQuery;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, GetUserRolesQueryResult>
    {
        private readonly IRoleRepository _roleRepository;

        public GetUserRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public GetUserRolesQueryResult Handle(GetUserRolesQuery query)
        {
            var roles = _roleRepository.GetZsuiteModules();
            return new GetUserRolesQueryResult
            {
                Roles = AutoMapper.Mapper.Map<List<Role>>(roles)
            };
        }
    }
}