using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.User;
using FIL.Contracts.QueryResults;
using Microsoft.AspNetCore.Identity;
using System;

namespace FIL.Api.QueryHandlers.Users
{
    public class LoginWithGoogleUserQueryHandler : IQueryHandler<LoginWithGoogleUserQuery, LoginWithGoogleUserQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFeelUserAdditionalDetailRepository _feelUserAdditionalDetailRepository;
        private readonly IPasswordHasher<string> _passwordHasher;

        public LoginWithGoogleUserQueryHandler(IUserRepository userRepository, IFeelUserAdditionalDetailRepository feelUserAdditionalDetailRepository,
            IPasswordHasher<string> passwordHasher)
        {
            _userRepository = userRepository;
            _feelUserAdditionalDetailRepository = feelUserAdditionalDetailRepository;
            _passwordHasher = passwordHasher;
        }

        public LoginWithGoogleUserQueryResult Handle(LoginWithGoogleUserQuery query)
        {
            var response = new LoginWithGoogleUserQueryResult();
            var user = new User();
            if (query.ChannelId == Contracts.Enums.Channels.Feel)
            {
                user = _userRepository.GetByEmailAndChannel(query.Email, query.ChannelId, null);
            }
            else
            {
                user = query.ChannelId == null ? _userRepository.GetByEmail(query.Email) : _userRepository.GetByEmailAndChannel(query.Email, query.ChannelId, query.SignUpMethodId);
            }

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