using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.AuthedRoleFeature
{
    public class AuthedFeatureResponseViewModel
    {
        public IEnumerable<Feature> Feature { get; set; }
    }
}
