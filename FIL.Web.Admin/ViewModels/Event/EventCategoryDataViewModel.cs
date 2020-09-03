using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Event
{
    public class EventCategoryDataViewModel
    {
        [Required]
        public List<CategoryDataViewModel> categories { get; set; }
    }
}
