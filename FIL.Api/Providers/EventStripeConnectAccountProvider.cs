using FIL.Api.Repositories;
using FIL.Configuration;
using System;

namespace FIL.Api.Providers
{
    public interface IEventStripeConnectAccountProvider
    {
        FIL.Contracts.Enums.StripeAccount GetEventStripeAccount(long EventId, FIL.Contracts.Enums.Channels channel);
    }

    public class EventStripeConnectAccountProvider : IEventStripeConnectAccountProvider
    {
        private ISettings _settings;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IEventStripeAccountMappingRepository _eventStripeAccountMappingRepository;

        public EventStripeConnectAccountProvider(
            FIL.Logging.ILogger logger,
            IEventStripeAccountMappingRepository eventStripeAccountMappingRepository,
            ISettings settings)
        {
            _settings = settings;
            _logger = logger;
            _eventStripeAccountMappingRepository = eventStripeAccountMappingRepository;
        }

        public FIL.Contracts.Enums.StripeAccount GetEventStripeAccount(long EventId, FIL.Contracts.Enums.Channels channel)
        {
            try
            {
                var eventStripeAccount = _eventStripeAccountMappingRepository.GetByEventId(EventId);
                if (eventStripeAccount != null)
                {
                    return eventStripeAccount.StripeAccountId;
                }
                else
                {
                    return Contracts.Enums.StripeAccount.None;
                }
            }
            catch (Exception e)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, e);
                return Contracts.Enums.StripeAccount.None;
            }
        }
    }
}