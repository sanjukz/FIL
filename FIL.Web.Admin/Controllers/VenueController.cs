using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Venue;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.Venue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Kitms.Feel.Controllers
{
    public class VenueController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public VenueController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpPost]
        [Route("api/venue")]
        public async Task<SaveVenueResponseViewModel> SaveAsync([FromBody]VenueFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkForExisting = await GetByName(model.Name);
                if (checkForExisting.IsExisting)
                {
                    return new SaveVenueResponseViewModel
                    {
                        Success = false,
                        IsExisting = true
                    };
                }
                else
                {
                    var result = new { Success = true };
                    try
                    {
                        await _commandSender.Send(new SaveVenueCommand
                        {
                            Name = model.Name,
                            AddressLineOne = model.AddressLineOne,
                            AddressLineTwo = model.AddressLineTwo,
                            CityAltId = model.CityAltId,
                            Latitude = model.Latitude,
                            Longitude = model.Longitude,
                            HasImages = model.HasImages,
                            Prefix = model.Prefix

                        });
                        if (result.Success)
                        {
                            return new SaveVenueResponseViewModel
                            {
                                Success = true
                            };
                        }
                        else
                        {
                            return new SaveVenueResponseViewModel
                            {
                                Success = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new SaveVenueResponseViewModel
                        {
                            Success = false
                        };
                    }
                }
            }
            else
            {
                return new SaveVenueResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("api/venue/name/{name}")]
        public async Task<VenueSearchResponseViewModel> GetByName(string name)
        {
            var queryResult = await _querySender.Send(new VenueSearchQuery { Name = name });
            return new VenueSearchResponseViewModel { IsExisting = queryResult.IsExisting };
        }

        [HttpGet]
        [Route("api/venue/all")]
        public async Task<VenueResponseViewModel> GetAll()
        {
            var queryResult = await _querySender.Send(new VenueQuery());
            return queryResult.Venues != null ? new VenueResponseViewModel { Venues = queryResult.Venues } : null;
        }
    }
}
