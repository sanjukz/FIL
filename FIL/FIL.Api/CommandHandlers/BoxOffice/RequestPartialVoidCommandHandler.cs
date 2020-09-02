using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class RequestPartialVoidCommandHandler : BaseCommandHandler<RequestPartialVoidCommand>
    {
        private readonly IPartialVoidRequestDetailRepository _partialVoidRequestDetailRepository;

        public RequestPartialVoidCommandHandler(IPartialVoidRequestDetailRepository partialVoidRequestDetailRepository, IMediator mediator) : base(mediator)
        {
            _partialVoidRequestDetailRepository = partialVoidRequestDetailRepository;
        }

        protected override Task Handle(RequestPartialVoidCommand command)
        {
            foreach (var barcodeId in command.Barcodes)
            {
                var partialvoidrequest = _partialVoidRequestDetailRepository.GetByBarcodeNumber(barcodeId);
                if (partialvoidrequest == null)
                {
                    _partialVoidRequestDetailRepository.Save(new PartialVoidRequestDetail
                    {
                        BarcodeNumber = barcodeId,
                        Reason = command.Reason,
                        RequestDateTimeUtc = DateTime.UtcNow,
                        IsPartialVoid = false,
                        IsEnabled = true,
                        ModifiedBy = command.ModifiedBy,
                    });
                }
            }
            return Task.FromResult(0);
        }
    }
}