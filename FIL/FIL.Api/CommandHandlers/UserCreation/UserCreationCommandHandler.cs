using FIL.Api.Repositories;
using FIL.Contracts.Commands.UserCreation;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.UserCreation
{
    public class UserCreationCommandHandler : BaseCommandHandler<UserCreationCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly IMediator _mediator;

        private readonly IUsersWebsiteInviteRepository _userWebsiteInviteRepository;

        public UserCreationCommandHandler(IUsersWebsiteInviteRepository userWebsiteInviteRepository,
            IUserRepository userRepository,
            IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository,
            IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _mediator = mediator;
            _userWebsiteInviteRepository = userWebsiteInviteRepository;
        }

        protected override async Task Handle(UserCreationCommand command)
        {
            var UserAltId = Guid.NewGuid();
            var userData = new FIL.Contracts.DataModels.User
            {
                AltId = UserAltId,
                Email = command.Email,
                Password = command.PasswordHash,
                RolesId = 6, // TODO: XXX: Need a default
                CreatedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                UserName = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneCode = command.PhoneCode,
                PhoneNumber = command.PhoneNumber,
                ChannelId = Channels.Website,
                SignUpMethodId = SignUpMethods.Regular,
                IsEnabled = true
            };
            _userRepository.Save(userData);

            var user = _userRepository.GetByAltId(UserAltId);

            if (user != null && !command.IsVendor)
            {
                var BoxofficeUser = new FIL.Contracts.DataModels.BoxofficeUserAdditionalDetail
                {
                    UserId = user.Id,
                    ParentId = 171,
                    UserType = 1,
                    IsETicket = 0,
                    IsChildTicket = false,
                    IsSrCitizenTicket = false,
                    TicketLimit = 10,
                    ChildTicketLimit = 0,
                    ChildForPerson = 0,
                    SrCitizenLimit = 0,
                    SrCitizenPerson = 0,
                    CityId = 1,
                    Address = "Melbourne",
                    ContactNumber = "",
                    CreatedBy = command.ModifiedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsEnabled = true
                };
                _boxofficeUserAdditionalDetailRepository.Save(BoxofficeUser);
            }
        }
    }
}