using FIL.Api.Repositories.Tiqets;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.DataModels.Tiqets;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Tiqets
{
    public class SaveOrderCommandHandler : BaseCommandHandler<SaveOrderCommand>
    {
        private readonly ILogger _logger;
        private readonly ITiqetsTransactionRepository _tiqetsTransactionRepository;

        public SaveOrderCommandHandler(ILogger logger, ITiqetsTransactionRepository tiqetsTransactionRepository,
        IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _tiqetsTransactionRepository = tiqetsTransactionRepository;
        }

        protected override async Task Handle(SaveOrderCommand command)
        {
            try
            {
                var tiqetsTransactions = _tiqetsTransactionRepository.Save(new TiqetsTransaction
                {
                    TransactionId = command.TransactionId,
                    OrderReferenceId = command.OrderRefernceId,
                    PaymentConfirmationToken = command.PaymentToken,
                    IsOrderConfirmed = false,
                    IsEnabled = true,
                    ModifiedBy = command.ModifiedBy
                });
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to Disable Event HOHO Data", e));
            }
        }
    }
}