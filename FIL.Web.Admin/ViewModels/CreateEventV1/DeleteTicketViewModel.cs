using System;
using System.Collections.Generic;

namespace FIL.Web.Admin.ViewModels.CreateEventV1
{
  public class DeleteTicketViewModel
  {
    public bool Success { get; set; }
    public short TicketLength { get; set; }
    public Guid ETDAltId { get; set; }
    public long EventId { get; set; }
    public bool IsTicketSold { get; set; }
    public short CurrentStep { get; set; }
    public string CompletedStep { get; set; }
  }
}
