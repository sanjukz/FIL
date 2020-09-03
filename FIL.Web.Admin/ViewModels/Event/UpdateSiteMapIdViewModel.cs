using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Enums;
namespace FIL.Web.Admin.ViewModels.Event
{
    public class UpdateSiteMapIdViewModel
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public short SortOrder { get; set; }
        public Site SiteId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
