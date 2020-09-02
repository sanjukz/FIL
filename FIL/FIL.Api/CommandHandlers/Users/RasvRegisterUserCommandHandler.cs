using FIL.Api.Events.Event.HubSpot;
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
    public class RasvRegisterUserCommandHandler : BaseCommandHandler<RasvRegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IReferralRepository _referralRepository;
        private readonly IMediator _mediator;
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly ISaveIPProvider _saveIPProvider;

        public RasvRegisterUserCommandHandler(
            IUserRepository userRepository,
            IMediator mediator,
            IReferralRepository referralRepository,
            ISaveIPProvider saveIPProvider,
            IIPDetailRepository iPDetailRepository)
            : base(mediator)
        {
            _userRepository = userRepository;
            _referralRepository = referralRepository;
            _mediator = mediator;
            _saveIPProvider = saveIPProvider;
            _iPDetailRepository = iPDetailRepository;
        }

        protected override async Task Handle(RasvRegisterUserCommand command)
        {
            IPDetail ipDetail = new FIL.Contracts.DataModels.IPDetail();
            bool shouldInsert = true;
            //For Facebook login in FAP
            if (command.ChannelId == Channels.Feel && !string.IsNullOrEmpty(command.SocialLoginId))
            {
                var userModel = _userRepository.GetByEmailAndChannel(command.Email, command.ChannelId, command.SignUpMethodId);
                if (userModel != null)
                {
                    shouldInsert = false;
                    userModel.SocialLoginId = command.SocialLoginId;
                    _userRepository.Save(userModel);
                }
            }
            if (shouldInsert)
            {
                var user = new User
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
                    PhoneCode = command.PhoneCode,
                    PhoneNumber = command.PhoneNumber,
                    ChannelId = command.ChannelId,
                    IsActivated = true,
                    SignUpMethodId = command.SignUpMethodId,
                    SocialLoginId = command.SocialLoginId,
                    IsEnabled = true
                };
                user.IPDetailId = user.IPDetailId == 0 ? null : user.IPDetailId;
                if (command.ReferralId != null)
                {
                    var referral = _referralRepository.GetBySlug(command.ReferralId);
                    if (referral != null)
                    {
                        user.ReferralId = referral.Id;
                    }
                }
                var userDetails = _userRepository.Save(user);

                try
                {
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
                    await _mediator.Publish(new VisitorInfoEvent(user));
                }
            }
        }
    }
}