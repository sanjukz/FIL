using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Enums;

namespace FIL.Web.Feel.ViewModels.Search
{
    public class EventSearchResult
    {
        public string ParentCategory { get; set; }
        public string Name { get; set; }
        public Guid AltId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string RedirectUrl { get; set; }
    }
}
