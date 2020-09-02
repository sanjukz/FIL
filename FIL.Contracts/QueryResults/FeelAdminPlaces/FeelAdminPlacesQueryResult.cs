using FIL.Contracts.Models.Zoom;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.FeelAdminPlaces
{
    public class FeelAdminPlacesQueryResult
    {
        public bool IsFeelExists { get; set; }
        public List<FIL.Contracts.Models.Event> Events { get; set; }
        public List<FIL.Contracts.Models.EventDetail> EventDetails { get; set; }
        public List<FIL.Contracts.Models.User> Users { get; set; }
        public List<LiveOnlineTransactionDetailResponseModel> MyFeelDetails { get; set; }
        public List<FIL.Contracts.DataModels.EventAttribute> EventAttributes { get; set; }
    }
}