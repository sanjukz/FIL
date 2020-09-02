using System.Collections.Generic;

namespace FIL.Web.Kitms.Feel.ViewModels.CreateEventV1
{
  public class DeleteEventHostViewModel
  {
    public bool Success { get; set; }
    public bool IsHostStreamLinkCreated { get; set; }
    public short CurrentStep { get; set; }
    public string CompletedStep { get; set; }
  }
}
