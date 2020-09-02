using System;
using System.Collections.Generic;

namespace FIL.Web.Kitms.Feel.ViewModels.CreateEventV1
{
  public class EventDetailsViewModel
  {
    public bool Success { get; set; }
    public bool IsValidLink { get; set; }
    public bool IsDraft { get; set; }
    public string CompletedStep { get; set; }
    public short CurrentStep { get; set; }
    public FIL.Contracts.Models.CreateEventV1.EventDetailModel EventDetail { get; set; }
  }
}
