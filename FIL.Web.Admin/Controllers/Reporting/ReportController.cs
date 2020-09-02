using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using FIL.Web.Core.Providers;
using FIL.Contracts.Queries.Creator;
using FIL.Web.Kitms.Feel.ViewModels.Creator;

namespace FIL.Web.Kitms.Feel.Controllers
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
      var queryResult = await _querySender.Send(new FIL.Contracts.Queries.BoxOffice.ReportSubEventsQuery
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

    [HttpPost]
    [Route("api/report/transaction")]
    public async Task<ReportResponseDataViewModel> Get([FromBody] ReportFormDataViewModel model)
    {
      var queryResult = (dynamic)null;
      if (model.EventAltId.ToString() != "00000000-0000-0000-0000-000000000000")
      {
        queryResult = await _querySender.Send(new TransactionReportZsuiteQuery
        {
          UserAltId = userAltId,
          EventAltId = model.EventAltId,
          EventDetailId = model.EventDetailId,
          FromDate = model.FromDate,
          ToDate = model.ToDate
        });
      }
      else
      {
        queryResult = await _querySender.Send(new TransactionReportQueryByDate
        {
          UserAltId = userAltId,
          FromDate = model.FromDate,
          ToDate = model.ToDate
        });
      }

      return new ReportResponseDataViewModel
      {
        Transaction = queryResult.Transaction,
        TransactionDetail = queryResult.TransactionDetail,
        TransactionDeliveryDetail = queryResult.TransactionDeliveryDetail,
        TransactionPaymentDetail = queryResult.TransactionPaymentDetail,
        EventTicketAttribute = queryResult.EventTicketAttribute,
        EventTicketDetail = queryResult.EventTicketDetail,
        TicketCategory = queryResult.TicketCategory,
        CurrencyType = queryResult.CurrencyType,
        EventDetail = queryResult.EventDetail,
        EventAttribute = queryResult.EventAttribute,
        Event = queryResult.Event,
        Venue = queryResult.Venue,
        City = queryResult.City,
        State = queryResult.State,
        Country = queryResult.Country,
        ReportColumns = queryResult.ReportColumns,
        User = queryResult.User,
        IPDetail = queryResult.IPDetail,
        UserCardDetail = queryResult.UserCardDetail,
        TicketFeeDetail = queryResult.TicketFeeDetail
      };
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

    [HttpPost]
    [Route("api/report/inventory")]
    public async Task<InventoryReportResponseDataViewModel> GetInventoryReport([FromBody] ReportFormDataViewModel model)
    {
      var queryResult = await _querySender.Send(new InventoryReportQuery
      {
        UserAltId = userAltId,
        EventAltId = model.EventAltId,
        EventDetailAltId = model.EventDetailAltId
      });
      return new InventoryReportResponseDataViewModel
      {
        TicketCategory = queryResult.TicketCategory,
        EventTicketAttribute = queryResult.EventTicketAttribute,
        EventTicketDetail = queryResult.EventTicketDetail,
        CorporateTicketAllocationDetail = queryResult.CorporateTicketAllocationDetail,
        CorporateTransactionDetail = queryResult.CorporateTransactionDetail,
        Sponsor = queryResult.Sponsor,
        Transaction = queryResult.Transaction,
        TransactionDetail = queryResult.TransactionDetail,
        ReportColumns = queryResult.ReportColumns
      };
    }
  }
}