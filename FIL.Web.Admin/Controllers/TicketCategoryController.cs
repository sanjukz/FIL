using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TicketCategory;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.Zipcode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Kitms.Feel.Controllers
{
    public class TicketCategoryController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public TicketCategoryController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpGet]
        [Route("api/get/all/ticketCategoryDetail")]
        public async Task<TicketCategoryDetailResponseViewModel> GetAllTicketCategoryDetails()
        {
            var queryResult = await _querySender.Send(new TicketCategoryDetailQuery { });
            return new TicketCategoryDetailResponseViewModel
            {
                TicketCategoryDetails = queryResult.TicketCategoryDetails,
                TicketCategories = queryResult.TicketCategories
            };
        }
    }
}
