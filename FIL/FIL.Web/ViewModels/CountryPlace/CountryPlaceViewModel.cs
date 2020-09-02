using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.CountryPlace
{
    public class CountryPlaceViewModel
    {
        public List<FIL.Contracts.DataModels.CountryPlace> CountryPlace { get; set; }
        public List<FIL.Contracts.DataModels.CountryPlace> CountryCategoryCounts { get; set; }
    }
}
