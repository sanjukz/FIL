using System;

namespace FIL.Contracts.Models.CreateEventV1
{
    public class EventImageModel
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public Guid? EventAltId { get; set; }
        public bool? IsBannerImage { get; set; }
        public bool? IsHotTicketImage { get; set; }
        public bool? IsPortraitImage { get; set; }
        public bool? IsVideoUploaded { get; set; }
    }
}