using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.CreateEventV1
{
    public class EventDetailModel
    {
        public long EventId { get; set; }
        public string EventCategories { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCreate { get; set; }
        public Guid? AltId { get; set; }
        public string Slug { get; set; }
        public string DefaultCategory { get; set; }
        public List<string> ItemsForViewer { get; set; }
        public bool IsEnabled { get; set; }
        public string ParentCategory { get; set; }
        public int ParentCategoryId { get; set; }
        public bool IsPrivate { get; set; }
    }
}