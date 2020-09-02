using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Algolia
{
    public class PlacesSyncReponseModel
    {
        public bool IsSuccess { get; set; }
        public List<FIL.Contracts.Models.PlaceDetail> AllPlaces { get; set; }

    }
}
