using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.EventWizard
{
    public class EventFormDataViewModel
    {
        [Required]
        public int EventCategoryId { get; set; }
        [Required]
        public EventType EventTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ClientPointOfContactId { get; set; }
        public long? FbEventId { get; set; }
        [Required]
        public string MetaDetails { get; set; }
        [Required]
        public string TermsAndConditions { get; set; }
        public bool? IsPublishedOnSite { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public int? PublishedBy { get; set; }
        public int? TestedBy { get; set; }
    }
}
