using FIL.Api.Repositories;
using FIL.Contracts.Commands.Invite;
using FIL.Contracts.Models;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Invite
{
    public class UpdateInviteCommandHandler : BaseCommandHandler<UpdateInviteCommand>
    {
        private readonly IUsersWebsiteInviteRepository _userWebsiteInviteRepository;
        private readonly IMediator _mediator;

        public UpdateInviteCommandHandler(IUsersWebsiteInviteRepository userWebsiteInviteRepository, IMediator mediator) : base(mediator)
        {
            _userWebsiteInviteRepository = userWebsiteInviteRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(UpdateInviteCommand command)
        {
            var userinvite = _userWebsiteInviteRepository.GetById(command.Id);
            var userwebsiteInvite = new UsersWebsiteInvite
            {
                UserEmail = command.UserEmail,
                UserInviteCode = command.UserInviteCode,
                IsUsed = command.IsUsed,
                WebsiteID = 8,
                UsedOn = null,
                CreatedBy = command.ModifiedBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = command.ModifiedBy,
                ModifiedOn = DateTime.UtcNow,
                Id = command.Id
            };
            _userWebsiteInviteRepository.Save(userwebsiteInvite);
        }
    }
}