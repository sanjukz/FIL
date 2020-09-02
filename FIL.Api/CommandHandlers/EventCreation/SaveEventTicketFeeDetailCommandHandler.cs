using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class SaveEventTicketFeeDetailCommandHandler : BaseCommandHandlerWithResult<SaveTicketFeeDetailCommand, SaveEventTicketFeeDataResult>
    {
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;

        public SaveEventTicketFeeDetailCommandHandler(ITicketFeeDetailRepository ticketFeeDetailRepository, IMediator mediator)
            : base(mediator)
        {
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
        }

        protected override Task<ICommandResult> Handle(SaveTicketFeeDetailCommand command)
        {
            var eventFeeDetailData = new TicketFeeDetail();
            SaveEventTicketFeeDataResult saveEventTicketFeeDataResult = new SaveEventTicketFeeDataResult();
            var checkForEventTicketFeeDetail = _ticketFeeDetailRepository.GetByEventTicketAttributeIdAndFeedId(command.EventTicketAttributeId, command.FeedId);

            if (checkForEventTicketFeeDetail != null)
            {
                eventFeeDetailData = new TicketFeeDetail
                {
                    Id = checkForEventTicketFeeDetail.Id,
                    EventTicketAttributeId = command.EventTicketAttributeId,
                    FeeId = command.FeedId,
                    DisplayName = "",
                    ValueTypeId = command.ValueTypeId,
                    Value = command.Value,
                    IsEnabled = command.IsEnabled,
                    CreatedUtc = DateTime.UtcNow
                };
            }
            else
            {
                eventFeeDetailData = new TicketFeeDetail
                {
                    EventTicketAttributeId = command.EventTicketAttributeId,
                    FeeId = command.FeedId,
                    DisplayName = "",
                    ValueTypeId = command.ValueTypeId,
                    Value = command.Value,
                    IsEnabled = command.IsEnabled
                };
            }

            FIL.Contracts.DataModels.TicketFeeDetail result = _ticketFeeDetailRepository.Save(eventFeeDetailData);
            saveEventTicketFeeDataResult.Id = result.Id;
            return Task.FromResult<ICommandResult>(saveEventTicketFeeDataResult);
        }
    }
}