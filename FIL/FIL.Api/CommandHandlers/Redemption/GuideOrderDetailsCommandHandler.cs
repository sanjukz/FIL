using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Commands.Redemption;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Redemption
{
    public class GuideOrderDetailsCommandHandler : BaseCommandHandler<GuideOrderDetailsCommand>
    {
        private readonly IOrderDetailsRepository _OrderDetailsRepository;
        private readonly IUserRepository _UserRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IMediator _mediator;

        public GuideOrderDetailsCommandHandler(IOrderDetailsRepository OrderDetailsRepository,
        IUserRepository UserRepository,
        FIL.Logging.ILogger logger,
            IMediator mediator)
           : base(mediator)
        {
            _OrderDetailsRepository = OrderDetailsRepository;
            _UserRepository = UserRepository;
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task Handle(GuideOrderDetailsCommand command)
        {
            try
            {
                var orderDetails = _OrderDetailsRepository.GetTransaction(command.TransactionId);
                if (orderDetails != null)
                {
                    if (command.OrderStatusId == FIL.Contracts.Enums.ApproveStatus.Approved)
                    {
                        orderDetails.ApprovedBy = command.ModifiedBy;
                        orderDetails.ApprovedUtc = DateTime.UtcNow;
                        orderDetails.UpdatedUtc = DateTime.UtcNow;
                        orderDetails.OrderStatusId = (int)FIL.Contracts.Enums.ApproveStatus.Approved;
                        orderDetails.IsEnabled = true;
                        _OrderDetailsRepository.Save(orderDetails);
                    }
                    else if (command.OrderStatusId == FIL.Contracts.Enums.ApproveStatus.Success)
                    {
                        orderDetails.OrderCompletedDate = DateTime.UtcNow;
                        orderDetails.UpdatedUtc = DateTime.UtcNow;
                        orderDetails.OrderStatusId = (int)FIL.Contracts.Enums.ApproveStatus.Success;
                        orderDetails.IsEnabled = true;
                        _OrderDetailsRepository.Save(orderDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
        }
    }
}