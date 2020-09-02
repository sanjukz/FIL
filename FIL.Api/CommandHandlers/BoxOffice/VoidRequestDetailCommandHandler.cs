using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class VoidRequestDetailCommandHandler : BaseCommandHandler<VoidRequestDetailCommand>
    {
        private readonly IVoidRequestDetailRepository _voidRequestDetailRepository;

        public VoidRequestDetailCommandHandler(IVoidRequestDetailRepository voidRequestDetailRepository, IMediator mediator) : base(mediator)
        {
            _voidRequestDetailRepository = voidRequestDetailRepository;
        }

        protected override Task Handle(VoidRequestDetailCommand command)
        {
            _voidRequestDetailRepository.Save(new VoidRequestDetail
            {
                TransactionId = command.ConfirmationNumber,
                Reason = command.Reason,
                IsVoid = false,
                RequestDateTimeUtc = DateTime.UtcNow,
                IsEnabled = true,
                ModifiedBy = command.ModifiedBy,
            });
            return Task.FromResult(0);
        }
    }
}