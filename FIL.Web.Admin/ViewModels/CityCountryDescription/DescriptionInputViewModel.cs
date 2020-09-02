using FIL.Web.Kitms.Feel.ViewModels.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.CityCountryDescription
{
    public class DescriptionInputViewModel
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsCountryDescription { get; set; }
        public bool IsStateDescription { get; set; }
        public bool IsCityDescription { get; set; }
        public int StateId { get; set; }
    }
}
