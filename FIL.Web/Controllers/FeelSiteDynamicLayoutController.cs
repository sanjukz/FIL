using System;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using System.Threading.Tasks;
using FIL.Logging;
using System.Diagnostics;
using FIL.Web.Feel.ViewModels;
using FIL.Contracts.Queries;

namespace FIL.Web.Feel.Controllers
{
    public class FeelSiteDynamicLayoutController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ILogger _logger;
        private readonly Guid ValueRetailUid = Guid.NewGuid();
        public Stopwatch stopWatch = new Stopwatch();

        public FeelSiteDynamicLayoutController(ICommandSender commandSender, IQuerySender querySender, ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/section/{pageId}")]
        public async Task<FeelSiteDynamicLayoutSectionViewModel> GetLayoutSections(int pageId)
        {
            try
            {
                var queryResult = await _querySender.Send(new FeelSiteDynamicLayoutSectionQuery
                {
                    PageId = pageId
                });

                if (queryResult != null)
                {
                    return new FeelSiteDynamicLayoutSectionViewModel
                    {
                        PageName = queryResult.PageName,
                        Sections = queryResult.Sections
                    };
                }
                else
                {
                    throw new ArgumentNullException("No section found for this page.");
                }
            }
            catch
            {
                _logger.Log(Logging.Enums.LogCategory.Error, $"FAP page {pageId} returned no sections.");
                return new FeelSiteDynamicLayoutSectionViewModel();
            }
        }
    }
}
