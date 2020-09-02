using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Users;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Users
{
    public class RegisterUserCommandHandler : BaseCommandHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IUsersWebsiteInviteRepository _userWebsiteInviteRepository;
        private readonly IFeelUserAdditionalDetailRepository _feelUserAdditionalDetailRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly ISaveIPProvider _saveIPProvider;
        private readonly IReferralRepository _referralRepository;

        public RegisterUserCommandHandler(IUsersWebsiteInviteRepository userWebsiteInviteRepository,
            IUserRepository userRepository,
            IFeelUserAdditionalDetailRepository feelUserAdditionalDetailRepository,
            IIPDetailRepository iPDetailRepository,
            ICountryRepository countryRepository,
            ISaveIPProvider saveIPProvider,
            IReferralRepository referralRepository,
            IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
            _saveIPProvider = saveIPProvider;
            _mediator = mediator;
            _userWebsiteInviteRepository = userWebsiteInviteRepository;
            _feelUserAdditionalDetailRepository = feelUserAdditionalDetailRepository;
            _countryRepository = countryRepository;
            _iPDetailRepository = iPDetailRepository;
            _referralRepository = referralRepository;
        }

        protected override async Task Handle(RegisterUserCommand command)
        {
            IPDetail ipDetail = new FIL.Contracts.DataModels.IPDetail();

            var country = _countryRepository.GetByAltId(new Guid(command.PhoneCode.Split("~")[1]));
            var user = new FIL.Contracts.DataModels.User
            {
                AltId = Guid.NewGuid(),
                Email = command.Email,
                Password = command.PasswordHash,
                RolesId = command.ChannelId == Channels.Feel ? 11 : 2, // TODO: XXX: Need a default
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
                IsRASVMailOPT = command.IsMailOpt,
                CountryId = country.Id
            };

            user.IPDetailId = user.IPDetailId == 0 ? null : user.IPDetailId;
            var userDetails = _userRepository.Save(user);

            if (command.ChannelId == Channels.Feel)
            {
                var feelUserAdditional = new FeelUserAdditionalDetail
                {
                    UserId = Convert.ToInt32(userDetails.Id),
                    OptedForMailer = Convert.ToBoolean(command.IsMailOpt),
                    SocialLoginId = null,
                    SignUpMethodId = Convert.ToInt32(command.SignUpMethodId),
                    IsEnabled = true
                };
                _feelUserAdditionalDetailRepository.Save(feelUserAdditional);
            }
            try
            {
                if (command.ReferralId != null)
                {
                    var referral = _referralRepository.GetByAltId(Guid.Parse(command.ReferralId));
                    if (referral != null)
                    {
                        userDetails.ReferralId = referral.Id;
                    }
                }
                ipDetail = _saveIPProvider.SaveIp(command.Ip);
                if (ipDetail != null)
                {
                    userDetails.IPDetailId = ipDetail.Id;
                    _userRepository.Save(userDetails);
                }
            }
            catch
            {
                ipDetail = null;
            }
            if (command.SiteId == Site.RASVSite)
            {
                await _mediator.Publish(new Events.Event.HubSpot.VisitorInfoEvent(user));
            }
        }
    }
}