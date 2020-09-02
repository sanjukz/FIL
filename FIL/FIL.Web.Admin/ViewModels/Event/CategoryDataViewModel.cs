using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Event
{
  public class CategoryDataViewModel
  {
    public int Order { get; set; }
    public string DisplayName { get; set; }
    public string Slug { get; set; }
    public bool IsHomePage { get; set; }
    public int CategoryId { get; set; }
    public bool IsFeel { get; set; }
    public int Value { get; set; }
    public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
  }
}
