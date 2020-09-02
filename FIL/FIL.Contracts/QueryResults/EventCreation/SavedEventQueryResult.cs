using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.EventCreation
{
    public class SavedEventQueryResult
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string PlaceName { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public string History { get; set; }
        public int HistoryId { get; set; }
        public string Highlights { get; set; }
        public int HighlightsId { get; set; }
        public string Immersiveexperience { get; set; }
        public int ImmersiveexperienceId { get; set; }
        public string Archdetail { get; set; }
        public int ArchdetailId { get; set; }
        public string TilesSliderImages { get; set; }
        public string DescpagebannerImage { get; set; }
        public string InventorypagebannerImage { get; set; }
        public string GalleryImages { get; set; }
        public string PlacemapImages { get; set; }
        public string TimelineImages { get; set; }
        public string ImmersiveexpImages { get; set; }
        public string ArchdetailImages { get; set; }
        public string Metatags { get; set; }
        public string Metatitle { get; set; }
        public string Metadescription { get; set; }
        public int Categoryid { get; set; }
        public string Subcategoryid { get; set; }
        public string AmenityId { get; set; }
        public string HourTimeDuration { get; set; }
        public string MinuteTimeDuration { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string TagIds { get; set; }
        public List<EventHostMapping> EventHostMappings { get; set; }
    }
}