using System;
using System.Collections.Generic;

namespace FIL.Web.Kitms.Feel.ViewModels.CreateEventV1
{
  public class EventHostsViewModel
  {
    public bool Success { get; set; }
    public long EventId { get; set; }
    public bool IsValidLink { get; set; }
    public bool IsDraft { get; set; }
    public string CompletedStep { get; set; }
    public short CurrentStep { get; set; }
    public List<FIL.Contracts.DataModels.EventHostMapping> EventHostMapping { get; set; }
  }
}
