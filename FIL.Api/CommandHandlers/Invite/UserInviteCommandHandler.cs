using FIL.Api.Repositories;
using FIL.Contracts.Commands.Invite;
using FIL.Contracts.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Invite
{
    public class UserInviteCommandHandler : BaseCommandHandler<UserInviteCommand>
    {
        private readonly IUsersWebsiteInviteRepository _userWebsiteInviteRepository;
        private readonly IMediator _mediator;

        public UserInviteCommandHandler(IUsersWebsiteInviteRepository userWebsiteInviteRepository, IMediator mediator) : base(mediator)
        {
            _userWebsiteInviteRepository = userWebsiteInviteRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(UserInviteCommand command)
        {
            var userinvite = _userWebsiteInviteRepository.GetByEmail(command.UserEmail);
            if (userinvite == null)
            {
                var userwebsiteInvite = new UsersWebsiteInvite
                {
                    UserEmail = command.UserEmail,
                    UserInviteCode = command.UserInviteCode,
                    IsUsed = false,
                    WebsiteID = 8,
                    UsedOn = null,
                    CreatedBy = command.ModifiedBy,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = command.ModifiedBy,
                    ModifiedOn = DateTime.UtcNow
                };
                _userWebsiteInviteRepository.Save(userwebsiteInvite);
            }
            else
            {
                userinvite.UserInviteCode = RandomString(6);
                _userWebsiteInviteRepository.Save(userinvite);
            }
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}