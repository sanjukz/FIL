using System;

namespace FIL.Contracts.Commands.Location
{
    public class LocationCommand : Contracts.Interfaces.Commands.ICommandWithResult<LocationCommandResult>
    {
        public Guid AltId { get; set; }
        public long EventId { get; set; }
        public long EventDetailId { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string PlaceName { get; set; }
        public string TilesSliderImages { get; set; }
        public string DescpagebannerImages { get; set; }
        public string InventorypagebannerImage { get; set; }
        public string GalleryImages { get; set; }
        public string PlacemapImages { get; set; }
        public string TimelineImages { get; set; }
        public string ImmersiveexpImages { get; set; }
        public string ArchdetailImages { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public bool IsEdit { get; set; }
        public Guid ModifiedBy { get; set; }
        public string Description { get; set; }
        public int ParentCategoryId { get; set; }
        public string TimeZoneAbbreviation { get; set; }
        public string TimeZone { get; set; }
    }

    public class LocationCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}