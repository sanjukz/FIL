using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Country
{
    public class CountryFormDataViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string IsoAlphaTwoCode { get; set; }

        [Required]
        public string IsoAlphaThreeCode { get; set; }
    }
}
