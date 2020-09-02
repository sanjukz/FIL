using HubSpot.NET.Core.Interfaces;
using FIL.Api.Events.Event.HubSpot;
using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Hubspot;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FIL.Api.Events.Handlers.HubSpot
{
    public class AbandonCartEventHandler : INotificationHandler<AbandonCartEvent>
    {
        private readonly IHubSpotApi _hubSpotApi;
        private readonly IHubspotCartTrackRepository _hubspotCartTrackRepository;
        private readonly FIL.Logging.ILogger _logger;

        public AbandonCartEventHandler(IHubSpotApi hubSpotApi, IHubspotCartTrackRepository hubspotCartTrackRepository, FIL.Logging.ILogger logger)
        {
            _hubSpotApi = hubSpotApi;
            _hubspotCartTrackRepository = hubspotCartTrackRepository;
            _logger = logger;
        }

        public Task Handle(AbandonCartEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                List<RasvHubspotContact> lstRasvHubspotContact = new List<RasvHubspotContact>();
                foreach (HubspotCartTrack item in notification.AbandonCart)
                {
                    RasvHubspotContact rasvHubspotContact = new RasvHubspotContact();
                    if (!string.IsNullOrWhiteSpace(item.EmailId) && item.EmailId.Contains("@"))
                    {
                        lstRasvHubspotContact.Add(new RasvHubspotContact
                        {
                            Email = item.EmailId,
                            KyazoongaCartAbandonment = "Yes"
                        });
                    }
                    else
                    {
                        var hubspotContact = _hubSpotApi.Contact.GetById<RasvHubspotContact>(item.HubspotVid);
                        if (hubspotContact != null)
                        {
                            lstRasvHubspotContact.Add(new RasvHubspotContact
                            {
                                Email = hubspotContact.Email,
                                KyazoongaCartAbandonment = "Yes"
                            });
                        }
                    }

                    HubspotCartTrack isNotAbandoned = _hubspotCartTrackRepository.GetByVId(item.HubspotVid);
                    if (isNotAbandoned != null)
                    {
                        _hubspotCartTrackRepository.DeleteHubspotCartTrack(new HubspotCartTrack
                        {
                            Id = isNotAbandoned.Id,
                        });
                    }
                }
                try
                {
                    _hubSpotApi.Contact.Batch(lstRasvHubspotContact);
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return Task.FromResult(0);
        }
    }
}