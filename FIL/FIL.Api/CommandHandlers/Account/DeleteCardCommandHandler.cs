using FIL.Api.Repositories;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.DataModels;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Account
{
    public class DeleteCardCommandHandler : BaseCommandHandler<DeleteCardCommand>
    {
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;

        public DeleteCardCommandHandler(IUserCardDetailRepository userCardDetailRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            IMediator mediator)
            : base(mediator)
        {
            _userCardDetailRepository = userCardDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
        }

        protected override async Task Handle(DeleteCardCommand command)
        {
            UserCardDetail cardDetail = _userCardDetailRepository.GetByAltId(command.AltId);

            if (cardDetail != null)
            {
                var transactionPaymentDetails = _transactionPaymentDetailRepository.GetByUserCardDetailId(cardDetail.Id).ToList();

                if (transactionPaymentDetails != null)
                {
                    foreach (var transactionPaymentDetail in transactionPaymentDetails)
                    {
                        _transactionPaymentDetailRepository.Delete(transactionPaymentDetail);
                    }
                }
                _userCardDetailRepository.Delete(cardDetail);
            }
        }
    }
}