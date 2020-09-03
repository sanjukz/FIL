using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.City;
using FIL.Foundation.Senders;
using FIL.Web.Admin.ViewModels.City;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Admin.Controllers
{
    public class CityController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public CityController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpPost]
        [Route("api/city")]
        public async Task<SaveCityResponseViewModel> SaveAsync([FromBody]CityFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkForExisting = await GetByName(model.Name);
                if (checkForExisting.IsExisting)
                {
                    return new SaveCityResponseViewModel
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
                        await _commandSender.Send(new SaveCityCommand
                        {
                            Name = model.Name,
                            StateAltId = model.StateAltId
                        });
                        if (result.Success)
                        {
                            return new SaveCityResponseViewModel
                            {
                                Success = true
                            };
                        }
                        else
                        {
                            return new SaveCityResponseViewModel
                            {
                                Success = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new SaveCityResponseViewModel
                        {
                            Success = false
                        };
                    }
                }
            }
            else
            {
                return new SaveCityResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("api/city/name/{name}")]
        public async Task<CitySearchResponseViewModel> GetByName(string name)
        {
            var queryResult = await _querySender.Send(new CitySearchQuery { Name = name });
            return new CitySearchResponseViewModel { IsExisting = queryResult.IsExisting };
        }

        [HttpGet]
        [Route("api/city/{altId}")]
        public async Task<CityResponseViewModel> GetByAltId(Guid altId)
        {
            var queryResult = await _querySender.Send(new CityQuery { StateAltId = altId });
            return queryResult.Cities != null ? new CityResponseViewModel { Cities = queryResult.Cities } : null;
        }
    }
}
