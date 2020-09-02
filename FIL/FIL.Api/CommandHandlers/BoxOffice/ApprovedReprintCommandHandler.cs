using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class ApprovedReprintCommandHandler : BaseCommandHandler<ApprovedReprintCommand>
    {
        private readonly IReprintRequestDetailRepository _reprintRequestDetailRepository;

        public ApprovedReprintCommandHandler(IReprintRequestDetailRepository reprintRequestDetailRepository, IMediator mediator) : base(mediator)
        {
            _reprintRequestDetailRepository = reprintRequestDetailRepository;
        }

        protected override Task Handle(ApprovedReprintCommand command)
        {
            foreach (var reprintRequestDetailId in command.ReprintRequestDetailId.Distinct())
            {
                var reprintRequestDetail = _reprintRequestDetailRepository.Get(reprintRequestDetailId);
                reprintRequestDetail.IsApproved = true;
                reprintRequestDetail.ApprovedDateTime = DateTime.UtcNow;
                reprintRequestDetail.ModifiedBy = command.ModifiedBy;
                _reprintRequestDetailRepository.Save(reprintRequestDetail);
            }
            return Task.FromResult(0);
        }
    }
}