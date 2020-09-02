using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Event
{
    public class EventListResponseViewModel
    {
        
        public long Id { get; set; }
        
        public int EventCategoryId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string MainImagePath { get; set; }
        
    }
}
