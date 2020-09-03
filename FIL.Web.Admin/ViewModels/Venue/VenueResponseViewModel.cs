using System;
using FIL.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Venue
{
    public class VenueResponseViewModel
    {
        public List<FIL.Contracts.Models.Venue> Venues { get; set; }
    }
}
