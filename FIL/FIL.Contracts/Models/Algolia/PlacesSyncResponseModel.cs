using System.Collections.Generic;

namespace FIL.Contracts.Models.Algolia
{
    public class PlacesSyncReponseModel
    {
        public bool IsSuccess { get; set; }
        public List<FIL.Contracts.Models.PlaceDetail> AllPlaces { get; set; }
    }
}