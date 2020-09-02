using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class TicketPickupCommandHandler : BaseCommandHandlerWithResult<TicketPickupCommand, TicketPickupCommandResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly IUserRepository _userRepository;

        public TicketPickupCommandHandler(ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository, IUserRepository userRepository, IMediator mediator) : base(mediator)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _userRepository = userRepository;
        }

        protected override Task<ICommandResult> Handle(TicketPickupCommand command)
        {
            TicketPickupCommandResult ticketPickupCommandResult = new TicketPickupCommandResult();
            try
            {
                TransactionDeliveryDetail transactionDeliveryDetail = new TransactionDeliveryDetail();
                var transaction = _transactionRepository.GetSuccessfulTransactionDetails(command.TransactionId);
                if (command.TransactionDeliveryDetailId != 0)
                {
                    transactionDeliveryDetail = _transactionDeliveryDetailRepository.Get(command.TransactionDeliveryDetailId);
                    transactionDeliveryDetail.DeliveryStatus = true;
                    transactionDeliveryDetail.DeliverdTo = !string.IsNullOrWhiteSpace(transaction.FirstName) ? _userRepository.GetByAltId(transaction.CreatedBy).FirstName : "";
                    transactionDeliveryDetail.ModifiedBy = command.ModifiedBy;
                    transactionDeliveryDetail.DeliveryDate = DateTime.UtcNow;
                    _transactionDeliveryDetailRepository.Save(transactionDeliveryDetail);
                }
                else
                {
                    var transactionDetails = _transactionDetailRepository.GetByTransactionId(transaction.Id);
                    var transactionDeliveryDetails = _transactionDeliveryDetailRepository.GetByTransactionDetailIds(transactionDetails.Select(td => td.Id));
                    foreach (var item in transactionDeliveryDetails)
                    {
                        var currentTransactionDeliveryDetail = transactionDeliveryDetails.Where(s => s.Id == item.Id).FirstOrDefault();
                        currentTransactionDeliveryDetail.Id = item.Id;
                        currentTransactionDeliveryDetail.DeliveryStatus = true;
                        currentTransactionDeliveryDetail.DeliverdTo = !string.IsNullOrWhiteSpace(transaction.FirstName) ? _userRepository.GetByAltId(transaction.CreatedBy).FirstName : "";
                        currentTransactionDeliveryDetail.ModifiedBy = command.ModifiedBy;
                        currentTransactionDeliveryDetail.DeliveryDate = DateTime.UtcNow;
                        _transactionDeliveryDetailRepository.Save(currentTransactionDeliveryDetail);
                    }
                }
                ticketPickupCommandResult.Id = transaction.Id;
                ticketPickupCommandResult.Success = true;
            }
            catch (Exception ex)
            {
                ticketPickupCommandResult.Id = -1;
                ticketPickupCommandResult.Success = false;
            }
            return Task.FromResult<ICommandResult>(ticketPickupCommandResult);
        }
    }
}