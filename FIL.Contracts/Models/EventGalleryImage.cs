using System;

namespace FIL.Contracts.Models
{
    public class EventGalleryImage
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long EventId { get; set; }
        public string Name { get; set; }
        public string IsEnabled { get; set; }
        public bool IsBannerImage { get; set; }
        public bool IsHotTicketImage { get; set; }
        public bool IsPortraitImage { get; set; }
        public bool IsVideoUploaded { get; set; }
    }
}