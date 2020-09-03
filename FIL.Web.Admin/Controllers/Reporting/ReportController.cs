using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Foundation.Senders;
using FIL.Web.Admin.ViewModels.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using FIL.Web.Core.Providers;
using FIL.Contracts.Queries.Creator;
using FIL.Web.Admin.ViewModels.Creator;

namespace FIL.Web.Admin.Controllers
{
  public class TransactionReportController : Controller
  {
    private readonly IQuerySender _querySender;
    private readonly IMemoryCache _memoryCache;
    private readonly ISessionProvider _sessionProvider;
    private static Guid userAltId;

    public TransactionReportController(IQuerySender querySender, ISessionProvider sessionProvider,
        IMemoryCache memoryCache)
    {
      _querySender = querySender;
      _memoryCache = memoryCache;
      _sessionProvider = sessionProvider;
    }

    [HttpGet]
    [Route("api/report/event/all/{altId}")]
    public async Task<ReportEventsResponseDataViewModel> GetEvents(Guid altId)
    {
      userAltId = altId;
      var queryResult = await _querySender.Send(new ReportEventsQuery
      {
        AltId = altId,
        IsFeel = true
      });

      return new ReportEventsResponseDataViewModel
      {
        Events = queryResult.Events
      };
    }

    [HttpGet]
    [Route("api/report/subevent/all/{altId}")]
    public async Task<ReportSubEventsResponseDataViewModel> GetSubEvents(Guid altId)
    {
      var queryResult = await _querySender.Send(new Contracts.Queries.Reporting.ReportSubEventsQuery
      {
        UserAltId = userAltId,
        EventAltId = altId
      });
      return new ReportSubEventsResponseDataViewModel
      {
        SubEvents = queryResult.SubEvents
      };
    }

    [HttpGet]
    [Route("api/report/multiplesubevent/{altIds}")]
    public async Task<ReportSubEventsResponseDataViewModel> GetMultipleSubEvents(string altIds)
    {
      List<Guid> AltIdList = new List<Guid>();
      var altIdsArray = altIds.Split(",");
      foreach (var altId in altIdsArray)
      {
        AltIdList.Add(new Guid(altId));
      }

      var queryResult = await _querySender.Send(new Contracts.Queries.TransactionReport.GetMultipleSubEventsQuery
      {
        UserAltId = userAltId,
        EventAltIds = AltIdList
      });
      return new ReportSubEventsResponseDataViewModel
      {
        SubEvents = queryResult.SubEvents,
        CurrencyTypes = queryResult.CurrencyTypes
      };
    }

    [HttpGet]
    [Route("api/report/subevent/{altId}")]
    public async Task<ReportSubEventsResponseDataViewModel> GetBoSubEvents(Guid altId)
    {
      var session = await _sessionProvider.Get();
      userAltId = session.User.AltId;
      /*var queryResult = await _querySender.Send(new FIL.Contracts.Queries.BoxOffice.ReportSubEventsQuery
      {
        UserAltId = userAltId,
        EventAltId = altId
      }); */
      return new ReportSubEventsResponseDataViewModel
      {
        //SubEvents = queryResult.SubEvents
      };
    }


    [HttpGet]
    [Route("api/paymentgateway/all")]
    public PaymentGatewayResponseViewMaodel GetPaymentGateways()
    {
      List<string> paymentGateways = new List<string>();
      foreach (PaymentGateway pg in Enum.GetValues(typeof(PaymentGateway)))
      {
        paymentGateways.Add(pg.ToString());
      }
      return new PaymentGatewayResponseViewMaodel { PaymentGateways = paymentGateways };
    }

    [HttpGet]
    [Route("api/report/transaction-locator")]
    public async Task<FILTransactionLocatorViewModel> GetTransactionLocator(long TransactionId, string FirstName = null, string LastName = null, string EmailId = null, string UserMobileNo = null)
    {
      var queryResult = await _querySender.Send(new FILTransactionLocatorQuery
      {
        TransactionId = TransactionId,
        FirstName = FirstName,
        LastName = LastName,
        EmailId = EmailId,
        UserMobileNo = UserMobileNo
      });
      return new FILTransactionLocatorViewModel
      {
        Success = queryResult.Success,
        FILTransactionLocator = queryResult.FILTransactionLocator
      };
    }
  }
}