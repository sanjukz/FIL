using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class UpdateNotificationCommandHandler : BaseCommandHandlerWithResult<UpdateNotificationCommand, UpdateNotificationCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFeelUserAdditionalDetailRepository _feelUserAdditionalDetailRepository;

        public UpdateNotificationCommandHandler(IUserRepository userRepository,
            IFeelUserAdditionalDetailRepository feelUserAdditionalDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
            _feelUserAdditionalDetailRepository = feelUserAdditionalDetailRepository;
        }

        protected override async Task<ICommandResult> Handle(UpdateNotificationCommand command)
        {
            UpdateNotificationCommandResult commandResult = new UpdateNotificationCommandResult();
            var userModel = _userRepository.GetByAltId(command.UserAltId);
            var userAdditionalModel = _feelUserAdditionalDetailRepository.GetByUserId(Convert.ToInt32(userModel.Id));

            if (Convert.ToBoolean(command.ShouldUpdate))
            {
                if (Convert.ToBoolean(command.IsMailOpt))
                {
                    if (userAdditionalModel == null)
                    {
                        userAdditionalModel = _feelUserAdditionalDetailRepository.Save(new FeelUserAdditionalDetail
                        {
                            UserId = Convert.ToInt32(userModel.Id),
                            OptedForMailer = true,
                            IsEnabled = true,
                            ModifiedBy = command.UserAltId
                        });
                    }
                    else
                    {
                        if (!userAdditionalModel.OptedForMailer)
                        {
                            userAdditionalModel.OptedForMailer = true;
                            userAdditionalModel = _feelUserAdditionalDetailRepository.Save(userAdditionalModel);
                        }
                    }
                }
                else
                {
                    if (userAdditionalModel != null && userAdditionalModel.OptedForMailer)
                    {
                        userAdditionalModel.OptedForMailer = false;
                        userAdditionalModel = _feelUserAdditionalDetailRepository.Save(userAdditionalModel);
                    }
                }
            }
            if (userAdditionalModel == null)
            {
                commandResult.IsMailOpt = false;
            }
            else if (userAdditionalModel != null && userAdditionalModel.OptedForMailer)
            {
                commandResult.IsMailOpt = true;
            }
            else
            {
                commandResult.IsMailOpt = false;
            }

            return commandResult;
        }
    }
}