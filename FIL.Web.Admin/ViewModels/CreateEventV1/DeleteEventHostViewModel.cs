using System.Collections.Generic;

namespace FIL.Web.Admin.ViewModels.CreateEventV1
{
  public class DeleteEventHostViewModel
  {
    public bool Success { get; set; }
    public bool IsHostStreamLinkCreated { get; set; }
    public short CurrentStep { get; set; }
    public string CompletedStep { get; set; }
  }
}
