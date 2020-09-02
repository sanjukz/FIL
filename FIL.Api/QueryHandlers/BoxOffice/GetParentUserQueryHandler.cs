using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetParentUserQueryHandler : IQueryHandler<GetParentUserQuery, GetParentUserQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;

        public GetParentUserQueryHandler(IUserRepository userRepository, IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository)
        {
            _userRepository = userRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
        }

        public GetParentUserQueryResult Handle(GetParentUserQuery query)
        {
            var users = _userRepository.GetAllByChannel(Contracts.Enums.Channels.ZSuite);
            var boxOfficeUser = _boxofficeUserAdditionalDetailRepository.GetByUserIdsAndUserType(users.Select(s => s.Id), query.RoleId);
            var boUsers = _userRepository.GetByUserIds(boxOfficeUser.Select(s => s.UserId));
            return new GetParentUserQueryResult
            {
                ParentUsers = AutoMapper.Mapper.Map<List<User>>(boUsers)
            };
        }
    }
}