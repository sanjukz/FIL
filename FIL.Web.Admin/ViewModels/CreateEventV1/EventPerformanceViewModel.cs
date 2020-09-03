using System;
using System.Collections.Generic;

namespace FIL.Web.Admin.ViewModels.CreateEventV1
{
  public class EventPerformanceViewModel
  {
    public bool Success { get; set; }
    public long EventId { get; set; }
    public string OnlineEventType { get; set; }
    public string CompletedStep { get; set; }
    public short CurrentStep { get; set; }
    public System.Guid? EventAltId { get; set; }
    public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
    public FIL.Contracts.Models.CreateEventV1.PerformanceTypeModel PerformanceTypeModel { get; set; }
  }
}
