using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Contracts.Queries.TicketAlert;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Core.Providers;
using FIL.Web.Admin.ViewModels.TicketAlert;
using FIL.Contracts.Queries.Reporting;

namespace FIL.Web.Admin.Controllers.Reporting
{
  public class TicketAlertController : Controller
  {
    private readonly IQuerySender _querySender;

    public TicketAlertController(IQuerySender querySender)
    {
      _querySender = querySender;
    }

    [HttpGet]
    [Route("api/ticketAlertReport/{altId}")]
    public async Task<TicketAlertReportingResponseViewModel> GetTicketAlertReport(Guid altId)
    {
      var queryResult = await _querySender.Send(new TicketAlertReportQuery
      {
        AltId = altId
      });

      return new TicketAlertReportingResponseViewModel
      {
        TicketAlertData = queryResult.TicketAlertReport
      };
    }

    [HttpGet]
    [Route("api/ticketAlertEvents")]
    public async Task<TicketAlertEventsDataViewModel> GetTicketAlertEvents()
    {
      var queryResult = await _querySender.Send(new TicketAlertEventsQuery
      {
      });

      return new TicketAlertEventsDataViewModel
      {
        Events = queryResult.Events
      };
    }
  }
}
