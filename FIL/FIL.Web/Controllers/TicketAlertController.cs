using System;
using System.Threading.Tasks;
using FIL.Web.Feel.ViewModels.TicketAlert;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Contracts.Queries.TicketAlert;
using FIL.Contracts.Commands.TicketAlert;
using FIL.Web.Feel.Providers;
using FIL.Contracts.Enums;
using System.Collections.Generic;
using FIL.Messaging.Models.Emails;
using System.Linq;
using FIL.Web.Core;
using FIL.Messaging.Senders;
using FIL.Web.Core.UrlsProvider;
using FIL.Contracts.QueryResults.TicketAlert;
using FIL.Web.Core.Providers;
using FIL.Contracts.Commands.RegistrationEvent;
using Microsoft.Extensions.Caching.Memory;

namespace FIL.Web.Feel.Controllers
{
    public class TicketAlertController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ICommandSender _commandSender;
        private readonly IConfirmationEmailSender _confirmationEmailSender;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly IClientIpProvider _clientIpProvider;

        public TicketAlertController(IQuerySender querySender,
             IConfirmationEmailSender confirmationEmailSender,
              ISiteIdProvider siteIdProvider,
              ISiteUrlsProvider siteUrlsProvider,
              IMemoryCache memoryCache,
            ICommandSender commandSender,
            IClientIpProvider clientIpProvider)
        {
            _querySender = querySender;
            _commandSender = commandSender;
            _confirmationEmailSender = confirmationEmailSender;
            _memoryCache = memoryCache;
            _siteIdProvider = siteIdProvider;
            _siteUrlsProvider = siteUrlsProvider;
            _clientIpProvider = clientIpProvider;
        }

        [HttpGet]
        [Route("api/ticketAlert/{eventAltId}")]
        public async Task<TicketAlertEventMappingDataViewModel> Get(Guid eventAltId)
        {
            if (!_memoryCache.TryGetValue($"alert_{eventAltId}", out Contracts.QueryResults.TicketAlertQueryResult queryResult))
            {
                var cachequeryResult = await _querySender.Send(new TicketAlertQuery
                {
                    altId = eventAltId
                });
                queryResult = cachequeryResult;
                _memoryCache.Set($"alert_{eventAltId}", queryResult, DateTime.Now.AddMinutes(5));
            }
            return new TicketAlertEventMappingDataViewModel
            {
                Event = queryResult.Event,
                Countries = queryResult.Countries,
                AllCountriesModel = queryResult.AllCountries,
                EventDetails = queryResult.EventDetails,
                TicketAlertEventMappings = queryResult.ticketAlertEventMappings
            };
        }

        [HttpGet]
        [Route("api/ticketAlertReport")]
        public async Task<TicketAlertReportingResponseViewModel> GetTicketAlertReport()
        {
            var queryResult = await _querySender.Send(new TicketAlertReportQuery
            {
            });

            return new TicketAlertReportingResponseViewModel
            {
                TicketAlertData = queryResult.TicketAlertReport
            };
        }

        [HttpPost]
        [Route("api/ticketAlert/signup")]
        public async Task<TicketAlertUserMappingResponseDataViewModel> saveSignUp([FromBody]TicketAlertUserMappingRequestDataViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    model.CountriesAltIds = model.CountriesAltIds.Distinct().ToList();
                    model.TicketAlertEventMapping = model.TicketAlertEventMapping.Distinct().ToList();
                    var queryResult = await _querySender.Send(new TicketAlertUesrMappingQuery
                    {
                        Countries = model.CountriesAltIds,
                        Email = model.Email,
                        EventAltId = model.EventAltId,
                        EventDetailId = 0,
                        TicketAlertEvents = model.TicketAlertEventMapping
                    });
                    if (queryResult.IsAlredySignUp)
                    {
                        return new TicketAlertUserMappingResponseDataViewModel
                        {
                            IsAlreadySignUp = true,
                            Success = false
                        };
                    }
                    else
                    {
                        FIL.Contracts.Enums.TourTravelPackages TourTravelPackages = FIL.Contracts.Enums.TourTravelPackages.No;
                        if (model.TourAndTavelPackage == 1)
                        {
                            TourTravelPackages = FIL.Contracts.Enums.TourTravelPackages.Yes;
                        }
                        await _commandSender.Send(new TicketAlertUserMappingCommand
                        {
                            CountriesAltId = model.CountriesAltIds,
                            EventAltId = model.EventAltId,
                            EventDetailId = 0,
                            FirstName = model.FirstName,
                            TourTravelPackages = TourTravelPackages,
                            LastName = model.LastName,
                            PhoneCode = model.PhoneCode,
                            PhoneNumber = model.PhoneNumber,
                            Email = model.Email,
                            NumberOfTickets = model.NoOfTickets,
                            TicketAlertEvents = model.TicketAlertEventMapping,
                            Ip = _clientIpProvider.Get()
                        });

                        try
                        {
                            var siteUrls = _siteUrlsProvider.GetSiteUrl(_siteIdProvider.GetSiteId());
                            Email email = new Email();
                            email.To = model.Email;
                            email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
                            email.TemplateName = "FILTicketAlert";

                            email.Variables = new Dictionary<string, object>
                            {
                                ["eventname"] = model.EventName
                            };
                            await _confirmationEmailSender.Send(email);
                            return new TicketAlertUserMappingResponseDataViewModel
                            {
                                IsAlreadySignUp = false,
                                Success = true
                            };
                        }
                        catch (Exception e)
                        {
                            return new TicketAlertUserMappingResponseDataViewModel
                            {
                                IsAlreadySignUp = false,
                                Success = true
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    return new TicketAlertUserMappingResponseDataViewModel
                    {
                        IsAlreadySignUp = false,
                        Success = false
                    };
                }
            }
            else
            {
                return new TicketAlertUserMappingResponseDataViewModel
                {
                    IsAlreadySignUp = false,
                    Success = false
                };
            }
        }
    }
}
