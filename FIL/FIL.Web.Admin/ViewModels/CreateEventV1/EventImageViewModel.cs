using System.Collections.Generic;
using System.IO;

namespace FIL.Web.Kitms.Feel.ViewModels.CreateEventV1
{
  public class EventImageViewModel
  {
    public bool Success { get; set; }
    public bool IsValidLink { get; set; }
    public bool IsDraft { get; set; }
    public string CompletedStep { get; set; }
    public short CurrentStep { get; set; }
    public FIL.Contracts.Models.CreateEventV1.EventImageModel EventImageModel { get; set; }
  }
}
