using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Zipcode;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.Zipcode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Kitms.Feel.Controllers
{
    public class ZipcodeController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public ZipcodeController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpPost]
        [Route("api/zipcode")]
        public async Task<SaveZipcodeResponseViewModel> SaveAsync([FromBody]ZipcodeFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkForExisting = await GetByZipcode(model.zipcode);
                if (checkForExisting.IsExisting)
                {
                    return new SaveZipcodeResponseViewModel
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
                        await _commandSender.Send(new SaveZipcodeCommand
                        {
                            Zipcode = model.zipcode,
                            Region = model.Region,
                            CityAltId = model.CityAltId

                        });
                        if (result.Success)
                        {
                            return new SaveZipcodeResponseViewModel
                            {
                                Success = true
                            };
                        }
                        else
                        {
                            return new SaveZipcodeResponseViewModel
                            {
                                Success = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new SaveZipcodeResponseViewModel
                        {
                            Success = false
                        };
                    }
                }
            }
            else
            {
                return new SaveZipcodeResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("api/zipcode/name/{name}")]
        public async Task<ZipcodeSearchResponseViewModel> GetByZipcode(string zipcode)
        {
            var queryResult = await _querySender.Send(new ZipcodeSearchQuery { Zipcode = zipcode });
            return new ZipcodeSearchResponseViewModel { IsExisting = queryResult.IsExisting };
        }

        [HttpGet]
        [Route("api/zipcode/{altId}")]
        public async Task<ZipcodeResponseViewModel> GetByAltId(Guid altId)
        {
            var queryResult = await _querySender.Send(new ZipcodeQuery { CityAltId = altId });
            return queryResult.Zipcodes != null ? new ZipcodeResponseViewModel { Zipcodes = queryResult.Zipcodes } : null;
        }
    }
}
