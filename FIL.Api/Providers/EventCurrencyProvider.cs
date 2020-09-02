using FIL.Api.Repositories;
using FIL.Configuration;
using System;
using System.Linq;

namespace FIL.Api.Providers
{
    public interface IEventCurrencyProvider
    {
        FIL.Contracts.DataModels.CurrencyType GetEventCurrency(FIL.Contracts.DataModels.Event @event);
    }

    public class EventCurrencyProvider : IEventCurrencyProvider
    {
        private ISettings _settings;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public EventCurrencyProvider(
            FIL.Logging.ILogger logger,
        IEventDetailRepository eventDetailRepository,
        IEventTicketDetailRepository eventTicketDetailRpository,
        IEventTicketAttributeRepository eventTicketAttributeRepository,
        ICurrencyTypeRepository currencyTypeRepository,
        ISettings settings)
        {
            _settings = settings;
            _logger = logger;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRpository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public FIL.Contracts.DataModels.CurrencyType GetEventCurrency(FIL.Contracts.DataModels.Event @event)
        {
            try
            {
                var eventDetails = _eventDetailRepository.GetByEventId(@event.Id);
                var eventTicketDetail = _eventTicketDetailRepository.GetByEventDetailId(eventDetails.Id).FirstOrDefault();
                var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id);
                var currency = _currencyTypeRepository.Get(eventTicketAttribute.CurrencyId);
                return currency;
            }
            catch (Exception e)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, e);
                return new FIL.Contracts.DataModels.CurrencyType { };
            }
        }
    }
}