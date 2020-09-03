using System;

namespace FIL.Web.Kitms.ViewModels.TransactionReport
{
  public class TransactionReportRequestViewModel
  {
    public string EventAltId { get; set; }
    public string CurrencyTypes { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
  }
}
