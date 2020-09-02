using HubSpot.NET.Core.Interfaces;
using FIL.Api.Events.Event.HubSpot;
using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Hubspot;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FIL.Api.Events.Handlers.HubSpot
{
    public class RemoveCartEventHandler : INotificationHandler<RemoveCartEvent>
    {
        private readonly IHubSpotApi _hubSpotApi;
        private readonly IHubspotCartTrackRepository _hubspotCartTrackRepository;
        private readonly IUserRepository _userRepository;
        private readonly FIL.Logging.ILogger _logger;

        public RemoveCartEventHandler(IHubSpotApi hubSpotApi, IHubspotCartTrackRepository hubspotCartTrackRepository, IUserRepository userRepository, FIL.Logging.ILogger logger)
        {
            _hubSpotApi = hubSpotApi;
            _hubspotCartTrackRepository = hubspotCartTrackRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public Task Handle(RemoveCartEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                RasvHubspotContact vId;
                if (!string.IsNullOrWhiteSpace(notification.CartDetail.HubspotUTK))
                {
                    vId = _hubSpotApi.Contact.GetByUserToken<RasvHubspotContact>(notification.CartDetail.HubspotUTK);
                }
                else
                {
                    User user = _userRepository.GetByAltId(notification.CartDetail.UserAltId);
                    vId = _hubSpotApi.Contact.GetByEmail<RasvHubspotContact>(user.Email);
                }

                HubspotCartTrack isNotAbandoned = _hubspotCartTrackRepository.GetByVId((long)vId.Id);
                _hubspotCartTrackRepository.DeleteHubspotCartTrack(new HubspotCartTrack
                {
                    Id = isNotAbandoned.Id,
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