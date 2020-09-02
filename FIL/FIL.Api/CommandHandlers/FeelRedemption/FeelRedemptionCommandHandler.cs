using FIL.Api.Repositories;
using FIL.Contracts.Commands.FeelRedemption;
using FIL.Contracts.DataModels;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.FeelNewsLetterSignUps
{
    public class FeelRedemptionCommandHandler : BaseCommandHandler<FeelRedemptionCommand>
    {
        private readonly ITransactionDetailRepository _transactionDetailRepository;

        public FeelRedemptionCommandHandler(ITransactionDetailRepository transactionDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _transactionDetailRepository = transactionDetailRepository;
        }

        protected override Task Handle(FeelRedemptionCommand command)
        {
            var transactionDetailLists = _transactionDetailRepository.GetAllByIds(command.TransactionDetailIds);
            var transactionDetailModel = AutoMapper.Mapper.Map<List<TransactionDetail>>(transactionDetailLists);
            foreach (var currentTransactionDetailId in transactionDetailModel)
            {
                currentTransactionDetailId.IsRedeemed = true;
                _transactionDetailRepository.Save(currentTransactionDetailId);
            }
            return Task.FromResult(0);
        }
    }
}