using HubSpot.NET.Core.Interfaces;
using FIL.Api.Events.Event.HubSpot;
using FIL.Contracts.Models.Hubspot;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FIL.Api.Events.Handlers.HubSpot
{
    public class LoginEventHandler : INotificationHandler<LoginEvent>
    {
        private readonly IHubSpotApi _hubSpotApi;
        private readonly FIL.Logging.ILogger _logger;

        public LoginEventHandler(IHubSpotApi hubSpotApi, FIL.Logging.ILogger logger)
        {
            _hubSpotApi = hubSpotApi;
            _logger = logger;
        }

        public Task Handle(LoginEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _hubSpotApi.Contact.CreateOrUpdate(new RasvHubspotContact
                {
                    Email = notification.User.Email,
                    RMS2019TicketPlatformLogin = "Yes",
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return Task.FromResult(0);
        }
    }
}