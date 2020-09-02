using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Queries.User;
using FIL.Contracts.QueryResults;
using Microsoft.AspNetCore.Identity;
using System;

namespace FIL.Api.QueryHandlers.Users
{
    public class LoginWithFacebookRASVUserQueryHandler : IQueryHandler<LoginWithFacebookUserQuery, LoginWithFacebookUserQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<string> _passwordHasher;

        public LoginWithFacebookRASVUserQueryHandler(IUserRepository userRepository,
            IPasswordHasher<string> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public LoginWithFacebookUserQueryResult Handle(LoginWithFacebookUserQuery query)
        {
            var response = new LoginWithFacebookUserQueryResult();
            var user = query.ChannelId == null ? _userRepository.GetByEmail(query.Email)
                : (query.ChannelId == Contracts.Enums.Channels.Feel ? _userRepository.GetBySocialIdAndChannel(query.SocialLoginId, Contracts.Enums.Channels.Feel) : _userRepository.GetByEmailAndChannel(query.Email, query.ChannelId, query.SignUpMethodId));
            if (user != null)
            {
                bool lockedOut = user.LockOutEnabled;
                if (lockedOut)
                {
                    lockedOut = user.LockOutEndDateUtc != null && user.LockOutEndDateUtc > DateTime.UtcNow;
                }
                if (!lockedOut)
                {
                    response.Success = true;
                    user.LoginCount++;
                    user.AccessFailedCount = 0;
                    user.LockOutEnabled = false;
                }
                else
                {
                    user.AccessFailedCount++;
                    if (user.AccessFailedCount >= 5)
                    {
                        user.LockOutEnabled = true;
                        user.LockOutEndDateUtc = DateTime.UtcNow.Add(TimeSpan.FromMinutes(15));
                    }
                }
                user.ModifiedBy = user.AltId;
                _userRepository.Save(user);
                response.User = Mapper.Map<Contracts.Models.User>(user);
            }

            return response;
        }
    }
}