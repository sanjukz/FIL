using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Queries.User;
using FIL.Contracts.QueryResults;

namespace FIL.Api.QueryHandlers.Users
{
    public class LoginWithOTPQueryHandler : IQueryHandler<LoginWithOTPQuery, LoginWithOTPQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public LoginWithOTPQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public LoginWithOTPQueryResult Handle(LoginWithOTPQuery query)
        {
            var user = _userRepository.GetByPhoneAndChannel(query.PhoneCode, query.PhoneNumber, query.ChannelId);
            if (user == null && string.IsNullOrEmpty(query.Email))
            {
                //If new user need addtional feilds for signup
                return new LoginWithOTPQueryResult
                {
                    Success = false,
                    IsAdditionalFieldsReqd = true
                };
            }

            user.PhoneConfirmed = true;
            user.LoginCount++;
            user.AccessFailedCount = 0;
            user.LockOutEnabled = false;
            user.ModifiedBy = user.AltId;
            user.IsActivated = true;
            _userRepository.Save(user);

            return new LoginWithOTPQueryResult
            {
                Success = true,
                User = Mapper.Map<Contracts.Models.User>(user)
            };
        }
    }
}