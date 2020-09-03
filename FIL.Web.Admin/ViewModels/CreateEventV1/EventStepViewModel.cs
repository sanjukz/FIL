namespace FIL.Web.Admin.ViewModels.CreateEventV1
{
  public class EventStepViewModel
  {
    public long EventId { get; set; }
    public bool Success { get; set; }
    public string CompletedStep { get; set; }
    public int CurrentStep { get; set; }
  }
}
