using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetAllBOUserQueryHandler : IQueryHandler<GetAllBOUserQuery, GetParentUserQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public GetAllBOUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public GetParentUserQueryResult Handle(GetAllBOUserQuery query)
        {
            var users = _userRepository.GetAllByChannel(Contracts.Enums.Channels.ZSuite);
            return new GetParentUserQueryResult
            {
                ParentUsers = AutoMapper.Mapper.Map<List<User>>(users)
            };
        }
    }
}