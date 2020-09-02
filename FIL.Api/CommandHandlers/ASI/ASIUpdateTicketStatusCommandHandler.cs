using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.ASI;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.ASI
{
    public class ASIUpdateTicketStatusCommandHandler : BaseCommandHandlerWithResult<ASIUpdateTicketStatusCommand, ASIUpdateTicketStatusCommandResult>
    {
        private readonly IASIPaymentResponseDetailTicketMappingRepository _aSIPaymentResponseDetailTicketMappingRepository;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public ASIUpdateTicketStatusCommandHandler(IASIPaymentResponseDetailTicketMappingRepository aSIPaymentResponseDetailTicketMappingRepository,
            ILogger logger,
            ISettings settings,
            IMediator mediator) : base(mediator)
        {
            _aSIPaymentResponseDetailTicketMappingRepository = aSIPaymentResponseDetailTicketMappingRepository;
            _mediator = mediator;
            _logger = logger;
            _settings = settings;
        }

        protected override async Task<ICommandResult> Handle(ASIUpdateTicketStatusCommand command)
        {
            try
            {
                var allTicketDetails = _aSIPaymentResponseDetailTicketMappingRepository.GetByIds(command.ASIPaymentResponseDetailTicketMappings.Select(s => s.Id)).ToList();
                foreach (FIL.Contracts.DataModels.ASI.ASIPaymentResponseDetailTicketMapping asiPaymentResponseDetailTicketMappings in allTicketDetails)
                {
                    asiPaymentResponseDetailTicketMappings.IsEnabled = true;
                    asiPaymentResponseDetailTicketMappings.UpdatedUtc = DateTime.UtcNow;
                    _aSIPaymentResponseDetailTicketMappingRepository.Save(asiPaymentResponseDetailTicketMappings);
                }
                return new ASIUpdateTicketStatusCommandResult
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return new ASIUpdateTicketStatusCommandResult { };
            }
        }
    }
}