using System;

namespace FIL.Contracts.Models
{
    public class DynamicStadiumCoordinate
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public int VenueId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string SectionCoordinates { get; set; }
        public string SectionTextCoordinates { get; set; }
        public string CircleRectangleValue { get; set; }
        public string Styles { get; set; }
        public bool IsDisplay { get; set; }
        public string PathOffSet { get; set; }
        public int PathTypeId { get; set; }
    }
}