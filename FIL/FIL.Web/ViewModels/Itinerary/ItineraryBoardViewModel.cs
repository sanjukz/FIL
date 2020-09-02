using FIL.Web.Feel.ViewModels.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Itinerary
{
    public class ItineraryBoardViewModel
    {
        public List<List<FIL.Contracts.DataModels.SearchVenue>> ItineraryBoardData { get; set; }
        public List<List<FIL.Contracts.DataModels.SearchVenue>> ItineraryBoardCopyData { get; set; }
        public RootObject RootObject { get; set; }
        public string CardId;
        public string SourceLaneId;
        public string TargetLaneId;
        public int Position;
        public Card CardDetails;
        public bool IsDelete;
        public bool IsAddNew;
        public int PlaceId;
    }

    public class RootObject
    {
        public List<Lane> Lanes;
    }
    public class Lane
    {
        public string id;
        public string title;
        public string label;
        public Style style;
        public List<Card> Cards;
    }
    public class Card
    {
        public string id;
        public string title;
        public string label;
        public string description;
    }
    public class Style
    {
        public int width;
    }
}
