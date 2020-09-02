using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.FeelRedemption;
using FIL.Contracts.QueryResults.FeelRedemption;
using Microsoft.AspNetCore.Identity;

namespace FIL.Api.QueryHandlers.Users
{
    public class UserLoginUserQueryHandler : IQueryHandler<UserLoginUserQuery, UserLoginUserQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<string> _passwordHasher;

        public UserLoginUserQueryHandler(IUserRepository userRepository, IPasswordHasher<string> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public UserLoginUserQueryResult Handle(UserLoginUserQuery query)
        {
            var response = new UserLoginUserQueryResult();
            var user = _userRepository.GetByEmailAndChannel(query.Email, query.ChannelId, query.SignUpMethodId);
            if (user != null)
            {
                if (user.RolesId == 11)   // 11 is the roleID for Seller
                {
                    if (_passwordHasher.VerifyHashedPassword(user.Email, user.Password, query.Password) ==
                        PasswordVerificationResult.Success)
                    {
                        response.IsSuccess = true;
                        response.User = AutoMapper.Mapper.Map<User>(user);
                        response.IsPasswordValid = true;
                    }
                    else
                    {
                        response.IsPasswordValid = false;
                        response.IsSuccess = true;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;
        }
    }
}