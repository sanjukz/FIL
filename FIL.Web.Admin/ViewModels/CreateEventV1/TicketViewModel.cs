using System.Collections.Generic;
using System.IO;

namespace FIL.Web.Kitms.Feel.ViewModels.CreateEventV1
{
  public class TicketViewModel
  {
    public long EventId { get; set; }
    public long EventDetailId { get; set; }
    public bool IsCreate { get; set; }
    public bool Success { get; set; }
    public bool IsValidLink { get; set; }
    public bool IsDraft { get; set; }
    public string CompletedStep { get; set; }
    public short CurrentStep { get; set; }
    public List<FIL.Contracts.Models.CreateEventV1.TicketModel> Tickets { get; set; }
  }
}
