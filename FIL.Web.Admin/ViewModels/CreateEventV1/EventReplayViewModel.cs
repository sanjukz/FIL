using System.Collections.Generic;
using System.IO;
using FIL.Contracts.Models.CreateEventV1;

namespace FIL.Web.Admin.ViewModels.CreateEventV1
{
  public class EventReplayViewModel
  {
    public bool Success { get; set; }
    public string CompletedStep { get; set; }
    public short CurrentStep { get; set; }
    public long EventId { get; set; }
    public List<ReplayDetailModel> ReplayDetailModel { get; set; }
  }
}
