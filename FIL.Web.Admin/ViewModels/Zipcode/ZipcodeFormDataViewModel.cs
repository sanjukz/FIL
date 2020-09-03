using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Zipcode
{
    public class ZipcodeFormDataViewModel
    {
        public string zipcode { get; set; }
        public string Region { get; set; }
        public Guid CityAltId { get; set; }
    }
}
