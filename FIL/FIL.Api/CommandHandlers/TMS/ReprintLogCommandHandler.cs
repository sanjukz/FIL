using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS
{
    public class ReprintLogCommandHandler : BaseCommandHandlerWithResult<ReprintLogCommand, ReprintLogCommandResult>
    {
        private readonly IReprintRequestRepository _reprintRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly Logging.ILogger _logger;

        public ReprintLogCommandHandler(Logging.ILogger logger,
            IReprintRequestRepository reprintRequestRepository,
             IUserRepository userRepository,
            IMediator mediator) : base(mediator)
        {
            _reprintRequestRepository = reprintRequestRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(ReprintLogCommand command)
        {
            ReprintLogCommandResult reprintLogCommandResult = new ReprintLogCommandResult();
            try
            {
                User user = _userRepository.GetByAltId(command.ModifiedBy);
                var reprintRequest = new ReprintRequest
                {
                    UserId = user.Id,
                    TransactionId = command.TransactionId,
                    RequestDateTime = DateTime.UtcNow,
                    Remarks = command.Remarks,
                    ModuleId = FIL.Contracts.Enums.Modules.KITMS,
                    ModifiedBy = command.ModifiedBy,
                };
                ReprintRequest reprintRequestResult = _reprintRequestRepository.Save(reprintRequest);
                reprintLogCommandResult.Id = reprintRequestResult.Id;
                reprintLogCommandResult.Success = true;
                return Task.FromResult<ICommandResult>(reprintLogCommandResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                reprintLogCommandResult.Id = -1;
                reprintLogCommandResult.Success = false;
                return Task.FromResult<ICommandResult>(reprintLogCommandResult);
            }
        }
    }
}