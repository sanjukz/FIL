using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.EventWizard;
using FIL.Foundation.Senders;
using FIL.Web.Admin.ViewModels.EventWizard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Admin.Controllers
{
    public class EventWizardController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public EventWizardController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpPost]
        [Route("api/event")]
        public IActionResult Save([FromBody]EventWizardDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new { Succeeded = true };
                //_commandSender.Send(new EventWizardCommand
                //{
                //    // figuring out what and how to do the stuff here :)
                //});
                if (result.Succeeded)
                {
                    return Ok(new EventWizardResponseModel
                    {
                        Success = true
                    });
                }
                else
                {
                    return Ok(new EventWizardResponseModel
                    {
                        Success = false
                    });
                }
            }
            else
            {
                return BadRequest("Error");
            }
        }      
    }
}
