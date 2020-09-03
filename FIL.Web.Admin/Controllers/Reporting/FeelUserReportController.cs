using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Contracts.Commands.TicketAlert;
using FIL.Contracts.Enums;
using System.Collections.Generic;
using System.Linq;
using FIL.Web.Core;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Core.Providers;
using FIL.Web.Kitms.ViewModels.TransactionReport;
using FIL.Contracts.Queries.Reporting;
using FIL.Web.Admin.ViewModels.Reporting;
using FIL.Caching.Contracts.Interfaces;

namespace FIL.Web.Controllers
{
    public class FeelUserReportController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ICacheHelper _cacheHelper;

        public FeelUserReportController(IQuerySender querySender, ICacheHelper cacheHelper)
        {
            _querySender = querySender;
            _cacheHelper = cacheHelper;
        }

        [HttpGet]
        [Route("api/get/feeluserreport")]
        public async Task<FeelUserReportResponseDataViewModel> GetTicketAlertReport()
        {
            try
            {
                var queryResult = await _querySender.Send(new FIL.Contracts.Queries.Reporting.FeelUserQuery
                {
                });

                return new FeelUserReportResponseDataViewModel
                {
                    FeelUsers = queryResult.FeelUsers
                };
            }
            catch (Exception e)
            {
                return new FeelUserReportResponseDataViewModel
                {

                };
            }
        }
    }
}
