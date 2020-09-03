using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIL.Web.Admin.ViewModels.CreateEventV1
{
  public class EventScheduleViewModel
  {
    public bool Success { get; set; }
    public bool IsValidLink { get; set; }
    public bool IsDraft { get; set; }
    public string CompletedStep { get; set; }
    public short CurrentStep { get; set; }
    public FIL.Contracts.Models.CreateEventV1.EventScheduleModel EventScheduleModel { get; set; }
  }

  public class EventRecurranceViewModel
  {
    [Required]
    public long EventId { get; set; }
    [Required]
    public long EventDetailId { get; set; }
    public bool Success { get; set; }
    public bool IsValidLink { get; set; }
    public bool IsDraft { get; set; }
    public string CompletedStep { get; set; }
    public short CurrentStep { get; set; }
    public long EventScheduleId { get; set; }
    public long ScheduleDetailId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public FIL.Contracts.Enums.OccuranceType OccuranceType { get; set; }
    public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
    public string DayIds { get; set; }
    public string LocalStartTime { get; set; }
    public string LocalEndTime { get; set; }
    public int OccuranceCount { get; set; }
    public string TimeZoneAbbrivation { get; set; }
    public string TimeZoneOffset { get; set; }
  }

  public class EventRecurranceResponseViewModel
  {
    public bool Success { get; set; }
    public bool IsValidLink { get; set; }
    public bool IsDraft { get; set; }
    public List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel> EventRecurranceScheduleModel { get; set; }
  }
}
