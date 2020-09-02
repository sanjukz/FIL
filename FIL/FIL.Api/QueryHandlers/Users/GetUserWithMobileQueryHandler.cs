using FIL.Api.Repositories;
using FIL.Contracts.Queries.User;
using FIL.Contracts.QueryResults.User;

namespace FIL.Api.QueryHandlers.Users
{
    public class GetUserWithMobileQueryHandler : IQueryHandler<GetUserWithMobileQuery, GetUserWithMobileQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserWithMobileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public GetUserWithMobileQueryResult Handle(GetUserWithMobileQuery query)
        {
            var user = _userRepository.GetByPhoneAndChannel(query.PhoneCode, query.PhoneNumber, query.ChannelId);
            if (user == null)
            {
                return new GetUserWithMobileQueryResult
                {
                    IsExist = false
                };
            }
            else
            {
                return new GetUserWithMobileQueryResult
                {
                    IsExist = true
                };
            }
        }
    }
}