using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Search
{
    public class SearchResponseViewModel
    {
        public List<FIL.Contracts.Models.Search> Countries { get; set; }
        public List<FIL.Contracts.Models.Search> States { get; set; }
        public List<FIL.Contracts.Models.Search> Cities { get; set; }
        public List<EventSearchResult> CategoryEvents { get; set; }
    }
}
