using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIL.Contracts.Models.Export;
using FIL.Contracts.Queries.Export;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Feel.Controllers
{
    public class ExportController : Controller
    {
        private readonly IQuerySender _querySender;

        public ExportController(IQuerySender querySender)
        {
            // TODO: temp memory or external cache on results for export
            _querySender = querySender;
        }

        [HttpGet]
        [Route("api/infinite/export")]
        public async Task<List<FeelExportContainer>> InfiniteExport(Guid? accessToken)
        {
            if (accessToken.HasValue)
            {
                try
                {
                    var result = await _querySender.Send(new FeelExportQuery());
                    return result.Results;
                }
                catch (TaskCanceledException ex)
                {
                    // Check ex.CancellationToken.IsCancellationRequested here.
                    // If false, it's pretty safe to assume it was a timeout.
                }
                
                
            }
            return new List<FeelExportContainer>();
        }
    }
}
