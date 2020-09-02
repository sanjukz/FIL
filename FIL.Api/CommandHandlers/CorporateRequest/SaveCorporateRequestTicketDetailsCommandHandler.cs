using FIL.Api.Repositories;
using FIL.Contracts.Commands.CorporateRequest;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CorporateRequest
{
    public class SaveCorporateRequestTicketDetailsCommandHandler : BaseCommandHandlerWithResult<SaveCorporateRequestTicketDetailsCommand, SaveCorporateRequestTicketDetailsCommandResult>
    {
        private readonly ICorporateRequestDetailRepository _corporateRequestDetailRepository;

        public SaveCorporateRequestTicketDetailsCommandHandler(ICorporateRequestDetailRepository CorporateRequestDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _corporateRequestDetailRepository = CorporateRequestDetailRepository;
        }

        protected override Task<ICommandResult> Handle(SaveCorporateRequestTicketDetailsCommand command)
        {
            SaveCorporateRequestTicketDetailsCommandResult SaveCorporateRequestTicketDetails = new SaveCorporateRequestTicketDetailsCommandResult();
            foreach (var item in command.CorporateRequestTicketData)
            {
                var corporateRequestDetails = new CorporateRequestDetail
                {
                    CorporateRequestId = command.CorporateRequestId,
                    RequestedTickets = item.Quantity,
                    ApprovedTickets = 0,
                    Price = item.TicketPrice,
                    ApprovedStatus = false,
                    IsEnabled = true,
                    CreatedUtc = DateTime.UtcNow,
                    CreatedBy = command.ModifiedBy,
                    EventTicketAttributeId = item.EventTicketAttributeId,
                    TicketTypeId = item.TicketTypeId
                };
                _corporateRequestDetailRepository.Save(corporateRequestDetails);
            }
            SaveCorporateRequestTicketDetails.Success = true;
            return Task.FromResult<ICommandResult>(SaveCorporateRequestTicketDetails);
        }
    }
}