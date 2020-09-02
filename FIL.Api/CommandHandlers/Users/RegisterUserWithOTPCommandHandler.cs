using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Users;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Users
{
    public class RegisterUserWithOTPCommandHandler : BaseCommandHandlerWithResult<RegisterUserWithOTPCommand, RegisterUserWithOTPCommandCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IReferralRepository _referralRepository;
        private readonly ISaveIPProvider _saveIPProvider;
        private readonly ICountryRepository _countryRepository;

        public RegisterUserWithOTPCommandHandler(
            IUserRepository userRepository,
             IReferralRepository referralRepository,
            ISaveIPProvider saveIPProvider,
            ICountryRepository countryRepository,
            IMediator mediator) : base(mediator)
        {
            _userRepository = userRepository;
            _saveIPProvider = saveIPProvider;
            _countryRepository = countryRepository;
            _referralRepository = referralRepository;
        }

        protected override async Task<ICommandResult> Handle(RegisterUserWithOTPCommand command)
        {
            RegisterUserWithOTPCommandCommandResult response = new RegisterUserWithOTPCommandCommandResult();
            IPDetail ipDetail = new FIL.Contracts.DataModels.IPDetail();

            var user = new User();
            if (command.ChannelId == Channels.Feel)
            {
                user = _userRepository.GetByEmailAndChannel(command.Email, command.ChannelId, null);
            }
            if (user == null)
            {
                var country = _countryRepository.GetByAltId(new Guid(command.PhoneCode.Split("~")[1]));
                var userModel = new User
                {
                    AltId = Guid.NewGuid(),
                    Email = command.Email,
                    Password = command.PasswordHash,
                    RolesId = command.ChannelId == Channels.Feel ? 11 : 2,
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    UserName = command.UserName,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    PhoneCode = command.PhoneCode.Split("~")[0],
                    PhoneNumber = command.PhoneNumber,
                    ChannelId = command.ChannelId,
                    SignUpMethodId = command.SignUpMethodId,
                    IsEnabled = true,
                    CountryId = country.Id
                };
                userModel.IPDetailId = userModel.IPDetailId == 0 ? null : userModel.IPDetailId;
                userModel = _userRepository.Save(userModel);
                response.Success = true;
                response.User = userModel;
                try
                {
                    if (command.ReferralId != null)
                    {
                        var referral = _referralRepository.GetByAltId(Guid.Parse(command.ReferralId));
                        if (referral != null)
                        {
                            userModel.ReferralId = referral.Id;
                        }
                    }
                    ipDetail = _saveIPProvider.SaveIp(command.Ip);
                    if (ipDetail != null)
                    {
                        userModel.IPDetailId = ipDetail.Id;
                        _userRepository.Save(userModel);
                    }
                }
                catch
                {
                    ipDetail = null;
                }
            }
            else if (string.IsNullOrEmpty(user.PhoneCode) && string.IsNullOrEmpty(user.PhoneNumber))
            {
                user.PhoneConfirmed = true;
                user.PhoneCode = command.PhoneCode;
                user.PhoneNumber = command.PhoneNumber;
                user = _userRepository.Save(user);
                response.Success = true;
                response.User = user;
            }
            else
            {
                response.EmailAlreadyRegistered = true;
            }
            return response;
        }
    }
}