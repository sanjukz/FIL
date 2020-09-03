using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.CreateEventV1
{
  public class DeleteSponsorViewModel
  {
    public bool Success { get; set; }
    public short CurrentStep { get; set; }
    public string CompletedStep { get; set; }
  }
}
