using FIL.Contracts.Models.Tiqets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Tiqets
{
    public class UpdateDetailResponseModel
    {
        public bool success { get; set; }
        public List<TiqetImageModel> TiqetImagesList { get; set; }

    }
}
