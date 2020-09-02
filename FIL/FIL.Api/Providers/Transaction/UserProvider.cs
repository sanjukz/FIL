using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Logging;
using Microsoft.AspNetCore.Identity;
using System;

namespace FIL.Api.Providers.Transaction
{
    public interface IUserProvider
    {
        User GetCheckoutUser(CheckoutCommand checkoutCommand, FIL.Contracts.Models.Country country);
    }

    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly FIL.Logging.ILogger _logger;

        public UserProvider(ILogger logger, ISettings settings,
            IPasswordHasher<string> passwordHasher,
                 IUserRepository userRepository
            )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public User GetCheckoutUser(CheckoutCommand checkoutCommand, FIL.Contracts.Models.Country country)
        {
            try
            {
                if (checkoutCommand.IsLoginCheckout)
                {
                    User currentUser = _userRepository.GetByAltId(checkoutCommand.UserAltId);
                    return currentUser;
                }
                else
                {
                    User user = checkoutCommand.GuestUser.SignUpMethodId == null ? _userRepository.GetByEmailIdAndChannelId(checkoutCommand.GuestUser.Email, checkoutCommand.ChannelId)
                        : _userRepository.GetByEmailAndChannel(checkoutCommand.GuestUser.Email, checkoutCommand.ChannelId, checkoutCommand.GuestUser.SignUpMethodId);
                    if (user == null)
                    {
                        Guid userAltId = Guid.NewGuid();
                        user = new User
                        {
                            AltId = userAltId,
                            Email = checkoutCommand.GuestUser.Email,
                            Password = _passwordHasher.HashPassword(checkoutCommand.GuestUser.Email, checkoutCommand.GuestUser.Password),
                            RolesId = 2, // TODO: XXX: Need a default
                            CreatedBy = userAltId,
                            CreatedUtc = DateTime.UtcNow,
                            UserName = checkoutCommand.GuestUser.Email,
                            FirstName = checkoutCommand.GuestUser.FirstName,
                            LastName = checkoutCommand.GuestUser.LastName,
                            PhoneCode = country.Phonecode.ToString(),
                            PhoneNumber = checkoutCommand.GuestUser.PhoneNumber,
                            ChannelId = ((checkoutCommand.ChannelId == Channels.Website) ? Channels.Website : Channels.Feel),
                            SignUpMethodId = SignUpMethods.Regular,
                            IsEnabled = true,
                            IsRASVMailOPT = checkoutCommand.GuestUser.IsMailOpt
                        };
                        user = _userRepository.Save(user);
                    }
                    else
                    {
                        user.FirstName = checkoutCommand.GuestUser.FirstName;
                        user.LastName = checkoutCommand.GuestUser.LastName;
                        user.PhoneCode = country.Phonecode.ToString();
                        user.PhoneNumber = checkoutCommand.GuestUser.PhoneNumber;
                        user = _userRepository.Save(user);
                    }
                    return user;
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new User
                {
                };
            }
        }
    }
}