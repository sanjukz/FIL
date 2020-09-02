using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using Stripe;
using System;

namespace FIL.Api.Providers
{
    public interface IGenerateStripeConnectProvider
    {
        string GenerateStripeAccessToken(string authorization_code,
            Contracts.Enums.Channels channel,
            long eventId);
    }

    public class GenerateStripeConnectProvider : IGenerateStripeConnectProvider
    {
        private ISettings _settings;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IEventStripeAccountMappingRepository _eventStripeAccountMappingRepository;

        public GenerateStripeConnectProvider(
            FIL.Logging.ILogger logger,
            IEventStripeAccountMappingRepository eventStripeAccountMappingRepository,
            ISettings settings)
        {
            _settings = settings;
            _logger = logger;
            _eventStripeAccountMappingRepository = eventStripeAccountMappingRepository;
        }

        public string GenerateStripeAccessToken(string authorization_code,
            Contracts.Enums.Channels channel,
            long eventId)
        {
            try
            {
                var eventStripeAccount = _eventStripeAccountMappingRepository.GetByEventId(eventId);
                StripeConfiguration.ApiKey = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.Feel.SecretKey);
                if (channel == Contracts.Enums.Channels.Feel && eventStripeAccount != null && eventStripeAccount.StripeAccountId == Contracts.Enums.StripeAccount.StripeAustralia) // If event belongs to StripeAustralia
                {
                    StripeConfiguration.ApiKey = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.FeelAustralia.SecretKey);
                }
                else if (channel == Contracts.Enums.Channels.Feel && eventStripeAccount != null && eventStripeAccount.StripeAccountId == Contracts.Enums.StripeAccount.StripeIndia) // If event belongs to StripeIndia
                {
                    StripeConfiguration.ApiKey = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.FeelIndia.SecretKey);
                }
                if (channel == Contracts.Enums.Channels.Website)
                {
                    StripeConfiguration.ApiKey = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.SecretKey);
                }
                var options = new OAuthTokenCreateOptions
                {
                    GrantType = "authorization_code",
                    Code = authorization_code,
                };
                var service = new OAuthTokenService();
                var response = service.Create(options);
                return response.StripeUserId;
            }
            catch (Exception e)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, e);
                return "";
            }
        }
    }
}