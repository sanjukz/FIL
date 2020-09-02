using FIL.Api.Repositories;
using FIL.Contracts.Queries.UserProfile;
using FIL.Contracts.QueryResults.UserProfile;

namespace FIL.Api.QueryHandlers.UserProfile
{
    public class UserProfileQueryHandler : IQueryHandler<UserProfileQuery, UserProfileQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public UserProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserProfileQueryResult Handle(UserProfileQuery query)
        {
            var user = _userRepository.GetByAltId(query.AltId);
            if (user == null)
            {
                return new UserProfileQueryResult();
            }
            else
            {
                var userModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.UserProfile>(user);
                return new UserProfileQueryResult
                {
                    Profile = userModel,
                };
            }
        }
    }
}