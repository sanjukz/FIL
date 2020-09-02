using HubSpot.NET.Api.Deal.Dto;
using HubSpot.NET.Core.Interfaces;
using FIL.Api.Events.Event.HubSpot;
using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models;
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
    public class TransactionEventHandler : INotificationHandler<TransactionEvent>
    {
        private readonly IHubSpotApi _hubSpotApi;
        private readonly IOrderConfirmationProvider _orderConfirmationProvider;
        private readonly IHubspotCartTrackRepository _hubspotCartTrackRepository;
        private readonly FIL.Logging.ILogger _logger;

        public TransactionEventHandler(IHubSpotApi hubSpotApi, IOrderConfirmationProvider orderConfirmationProvider, IHubspotCartTrackRepository hubspotCartTrackRepository, FIL.Logging.ILogger logger)
        {
            _hubSpotApi = hubSpotApi;
            _orderConfirmationProvider = orderConfirmationProvider;
            _hubspotCartTrackRepository = hubspotCartTrackRepository;
            _logger = logger;
        }

        public Task Handle(TransactionEvent notification, CancellationToken cancellationToken)
        {
            // TODO: XXX: HUB:
            // Needs to check its a RASV transaction then,
            // This needs to loop over all of the Transaction items and then send
            // YES for each of RIDES, BODYWORLDS fields if theyre in the transaction
            // fields have spaces replaced with _ in API calls
            // field - value
            // RMS Ride Pass Purchased - YES if present
            // RMS Bodyworlds Purchased - YES if present
            // RMS Ticket Type - Online Ticket Saver or RACV Member
            // RMS 2019 Ticket Date of Purchase - Date for Hubspot of purchase

            //IN case of multiple tickets do we need to send by comma separated
            try
            {
                if (notification.TransactionStatus == Contracts.Enums.TransactionStatus.UnderPayment)
                {
                    var contact = _hubSpotApi.Contact.GetByEmail<RasvHubspotContact>(notification.EmailId) ?? new RasvHubspotContact();
                    contact = _hubSpotApi.Contact.CreateOrUpdate(new RasvHubspotContact
                    {
                        //Id = contact.Id,
                        Email = contact.Email ?? notification.EmailId,
                        ZipCode = notification.ZipCode
                    });

                    HubspotCartTrack hubspotCartTrack = _hubspotCartTrackRepository.GetByVId((long)contact.Id);
                    if (hubspotCartTrack == null)
                    {
                        _hubspotCartTrackRepository.SaveHubspotCartTrack(new HubspotCartTrack
                        {
                            HubspotVid = (long)contact.Id,
                            EmailId = contact.Email ?? notification.EmailId,
                        });
                    }
                }
                else
                {
                    var orderConfirmation = _orderConfirmationProvider.Get(notification.TransactionId, false, Contracts.Enums.Channels.Website);

                    if (orderConfirmation.orderConfirmationSubContainer[0].Event.AltId.ToString().ToUpper() == "1F0257FA-EEA6-4469-A7BC-B878A215C8A9")
                    {
                        //orderConfirmation.Transaction.CreatedUtc = orderConfirmation.Transaction.CreatedUtc.AddHours(10);
                        bool IsRMSRidePassPurchased = false;
                        bool IsRMSBodyworldsPurchased = false;
                        var RMSTicketType = new List<string>();
                        var DealsRMSTicketType = new List<string>();
                        decimal? TicketAmount = 0;

                        if (orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554440 || w.EventDetail.Id == 554402).Count() > 0)
                        {
                            RMSTicketType.Add("General Admission");
                            DealsRMSTicketType.Add("General Admission");
                            List<SubEventContainer> TransactionDetail = orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554402 || w.EventDetail.Id == 554440).ToList();
                            TicketAmount += TransactionDetail[0].TransactionDetail.Sum(s => s.PricePerTicket * s.TotalTickets);
                        }
                        if (orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554441 || w.EventDetail.Id == 554403).Count() > 0)
                        {
                            RMSTicketType.Add("RACV Member 2019");
                            DealsRMSTicketType.Add("RACV Member");
                            List<SubEventContainer> TransactionDetail = orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554403 || w.EventDetail.Id == 554441).ToList();
                            TicketAmount += TransactionDetail[0].TransactionDetail.Sum(s => s.PricePerTicket * s.TotalTickets);
                        }

                        if (orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554442 || w.EventDetail.Id == 554404).Count() > 0)
                        {
                            RMSTicketType.Add("After 5pm 2019");
                            DealsRMSTicketType.Add("After 5");
                            List<SubEventContainer> TransactionDetail = orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554404 || w.EventDetail.Id == 554442).ToList();
                            TicketAmount += TransactionDetail[0].TransactionDetail.Sum(s => s.PricePerTicket * s.TotalTickets);
                        }

                        if (orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554443 || w.EventDetail.Id == 554405).Count() > 0)
                        {
                            RMSTicketType.Add("Group Bookings 2019");
                            DealsRMSTicketType.Add("Group Bookings");
                            List<SubEventContainer> TransactionDetail = orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554405 || w.EventDetail.Id == 554443).ToList();
                            TicketAmount += TransactionDetail[0].TransactionDetail.Sum(s => s.PricePerTicket * s.TotalTickets);
                        }

                        if (orderConfirmation.orderConfirmationSubContainer[0].subEventContainer.Where(w => w.EventDetail.Id == 554439 || w.EventDetail.Id == 554401).Count() > 0)
                        {
                            IsRMSRidePassPurchased = true;
                            RMSTicketType.Add("Ride Pass");
                            DealsRMSTicketType.Add("Ride Pass");
                        }

                        if (RMSTicketType.Any() || IsRMSBodyworldsPurchased || IsRMSRidePassPurchased)
                        {
                            var contact = _hubSpotApi.Contact.GetByEmail<RasvHubspotContact>(orderConfirmation.Transaction.EmailId) ?? new RasvHubspotContact();
                            //to set to AEST
                            orderConfirmation.Transaction.CreatedUtc = orderConfirmation.Transaction.CreatedUtc.AddHours(10);

                            var dateTimeOffset = new DateTimeOffset(new DateTime(orderConfirmation.Transaction.CreatedUtc.Year, orderConfirmation.Transaction.CreatedUtc.Month, orderConfirmation.Transaction.CreatedUtc.Day, 00, 00, 00, 00, DateTimeKind.Utc));
                            var unixDateTime = dateTimeOffset.ToUnixTimeMilliseconds();

                            string rmsTicketPurchased = "";
                            rmsTicketPurchased = (string.IsNullOrWhiteSpace(contact.RMSTicketPurchased)) ? "2019" : contact.RMSTicketPurchased;
                            if (!rmsTicketPurchased.Contains("2019"))
                            {
                                rmsTicketPurchased = rmsTicketPurchased + ";2019";
                            }

                            string rmsRidePassPurchased = IsRMSRidePassPurchased ? "Yes" : "No";

                            rmsRidePassPurchased = (contact.RMSRidePassPurchased == "Yes") ? "Yes" : rmsRidePassPurchased;

                            if (!string.IsNullOrWhiteSpace(contact.RMSTicketType))
                            {
                                foreach (var item in contact.RMSTicketType.Split(";"))
                                {
                                    if (!RMSTicketType.Contains(item))
                                    {
                                        RMSTicketType.Add(item);
                                    }
                                }
                            }

                            string zipCode = "3032";
                            if (orderConfirmation.orderConfirmationSubContainer[0].subEventContainer[0].Zipcode != null)
                            {
                                zipCode = orderConfirmation.orderConfirmationSubContainer[0].subEventContainer[0].Zipcode.Postalcode.ToString();
                            }

                            contact = _hubSpotApi.Contact.CreateOrUpdate(new RasvHubspotContact
                            {
                                //Id = contact.Id,
                                Email = contact.Email ?? orderConfirmation.Transaction.EmailId,
                                RMS2019TicketDateOfPurchase = unixDateTime.ToString(),
                                RMSRidePassPurchased = rmsRidePassPurchased,
                                RMSTicketPurchased = rmsTicketPurchased,
                                ZipCode = zipCode,
                                RMSTicketType = RMSTicketType.Any() ? string.Join(';', RMSTicketType) : null
                            });

                            try
                            {
                                //TicketAmount = (double?)(TicketAmount ?? (decimal?)0.0),
                                _hubSpotApi.Deal.Create<RasvHubspotDeal>(new RasvHubspotDeal
                                {
                                    //Pipeline = "75e28846-ad0d-4be2-a027-5e1da6590b98",
                                    Pipeline = "ab6f7db4-8afc-4733-bb09-54c93a630403",
                                    Stage = "b874331f-c417-422d-83eb-43773f08819a",
                                    Name = orderConfirmation.Transaction.Id.ToString(),
                                    CloseDate = unixDateTime.ToString(),
                                    RMSTicketType = string.Join(';', DealsRMSTicketType),
                                    Amount = (double?)(orderConfirmation.Transaction.GrossTicketAmount ?? (decimal?)0.0),
                                    TicketAmount = (double?)(TicketAmount ?? (decimal?)0.0),
                                    Associations = new DealHubSpotAssociations
                                    {
                                        AssociatedContacts = new[] { (long)contact.Id }
                                    }
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.Log(LogCategory.Error, ex);
                            }

                            HubspotCartTrack isNotAbandoned = _hubspotCartTrackRepository.GetByVId((long)contact.Id);
                            if (isNotAbandoned != null)
                            {
                                _hubspotCartTrackRepository.DeleteHubspotCartTrack(new HubspotCartTrack
                                {
                                    Id = isNotAbandoned.Id,
                                });
                            }
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