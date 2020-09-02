using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class ReprintRequestFormCommandHandler : BaseCommandHandler<ReprintRequestFormCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IReprintRequestRepository _reprintRequestRepository;
        private readonly IReprintRequestDetailRepository _reprintRequestDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;

        public ReprintRequestFormCommandHandler(IReprintRequestRepository reprintRequestRepository, IUserRepository userRepository, IReprintRequestDetailRepository reprintRequestDetailRepository, IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, IMediator mediator) : base(mediator)
        {
            _reprintRequestRepository = reprintRequestRepository;
            _userRepository = userRepository;
            _reprintRequestDetailRepository = reprintRequestDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
        }

        protected override async Task Handle(ReprintRequestFormCommand command)
        {
            var user = _userRepository.GetByAltId(command.ModifiedBy);
            var altId = Guid.NewGuid();
            var reprintRequestData = new ReprintRequest
            {
                UserId = user.Id,
                TransactionId = command.TransactionId,
                Remarks = command.Reason,
                IsApproved = false,
                RequestDateTime = DateTime.UtcNow,
                ModuleId = FIL.Contracts.Enums.Modules.Boxoffice,
                ModifiedBy = command.ModifiedBy,
                AltId = altId,
            };
            var reprintId = _reprintRequestRepository.Save(reprintRequestData);
            var matchSeatTicketDetailLookup = _matchSeatTicketDetailRepository.GetbyTransactionId(command.TransactionId)
                .ToDictionary(mst => mst.BarcodeNumber);
            foreach (var barcodeId in command.BarcodeId)
            {
                var matchSeatTicketDetails = matchSeatTicketDetailLookup[barcodeId];
                var reprintRequestDetails = new ReprintRequestDetail
                {
                    RePrintRequestId = reprintId.Id,
                    MatchSeatTicketDetaildId = matchSeatTicketDetails.Id,
                    BarcodeNumber = barcodeId,
                    IsRePrinted = false,
                    RePrintCount = 0,
                    IsApproved = false,
                    ModifiedBy = command.ModifiedBy,
                };
                _reprintRequestDetailRepository.Save(reprintRequestDetails);
            }
        }
    }
}