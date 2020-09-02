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
    public class VisitorInfoEventHandler : INotificationHandler<VisitorInfoEvent>
    {
        private readonly IHubSpotApi _hubSpotApi;
        private readonly FIL.Logging.ILogger _logger;

        public VisitorInfoEventHandler(IHubSpotApi hubSpotApi, FIL.Logging.ILogger logger)
        {
            _hubSpotApi = hubSpotApi;
            _logger = logger;
        }

        public Task Handle(VisitorInfoEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _hubSpotApi.Contact.CreateOrUpdate(new RasvHubspotContact
                {
                    Email = notification.User.Email,
                    FirstName = notification.User.FirstName,
                    LastName = notification.User.LastName,
                    Phone = notification.User.PhoneNumber,
                    RMS2019TicketPlatformLogin = "No",
                    RMSNewsletterSubscriber2019 = notification.User.IsRASVMailOPT == true ? "Yes" : "No",
                    RMSMarketingOptIn = notification.User.IsRASVMailOPT == true ? "Yes" : "No"
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