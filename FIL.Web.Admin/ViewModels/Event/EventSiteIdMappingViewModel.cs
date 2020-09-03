using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Enums;
namespace FIL.Web.Admin.ViewModels.Event
{
    public class EventSiteIdMappingViewModel
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public short SortOrder { get; set; }
        public Site SiteId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
