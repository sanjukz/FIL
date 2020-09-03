using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.EventCreation
{
    public class GetEventsResponseViewModel
    {
        public IEnumerable<FIL.Contracts.Models.Event> Event { get; set; }
    }
}
