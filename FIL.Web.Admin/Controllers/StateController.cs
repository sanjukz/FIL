using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.City;
using FIL.Contracts.Queries.State;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.City;
using FIL.Web.Kitms.Feel.ViewModels.State;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Kitms.Feel.Controllers
{
    public class StateController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public StateController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpPost]
        [Route("api/state")]
        public async Task<SaveStateResponseViewModel> SaveAsync([FromBody]StateFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkForExisting = await GetByName(model.Name);
                if (checkForExisting.IsExisting)
                {
                    return new SaveStateResponseViewModel
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
                        await _commandSender.Send(new SaveStateCommand
                        {
                            Name = model.Name,
                            Abbreviation = model.Abbreviation,
                            CountryAltId = model.CountryAltId
                        });
                        if (result.Success)
                        {
                            return new SaveStateResponseViewModel
                            {
                                Success = true
                            };
                        }
                        else
                        {
                            return new SaveStateResponseViewModel
                            {
                                Success = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new SaveStateResponseViewModel
                        {
                            Success = false
                        };
                    }
                }
            }
            else
            {
                return new SaveStateResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("api/state/name/{name}")]
        public async Task<StateSearchResponseViewModel> GetByName(string name)
        {
            var queryResult = await _querySender.Send(new StateSearchQuery { Name = name });
            return new StateSearchResponseViewModel { IsExisting = queryResult.IsExisting };
        }

        [HttpGet]
        [Route("api/state/{altId}")]
        public async Task<StateResponseViewModel> GetByAltId(Guid altId)
        {
            var queryResult = await _querySender.Send(new StateQuery { CountryAltId = altId });
            return queryResult.States != null ? new StateResponseViewModel { States = queryResult.States } : null;
        }

        [HttpGet]
        [Route("api/cities/{stateAltId}")]
        public async Task<CityResponseViewModel> GetAllCities(Guid stateAltId)
        {
            var queryResult = await _querySender.Send(new CitiesQuery { StateAltId = stateAltId });
            return queryResult.Success ? new CityResponseViewModel { Cities = queryResult.Cities } : new CityResponseViewModel { };
        }
    }
}
