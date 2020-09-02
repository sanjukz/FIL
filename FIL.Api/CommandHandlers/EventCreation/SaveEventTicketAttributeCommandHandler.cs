using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventCreation;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventCreation
{
    public class SaveEventTicketAttributeCommandHandler : BaseCommandHandlerWithResult<SaveEventTicketAttributeCommand, SaveEventTicketAttributeResult>
    {
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;

        public SaveEventTicketAttributeCommandHandler(IEventTicketAttributeRepository eventTicketAttributeRepository, IMediator mediator) : base(mediator)
        {
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
        }

        protected override Task<ICommandResult> Handle(SaveEventTicketAttributeCommand command)
        {
            var EventTickeAttribute = new EventTicketAttribute();
            SaveEventTicketAttributeResult saveEventTicketAttributelResult = new SaveEventTicketAttributeResult();
            var checkForEventTicketAttribute = _eventTicketAttributeRepository.Get(command.Id);

            if (checkForEventTicketAttribute != null)
            {
                EventTickeAttribute = new EventTicketAttribute
                {
                    Id = checkForEventTicketAttribute.Id,
                    EventTicketDetailId = command.EventTicketDetailId,
                    SalesStartDateTime = command.SalesStartDateTime,
                    SalesEndDatetime = command.SalesEndDatetime,
                    TicketTypeId = (TicketType)command.TicketTypeId,
                    ChannelId = (Channels)command.ChannelId,
                    CurrencyId = command.CurrencyId,
                    AvailableTicketForSale = command.AvailableTicketForSale,
                    RemainingTicketForSale = command.AvailableTicketForSale,
                    TicketCategoryDescription = command.TicketCategoryDescription,
                    ViewFromStand = command.ViewFromStand,
                    IsSeatSelection = false,
                    Price = command.price,
                    IsInternationalCardAllowed = false,
                    IsEMIApplicable = false,
                    LocalPrice = command.price,
                    SeasonPackage = false,
                    SeasonPackagePrice = 0,
                    LocalCurrencyId = command.CurrencyId,
                    IsEnabled = command.IsEnabled,
                    CreatedUtc = DateTime.UtcNow,
                    CreatedBy = command.ModifiedBy
                };
            }
            else
            {
                EventTickeAttribute = new EventTicketAttribute
                {
                    Id = command.Id,
                    EventTicketDetailId = command.EventTicketDetailId,
                    SalesStartDateTime = command.SalesStartDateTime,
                    SalesEndDatetime = command.SalesEndDatetime,
                    TicketTypeId = (TicketType)command.TicketTypeId,
                    ChannelId = (Channels)command.ChannelId,
                    CurrencyId = command.CurrencyId,
                    AvailableTicketForSale = command.AvailableTicketForSale,
                    RemainingTicketForSale = command.AvailableTicketForSale,
                    TicketCategoryDescription = command.TicketCategoryDescription,
                    ViewFromStand = command.ViewFromStand,
                    IsSeatSelection = false,
                    Price = command.price,
                    IsInternationalCardAllowed = false,
                    IsEMIApplicable = false,
                    LocalPrice = command.price,
                    SeasonPackage = false,
                    SeasonPackagePrice = 0,
                    LocalCurrencyId = command.CurrencyId,
                    IsEnabled = command.IsEnabled,
                    CreatedUtc = DateTime.UtcNow,
                    CreatedBy = command.ModifiedBy
                };
            }

            FIL.Contracts.DataModels.EventTicketAttribute result = _eventTicketAttributeRepository.Save(EventTickeAttribute);
            saveEventTicketAttributelResult.Id = result.Id;
            return Task.FromResult<ICommandResult>(saveEventTicketAttributelResult);
        }
    }
}