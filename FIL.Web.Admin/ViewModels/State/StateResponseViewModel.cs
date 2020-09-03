using System;
using FIL.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.State
{
    public class StateResponseViewModel
    {
        public List<FIL.Contracts.Models.State> States { get; set; }
    }
}
