using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Country;
using FIL.Foundation.Senders;
using FIL.Web.Admin.ViewModels.Country;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Admin.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public CountryController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpPost]
        [Route("api/country")]
        public async Task<SaveCountryResponseViewModel> SaveAsync([FromBody]CountryFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkForExisting = await GetByName(model.Name);
                if (checkForExisting.IsExisting)
                {
                    return new SaveCountryResponseViewModel
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
                        await _commandSender.Send(new SaveCountryCommand
                        {
                            Name = model.Name,
                            IsoAlphaTwoCode = model.IsoAlphaTwoCode,
                            IsoAlphaThreeCode = model.IsoAlphaThreeCode
                        });
                        if (result.Success)
                        {
                            return new SaveCountryResponseViewModel
                            {
                                Success = true
                            };
                        }
                        else
                        {
                            return new SaveCountryResponseViewModel
                            {
                                Success = false
                            };
                        }
                    }
                    catch(Exception ex)
                    {
                        return new SaveCountryResponseViewModel
                        {
                            Success = false
                        };
                    }                    
                }
            }
            else
            {
                return new SaveCountryResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("api/country/all")]
        public async Task<CountryResponseViewModel> GetAll()
        {
            var queryResult = await _querySender.Send(new CountryQuery());
            return queryResult.Countries != null ? new CountryResponseViewModel { Countries = queryResult.Countries } : null;
        }

        [HttpGet]
        [Route("api/country/{altId}")]
        public async Task<CountryResponseViewModel> GetByAltId(Guid altId)
        {
            var queryResult = await _querySender.Send(new CountryQuery { AltId = altId });
            return queryResult.Countries != null ? new CountryResponseViewModel { Countries = queryResult.Countries } : null;
        }

        [HttpGet]
        [Route("api/country/name/{name}")]
        public async Task<CountrySearchResponseViewModel> GetByName(string name)
        {
            var queryResult = await _querySender.Send(new CountrySearchQuery { Name = name });
            return new CountrySearchResponseViewModel { IsExisting = queryResult.IsExisting };
        }
    }
}
