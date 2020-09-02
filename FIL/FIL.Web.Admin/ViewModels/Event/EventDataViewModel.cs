using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Enums;
namespace FIL.Web.Kitms.Feel.ViewModels.Event
{
    public class EventDataViewModel
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public int EventCategoryId { get; set; }
        public EventType EventTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFeel { get; set; }
        public EventSource EventSourceId { get; set; }
        public bool? IsPublishedOnSite { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public string ImagePath { get; set; }
    }
}
