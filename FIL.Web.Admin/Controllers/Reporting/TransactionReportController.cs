using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Web.Kitms.ViewModels.TransactionReport;
using FIL.Web.Admin.ViewModels.TransactionReport;

namespace FIL.Web.Controllers
{
  public class TransactionReportController : Controller
  {
    private readonly IQuerySender _querySender;

    public TransactionReportController(IQuerySender querySender)
    {
      _querySender = querySender;
    }
    [HttpPost]
    [Route("api/get/reports")]
    public async Task<TransactionReportResponseViewModel> GetTransactionReport([FromBody] TransactionReportRequestViewModel model)
    {
      var queryResult = await _querySender.Send(new FAPTransactionReportQuery
      {
        EventAltId = model.EventAltId,
        CurrencyTypes = model.CurrencyTypes,
        FromDate = model.FromDate,
        ToDate = model.ToDate
      });

      return new TransactionReportResponseViewModel
      {
        TransactionReports = queryResult.TransactionReportData
      };
    }

    [HttpGet]
    [Route("api/report/get-places/{userAltId}")]
    public async Task<GetAllReportPlacesResponseModel> GetPlaces(Guid userAltId)
    {
      var queryResult = await _querySender.Send(new FAPGetPlacesQuery
      {
        isFeel = true,
        UserAltId = userAltId
      });

      return new GetAllReportPlacesResponseModel
      {
        Places = queryResult.Places
      };
    }
  }
}
