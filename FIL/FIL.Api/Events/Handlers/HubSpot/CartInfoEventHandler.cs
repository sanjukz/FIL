using HubSpot.NET.Core.Interfaces;
using FIL.Api.Events.Event.HubSpot;
using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Hubspot;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FIL.Api.Events.Handlers.HubSpot
{
    public class CartInfoEventHandler : INotificationHandler<CartInfoEvent>
    {
        private readonly IHubSpotApi _hubSpotApi;
        private readonly IHubspotCartTrackRepository _hubspotCartTrackRepository;
        private readonly IUserRepository _userRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CartInfoEventHandler(IHubSpotApi hubSpotApi, IHubspotCartTrackRepository hubspotCartTrackRepository, IUserRepository userRepository, FIL.Logging.ILogger logger)
        {
            _hubSpotApi = hubSpotApi;
            _hubspotCartTrackRepository = hubspotCartTrackRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public Task Handle(CartInfoEvent notification, CancellationToken cancellationToken)
        {
            // TODO: XXX: HUB: I dont think this has to do anything.
            // You should have the service look at transactions not completed (Status != Success)
            // within the abandonment period (~15 min)
            // Must also check the user doesnt have a different completed cart, because then this doesnt matter
            // The Abandonment is just YES / NO field named Kyazoonga Cart Abandonment (kyazoonga_cart_abandonment in the api)

            // will look when work on service
            try
            {
                bool IsRMSRidePassPurchased = false;
                bool IsRMSBodyworldsPurchased = false;
                string RMS2019TicketPlatformLogin = "No";
                var RMSTicketType = new List<string>();

                if ((notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554440).Count() > 0) || (notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554402).Count() > 0))
                {
                    RMSTicketType.Add("General Admission");
                }

                if ((notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554441).Count() > 0) || (notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554403).Count() > 0))
                {
                    RMSTicketType.Add("RACV Member 2019");
                }

                if ((notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554442).Count() > 0) || (notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554404).Count() > 0))
                {
                    RMSTicketType.Add("After 5pm 2019");
                }

                if ((notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554443).Count() > 0) || (notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554405).Count() > 0))
                {
                    RMSTicketType.Add("Group Bookings 2019");
                }

                if ((notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554439).Count() > 0) || (notification.CartDetail.EventDetailId.Where(w => w.EventDetailId == 554403).Count() > 0))
                {
                    IsRMSRidePassPurchased = true;
                    RMSTicketType.Add("Ride Pass");
                }

                if (RMSTicketType.Any() || IsRMSBodyworldsPurchased || IsRMSRidePassPurchased)
                {
                    RasvHubspotContact vId = null;
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(notification.CartDetail.HubspotUTK) && string.IsNullOrWhiteSpace(notification.CartDetail.Email))
                        {
                            RMS2019TicketPlatformLogin = "Yes";
                            vId = _hubSpotApi.Contact.GetByUserToken<RasvHubspotContact>(notification.CartDetail.HubspotUTK);
                        }
                        else if (notification.CartDetail.UserAltId != new Guid())
                        {
                            RMS2019TicketPlatformLogin = "No";
                            User user = _userRepository.GetByAltId(notification.CartDetail.UserAltId);
                            vId = _hubSpotApi.Contact.GetByEmail<RasvHubspotContact>(user.Email);

                            if (!string.IsNullOrWhiteSpace(vId.RMS2019TicketPlatformLogin))
                            {
                                RMS2019TicketPlatformLogin = vId.RMS2019TicketPlatformLogin;
                            }

                            vId = _hubSpotApi.Contact.CreateOrUpdate(new RasvHubspotContact
                            {
                                Email = user.Email,
                                RMS2019TicketPlatformLogin = RMS2019TicketPlatformLogin,
                            });
                        }
                        else
                        {
                            RMS2019TicketPlatformLogin = "No";
                            vId = _hubSpotApi.Contact.GetByEmail<RasvHubspotContact>(notification.CartDetail.Email);
                            if (!string.IsNullOrWhiteSpace(vId.RMS2019TicketPlatformLogin))
                            {
                                RMS2019TicketPlatformLogin = vId.RMS2019TicketPlatformLogin;
                            }

                            vId = _hubSpotApi.Contact.CreateOrUpdate(new RasvHubspotContact
                            {
                                Email = notification.CartDetail.Email,
                                RMS2019TicketPlatformLogin = RMS2019TicketPlatformLogin,
                                RMSNewsletterSubscriber2019 = notification.CartDetail.IsMailOpt ? "Yes" : "No",
                                RMSMarketingOptIn = notification.CartDetail.IsMailOpt ? "Yes" : "No"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrWhiteSpace(notification.CartDetail.Email))
                        {
                            vId = _hubSpotApi.Contact.CreateOrUpdate(new RasvHubspotContact
                            {
                                Email = notification.CartDetail.Email,
                                FirstName = notification.CartDetail.FirstName,
                                LastName = notification.CartDetail.LastName,
                                Phone = notification.CartDetail.Phone,
                                RMS2019TicketPlatformLogin = "No",
                                RMSNewsletterSubscriber2019 = notification.CartDetail.IsMailOpt ? "Yes" : "No",
                                RMSMarketingOptIn = notification.CartDetail.IsMailOpt ? "Yes" : "No"
                            });
                        }
                    }

                    if (vId != null)
                    {
                        HubspotCartTrack hubspotCartTrack = _hubspotCartTrackRepository.GetByVId((long)vId.Id);
                        if (hubspotCartTrack == null)
                        {
                            _hubspotCartTrackRepository.SaveHubspotCartTrack(new HubspotCartTrack
                            {
                                HubspotVid = (long)vId.Id,
                                EmailId = !string.IsNullOrWhiteSpace(vId.Email) ? vId.Email : notification.CartDetail.HubspotUTK,
                            });
                        }
                    }
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