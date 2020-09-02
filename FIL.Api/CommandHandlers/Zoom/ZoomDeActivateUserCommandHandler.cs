using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.Zoom;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Zoom
{
    public class ZoomDeActivateUserCommandHandler : BaseCommandHandlerWithResult<ZoomDeActivateUserCommand, ZoomDeActivateUserCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IZoomUserRepository _zoomUserRepository;

        public ZoomDeActivateUserCommandHandler(
       ILogger logger, ISettings settings, IMediator mediator, IZoomUserRepository zoomUserRepository) : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _zoomUserRepository = zoomUserRepository;
        }

        protected override async Task<ICommandResult> Handle(ZoomDeActivateUserCommand command)
        {
            ZoomDeActivateUserCommandResult zoomDeActivateUserCommandResult = new ZoomDeActivateUserCommandResult();

            var zoomUserModel = _zoomUserRepository.GetByAltId(command.AltId);

            if (zoomUserModel != null && zoomUserModel.UniqueId == command.UniqueId)
            {
                zoomUserModel.UniqueId = String.Empty;
                zoomUserModel.IsActive = false;
                _zoomUserRepository.Save(zoomUserModel);
            }
            zoomDeActivateUserCommandResult.success = true;
            return zoomDeActivateUserCommandResult;
        }
    }
}