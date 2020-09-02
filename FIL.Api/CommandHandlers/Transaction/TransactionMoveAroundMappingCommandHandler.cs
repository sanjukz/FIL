using FIL.Api.CommandHandlers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.Transaction
{
    public class TransactionMoveAroundMappingCommandHandler : BaseCommandHandler<TransactionMoveAroundMappingCommand>
    {
        private readonly ITransactionMoveAroundMappingRepository _transactionMoveAroundMappingRepository;

        public TransactionMoveAroundMappingCommandHandler(ITransactionMoveAroundMappingRepository transactionMoveAroundMappingRepository, IMediator mediator) : base(mediator)
        {
            _transactionMoveAroundMappingRepository = transactionMoveAroundMappingRepository;
        }

        protected override async Task Handle(TransactionMoveAroundMappingCommand command)
        {
            try
            {
                if (command.EventVenueMappingTimeId != -1)
                {
                    TransactionMoveAroundMapping data = new TransactionMoveAroundMapping();
                    data.AltId = Guid.NewGuid();
                    data.TransactionId = command.TransactionId;
                    data.EventVenueMappingTimeId = command.EventVenueMappingTimeId;
                    data.Address1 = string.IsNullOrWhiteSpace(command.PurchaserAddress.Address1) ? null : command.PurchaserAddress.Address1;
                    data.Address2 = string.IsNullOrWhiteSpace(command.PurchaserAddress.Address2) ? null : command.PurchaserAddress.Address2;
                    data.Town = string.IsNullOrWhiteSpace(command.PurchaserAddress.Town) ? null : command.PurchaserAddress.Town;
                    data.Region = string.IsNullOrWhiteSpace(command.PurchaserAddress.Region) ? null : command.PurchaserAddress.Region;
                    data.PostalCode = command.PurchaserAddress.PostalCode;
                    data.CreatedUtc = command.CreatedUtc;
                    data.CreatedBy = command.CreatedBy;
                    var transactionEventVenue = _transactionMoveAroundMappingRepository.Save(data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}